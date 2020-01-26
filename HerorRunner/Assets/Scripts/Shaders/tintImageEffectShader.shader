Shader "Hidden/tintImageEffectShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Multilayer("Multilayer",Float) = 0.5
    }
    SubShader
    {

        Cull Off ZWrite Off ZTest Always

         Tags {"RenderType"="Transparent" Queue = Transparent}
         Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {   
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _Multilayer;

            fixed4 frag (v2f i) : SV_Target
            {
              //TODO: Write more complex shader -> raymarching & underwatereffect
              fixed4 col = tex2D(_MainTex, i.uv);
              if(_Multilayer > 0){
                
                // just invert the colors
                col.r = 1 - col.r * (_Multilayer/4);
                col.g = 1 - col.g * (_Multilayer/2);
                col.b = 1 - col.b * (_Multilayer/3);
                col.rbg = col.rbg / 2;
                col.a = 0.1;
                
               }
               return col;
            }
            ENDCG
        }
    }
}
