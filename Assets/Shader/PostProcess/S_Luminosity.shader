Shader "Hidden/Luminosity"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [range(0.01,2)]_LuminosityStrength ("Luminosity", float) = 50
        [range(0,1)]_VignetteStrength ("VignetteStrength", float) = 0
        _Tint ("Color Vignette", Color) = (0,0,0,0)
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
            float _LuminosityStrength;
            float _VignetteStrength;
            half4 _Tint;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                //Tint
                //col *= _Tint;

                //Luminosité
                col.rgb = pow(col, 1 / _LuminosityStrength);

                //Vignette
                half2 uvCoord = i.uv;

                uvCoord = (uvCoord - 0.5) * 2;

                half uvDot = dot(uvCoord, uvCoord);

                half vignette = 1 - uvDot * _VignetteStrength;

                fixed4 colTintVignette = vignette * _Tint;
                
                col.rgb *= colTintVignette;
                
                return col;
            }
            ENDCG
        }
    }
}
