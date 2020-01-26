Shader "Hidden/tintImageEffectShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MaxTime("MaxTime",Float) = 0
        _TimeLeft("TimeLeft",Float) = 0
    }
    SubShader
    {

        Cull Off 
        ZWrite Off 
        ZTest Always
        Lighting Off
        

         Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
        }
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
            float _MaxTime;
            float _TimeLeft;
            float _Multiplier;

            fixed4 frag (v2f i) : SV_Target
            {
              //TODO: Write more complex shader -> raymarching & underwatereffect
              fixed4 col = tex2D(_MainTex, i.uv);
              _Multiplier =  1-_TimeLeft/_MaxTime;
              
               if(_TimeLeft<_MaxTime-16){
                    col.b = col.b + 0.7*cos(_TimeLeft-1.5);
               }
               
               if(_TimeLeft < _MaxTime-13){
                    col.g = col.g + 0.8*sin(0.4*_TimeLeft);
               }
               
               if(_TimeLeft<_MaxTime-8){
                    col.r = col.r + 0.7*sin(0.4*(_TimeLeft-3));
                    col = (col-0.5)*(0.3+_Multiplier*0.9) + 0.5;
               }

               if(_Multiplier > 0.2 && _Multiplier < 0.8){
                   col.a = 1 - _Multiplier;
               }else if (_Multiplier >= 0.8){
                   col.a = 0.2;
               }
               return col;
            }

            ENDCG
        }
    }
}
