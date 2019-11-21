Shader "Unlit/live shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		tintColor("Tint Color",Color) = (0.5, 0.5, 0.5, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        
		Pass
        {
            CGPROGRAM  //Ab hier unser Shader Code
            #pragma vertex vert //name der Vertex Shader Function
            #pragma fragment frag //name der Fragment Shader Function
     

            #include "UnityCG.cginc"

            struct vertexData
            {
                float4 position : POSITION;
				float normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct fragmentData
            {
                float2 uv : TEXCOORD0;
                float4 position : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float4 tintColor;

            fragmentData vert (vertexData vertex)
            {
                fragmentData output;
				output.position = UnityObjectToClipPos(vertex.position + vertex.normal);
				output.uv = TRANSFORM_TEX(vertex.uv, _MainTex);
                return output;
            }

            fixed4 frag (fragmentData fragment) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, floor(fragment.uv*50)/5) * tintColor;
				//col = 1 - col;
				//col = col > 0.9;
				col = col* fragment.uv.x;
                return col;
            }
            ENDCG
        }
    }
}
