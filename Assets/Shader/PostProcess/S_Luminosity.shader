// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/Luminosity"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [range(0.01,2)]_LuminosityStrength ("Luminosity", float) = 50
        [range(0,1)]_VignetteStrength ("Vignette Strength", float) = 0
        _LutColorGradeTex ("Lut Color Gradient Texture", 2D) = "white" {}
        _LutColorTransition ("Lut Color Transition", float) = 0
        _Tint ("Color Vignette", Color) = (0,0,0,0)
        _DepthRoundness("Depth Roundness", float) = 1.0
        _DepthFactor("Depth Factor", float) = 1.0
        _DepthPow("Depth Pow", float) = 1.0
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
                float4 screenPos : TEXCOORD1;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 screenPos : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                
                // compute depth
                o.screenPos = ComputeScreenPos(o.vertex);
                COMPUTE_EYEDEPTH(o.screenPos.z);
                
                return o;
            }

            sampler2D _MainTex;
            float _LuminosityStrength;
                        
            float _VignetteStrength;
            float _DepthRoundness;
            float _LutColorTransition;
            float _DepthPow;
            float _DepthFactor;
            
            sampler2D _LutColorGradeTex;
            
            half4 _Tint;

            UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);

            half4 ClolorGrade(sampler2D colorGradeTex, half4 colMain)
            {
                half4 gradeColor = (0,0,0,1);
                gradeColor.r = tex2D(colorGradeTex, float2(colMain.x,0)).x;
                gradeColor.g = tex2D(colorGradeTex, float2(colMain.y,0)).y;
                gradeColor.b = tex2D(colorGradeTex, float2(colMain.z,0)).z;

                gradeColor = half4(lerp(colMain.rgb, gradeColor.rgb, _LutColorTransition),1);
                
                return gradeColor;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                //Tint
                //col *= _Tint;

                //Luminosité
                col.rgb = pow(col, 1 / _LuminosityStrength);


                //Apply Tint
                col.rgb *= _Tint;

                //Lut Color Grading
                half4 gradeColor = ClolorGrade(_LutColorGradeTex, col);
                col.rgb *= gradeColor.rgb;

                
                //Vignette
                half2 uvCoord = i.uv;
                uvCoord = (uvCoord - 0.5) * 2;
                half uvDot = dot(uvCoord, uvCoord);
                half vignette = 1 - uvDot * _VignetteStrength;
                col.rgb *= vignette;
                
                // compute depth
                float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)));
                float depth = sceneZ - i.screenPos.z;

                fixed depthFading = saturate(abs(pow(depth, _DepthPow)) / _DepthFactor);

                //Faire un arrondit avec le DepthView
                //half depthFinal = depthFading * (1 - uvDot * _DepthRoundness);
                //depthFinal = clamp(0,1,depthFinal);
                
                col = lerp(fixed4 (0,0,0,1), col, depth);
                //col *= depthFinal;
                return col;
            }
            ENDCG
        }
    }
}