// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/Rainbow"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        
    }
    SubShader
    {

        Cull Off 
        ZWrite Off 
        ZTest Always
        

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
            
            sampler2D _MainTex;
            float _MaxTime;
            float _TimeLeft;
            float _Multiplier;
            
            
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
            float2 _OldTexCoord;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                if(_TimeLeft > 22 && _TimeLeft < 30){
                    v2f temp;
                    _OldTexCoord = o.uv;
                    o.uv[0] = _OldTexCoord[1];
                    o.uv[1] = _OldTexCoord[0];
                    
                }
 
                if(_TimeLeft > 10 && _TimeLeft <= 17){
                    _OldTexCoord = o.uv;
                    o.uv[0] =1- _OldTexCoord[0];
                    o.uv[1] =1- _OldTexCoord[1];
                }
                
                if(o.uv[0] < 0){
                    o.uv[0] = o.uv[0]+1;
                }
                if(o.uv[0] > 1){
                    o.uv[0] = o.uv[0]-1;
                }
                if(o.uv[1] < 0){
                    o.uv[1] = o.uv[1]+1;
                }
                if(o.uv[1] > 1){
                    o.uv[1] = o.uv[1]-1;
                }
                    
                return o;
            }

            

            fixed4 frag (v2f i) : SV_Target
            {
              //TODO: Write more complex shader -> raymarching & underwatereffect
              fixed4 col = tex2D(_MainTex, i.uv);
              _Multiplier =  1-_TimeLeft/_MaxTime;


                if(_TimeLeft > _MaxTime/2){
                   if(_TimeLeft<_MaxTime-16){
                        col.b = col.b + _Multiplier/2;
                        
                   }
                   
                   if(_TimeLeft < _MaxTime-13){
                       col.g = col.g + _Multiplier/2;
                   }
                   
                   if(_TimeLeft<_MaxTime-8){
                        col.r = col.r + _Multiplier/4;                        
                   }
                }
                if(_TimeLeft <= _MaxTime/2){
                     col.b = col.b + 0.7*cos(_TimeLeft-1.5);
                     col.g = col.g + 0.8*sin(0.4*_TimeLeft);
                     col.r = col.r + 0.7*sin(0.4*(_TimeLeft-3));
                }
               

               if(_Multiplier > 0.2 && _Multiplier < 0.8){
                   col.a = 1 - _Multiplier;
                   col = (col-0.5)*(0.3+_Multiplier*0.9) + 0.5;
               }else if (_Multiplier >= 0.8){
                   col.a = 0.2;
                   col = (col-0.5)*(0.3+0.8*0.9) + 0.5;
               }
               return col;
            }

            ENDCG
        }
    }
}
