Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TestText ("Test", 2D) = "white" {}
        _Color ("Aboba", Color) = (1,1,1,1)
        _TranspA ("Transparent", Range(0,1)) = 1
        _RangeOverclap("RangeLine", Range(0,0.5)) = 0.486
    }
    SubShader
    {
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite ON

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
            
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            fixed _TranspA;
            fixed _RangeOverclap;
            sampler2D _MainTex;
            fixed4 _Color;
            float4 _MainTex_ST;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = fixed4(0,0,0,0);
                fixed circle = ((i.uv.x - 0.5f) * (i.uv.x - 0.5f)) + ((i.uv.y - 0.5f) * (i.uv.y - 0.5f));
                if(circle <= pow(0.5f,2) && circle >= pow(_RangeOverclap,2))
                {
                    {
                        col.r = 255;
                        col.a = _TranspA;
                    }
                }
                return col;
            }
            ENDCG
        }
    }
}
