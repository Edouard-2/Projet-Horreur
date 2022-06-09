Shader "Hidden/Luminosity"
{
    Properties
    {
        //Main Texture
        _MainTex ("Texture", 2D) = "white" {}
        //Brightness
        _LuminosityStrength ("Luminosity", float) = 50
        //Vignette
        _VignetteTex ("Texture de la vignette", 2D) = "white" {}
        _VignetteStrength ("Vignette Strength", float) = 0
        _VignetteSoft ("Vignette Softness", float) = 0
        //Lut Table
        _LutColorGradeTex ("Lut Color Gradient Texture", 2D) = "white" {}
        _LutColorTransition ("Lut Color Transition", float) = 0
        //Tint
        _Tint ("Color Vignette", Color) = (0,0,0,0)
        //Depth
        _DepthLevel("Depth Level", float) = 1.0
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
                float4 screen_pos : TEXCOORD1;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 screen_pos : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                
                // compute depth
                o.screen_pos = ComputeScreenPos(o.vertex);
                COMPUTE_EYEDEPTH(o.screen_pos.z);
                
                return o;
            }

            //Déclaration variables
            sampler2D _MainTex;
            float _LuminosityStrength;
            sampler2D _VignetteTex;
            float _VignetteStrength;
            float _VignetteSoft;
            float _LutColorTransition;
            sampler2D _LutColorGradeTex;
            float _DepthLevel;
            float _DepthRoundness;
            float _DepthPow;
            float _DepthFactor;
            half4 _Tint;

            UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);

            //Update la lut table
            half4 ColorGrade(sampler2D colorGradeTex, half4 colMain)
            {
                half4 gradeColor;
                gradeColor.r = tex2D(colorGradeTex, float2(colMain.x,0)).x;
                gradeColor.g = tex2D(colorGradeTex, float2(colMain.y,0)).y;
                gradeColor.b = tex2D(colorGradeTex, float2(colMain.z,0)).z;

                gradeColor = half4(lerp(colMain.rgb, gradeColor.rgb, _LutColorTransition),1);
                
                return gradeColor;
            }

            //Function remap
            float Unity_Remap_Float(float InMin, float InMax, float OutMin, float OutMax , float Value)
            {
                float Out = OutMin + (Value - InMin) * (OutMax - OutMin) / (InMax - InMin);
                return Out;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                
                //Luminosité
                col.rgb = pow(col, 1 / _LuminosityStrength);


                //Apply Tint
                col.rgb *= _Tint;

                //Lut Color Grading
                half4 gradeColor = ColorGrade(_LutColorGradeTex, col);
                col.rgb *= gradeColor.rgb;

                //Calculate Depth Strength
                _DepthPow = Unity_Remap_Float(0, 1, 0, _DepthPow , _DepthLevel);

                //Vignette Setp 1
                half2 uvCoord = i.uv;
                uvCoord = uvCoord - 0.5;
                half uvDot = dot(uvCoord, uvCoord);

                //Compute depth
                float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.screen_pos)));
                float depth = sceneZ - i.screen_pos.z;
                fixed depthFading = saturate(abs(pow(depth, _DepthPow)) / _DepthFactor);

                //Faire un arrondit avec le DepthView
                half depthRound = depthFading * (1 - uvDot * _DepthRoundness);
                depthRound = clamp(0,1,depthRound);
                fixed4 depthCol = lerp(col,fixed4(0,0,0,1), depthRound);
                
                //Vignette Setp 2
                half vignette = 1- uvDot * _VignetteStrength;
                vignette = smoothstep(_VignetteStrength, _VignetteStrength - _VignetteSoft, vignette);
                fixed4 vignetteTex = tex2D(_VignetteTex, i.uv);
                depthCol.rgb = lerp(depthCol, vignetteTex, vignette);
                
                return depthCol;
            }
            ENDCG
        }
    }
}