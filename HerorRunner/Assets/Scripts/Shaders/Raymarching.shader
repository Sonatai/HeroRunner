// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/Raymarching"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
    
        
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            
           uniform sampler2D _MainTex;
           uniform float4x4 _FrustumCornersES;
           uniform float4 _MainTex_TexelSize;
           uniform float4x4 _CameraInvViewMatrix;
           uniform float3 _CameraWS;
           uniform float3 _LightDir;


            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 ray : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                 v2f o;
    
                // Index passed via custom blit function in RaymarchGeneric.cs
                half index = v.vertex.z;
                v.vertex.z = 0.1;
                
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv.xy;
                
                #if UNITY_UV_STARTS_AT_TOP
                if (_MainTex_TexelSize.y < 0)
                    o.uv.y = 1 - o.uv.y;
                #endif
            
                // Get the eyespace view ray (normalized)
                o.ray = _FrustumCornersES[(int)index].xyz;
            
                // Transform the ray from eyespace to worldspace
                // Note: _CameraInvViewMatrix was provided by the script
                o.ray = mul(_CameraInvViewMatrix, o.ray);
                
                // Dividing by z "normalizes" it in the z axis
                // Therefore multiplying the ray by some number i gives the viewspace position
                // of the point on the ray with [viewspace z]=i
                o.ray /= abs(o.ray.z);
            
                // Transform the ray from eyespace to worldspace
                o.ray = mul(_CameraInvViewMatrix, o.ray);
                
                return o;
            }
            
            // Torus
            // t.x: diameter
            // t.y: thickness
            // Adapted from: http://iquilezles.org/www/articles/distfunctions/distfunctions.htm
            float sdTorus(float3 p, float2 t)
            {
                float2 q = float2(length(p.xz) - t.x, p.y);
                return length(q) - t.y;
            }
            
            // This is the distance field function.  The distance field represents the closest distance to the surface
            // of any object we put in the scene.  If the given point (point p) is inside of an object, we return a
            // negative answer.
            float map(float3 p) {
                return sdTorus(p, float2(1, 0.2));
            }
            float3 calcNormal(in float3 pos)
            {
                // epsilon - used to approximate dx when taking the derivative
                const float2 eps = float2(0.001, 0.0);
            
                // The idea here is to find the "gradient" of the distance field at pos
                // Remember, the distance field is not boolean - even if you are inside an object
                // the number is negative, so this calculation still works.
                // Essentially you are approximating the derivative of the distance field at this point.
                float3 nor = float3(
                    map(pos + eps.xyy).x - map(pos - eps.xyy).x,
                    map(pos + eps.yxy).x - map(pos - eps.yxy).x,
                    map(pos + eps.yyx).x - map(pos - eps.yyx).x);
                return normalize(nor);
            }
            // Raymarch along given ray
            // ro: ray origin
            // rd: ray direction
            uniform sampler2D _ColorRamp;
            fixed4 raymarch(float3 ro, float3 rd, float s) {
                fixed4 ret = fixed4(0,0,0,0);

                const int maxstep = 64;
                const float drawdist = 40; // draw distance in unity units
            
                float t = 0; // current distance traveled along ray
                for (int i = 0; i < maxstep; ++i) {
                    if (t >= s || t > drawdist) { // check draw distance in additon to depth
                        ret = fixed4(0, 0, 0, 0);
                        break;
                    }

                    float3 p = ro + rd * t; // World space position of sample
                    float2 d = map(p);      // Sample of distance field (see map())
            
                    // If the sample <= 0, we have hit something (see map()).
                    // If t > drawdist, we can safely bail because we have reached the max draw distance
                    if (d.x < 0.001 || t > drawdist) {
                        // Simply return the number of steps taken, mapped to a color ramp.
                        float perf = (float)i / maxstep;
                        return fixed4(tex2D(_ColorRamp, float2(perf, 0)).xyz, 1);
                    }
            
                    t += d;
                }
            
                // By this point the loop guard (i < maxstep) is false.  Therefore
                // we have reached maxstep steps.
                return fixed4(tex2D(_ColorRamp, float2(1, 0)).xyz, 1);
            }
            
            uniform sampler2D _CameraDepthTexture;
            
            fixed4 frag (v2f i) : SV_Target
            {
                  // ray direction
                float3 rd = normalize(i.ray.xyz);
                // ray origin (camera position)
                float3 ro = _CameraWS;
            
                float2 duv = i.uv;
                #if UNITY_UV_STARTS_AT_TOP
                if (_MainTex_TexelSize.y < 0)
                    duv.y = 1 - duv.y;
                #endif
            
                // Convert from depth buffer (eye space) to true distance from camera
                // This is done by multiplying the eyespace depth by the length of the "z-normalized"
                // ray (see vert()).  Think of similar triangles: the view-space z-distance between a point
                // and the camera is proportional to the absolute distance.
                float depth = LinearEyeDepth(tex2D(_CameraDepthTexture, duv).r);
                depth *= length(i.ray.xyz);
            
                fixed3 col = tex2D(_MainTex,i.uv);
                fixed4 add = raymarch(ro, rd, depth);
            
                // Returns final color using alpha blending
                return fixed4(col*(1.0 - add.w) + add.xyz * add.w,1.0);
            }
            
            

            
            ENDCG
        }
    }
}
