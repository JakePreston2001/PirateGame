Shader "Custom/StylizedWater"
{
    Properties
    {
        [Header(Colors)]
        [HDR] _Color("Color", Color) = (1,1,1,1)
        [HDR]_FogColor("Fog Color", Color) = (1,1,1,1)
        [HDR]_IntersectionColor("Intersection color", Color) = (1,1,1,1)

        [Header(Thresholds)]
        _IntersectionThreshold("Intersction threshold", float) = 0
        _FogThreshold("Fog threshold", float) = 0
        _FoamThreshold("Foam threshold", float) = 0

        [Header(Normal maps)]
        [Normal]_NormalA("Normal A", 2D) = "bump" {}
        [Normal]_NormalB("Normal B", 2D) = "bump" {}
        _NormalStrength("Normal strength", float) = 1
        _NormalPanningSpeeds("Normal panning speeds", Vector) = (0,0,0,0)

        [Header(Foam)]
        _FoamTexture("Foam texture", 2D) = "white" {}
        _FoamTextureSpeedX("Foam texture speed X", float) = 0
        _FoamTextureSpeedY("Foam texture speed Y", float) = 0
        _FoamLinesSpeed("Foam lines speed", float) = 0
        _FoamIntensity("Foam intensity", float) = 1

        [Header(Misc)]
        _RenderTexture("Render texture", 2D) = "black" {}
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _FresnelPower("Fresnel power", float) = 1

        [Header(Waves)]
        _WaveA("Wave A (dir, steepness, wavelength)", Vector) = (0.37,0.21,0.22,60)
        _WaveB("Wave B (dir, steepness, wavelength)", Vector) = (0.6,0.01,0.18,30)
        _WaveC("Wave B (dir, steepness, wavelength)", Vector) = (1,0,0.08,15)

    }
        SubShader
        {
            Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            LOD 200

            CGPROGRAM
            #pragma surface surf Standard fullforwardshadows alpha:premul vertex:vert

            #pragma target 3.0


            struct Input
            {
                float4 screenPos;
                float3 worldPos;
                float3 viewDir;

            };

            fixed4 _Color;
            fixed4 _FogColor;
            fixed4 _IntersectionColor;

            float _IntersectionThreshold;
            float _FogThreshold;
            float _FoamThreshold;

            sampler2D _NormalA;
            sampler2D _NormalB;
            float4 _NormalA_ST;
            float4 _NormalB_ST;
            float _NormalStrength;
            float4 _NormalPanningSpeeds;

            sampler2D _FoamTexture;
            float4 _FoamTexture_ST;
            float _FoamTextureSpeedX;
            float _FoamTextureSpeedY;
            float _FoamLinesSpeed;
            float _FoamIntensity;

            sampler2D _RenderTexture;
            half _Glossiness;
            float _FresnelPower;

            sampler2D _CameraDepthTexture;
            float3 _CamPosition;
            float _OrthographicCamSize;

            float4 _WaveA, _WaveB, _WaveC;

            // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
            // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
            // #pragma instancing_options assumeuniformscaling
            UNITY_INSTANCING_BUFFER_START(Props)
                // put more per-instance properties here
            UNITY_INSTANCING_BUFFER_END(Props)
                float3 gerstnerWave
                (float4 wave, float3 p, inout float3 tangent, inout float3 binormal)
            {
                float steepness = wave.z;
                float length = wave.w;
                float k = 2 * UNITY_PI / length;
                float c = sqrt(9.8 / k);
                float2 d = normalize(wave.xy);
                float f = k * (dot(d, p.xz) - c * _Time.y);
                float a = steepness / k;

                tangent += float3(
                    -d.x * d.x * (steepness * sin(f)),
                    d.x * (steepness * cos(f)),
                    -d.x * d.y * (steepness * sin(f))
                    );
                binormal += float3(
                    -d.x * d.y * (steepness * sin(f)),
                    d.y * (steepness * cos(f)),
                    -d.y * d.y * (steepness * sin(f))
                    );

                return float3(
                    d.x * (a * cos(f)),
                    a * sin(f),
                    d.y * (a * cos(f))
                    );
            }

            void vert(inout appdata_full vertexData)
            {
                float4 gridPoint = mul(unity_ObjectToWorld,vertexData.vertex);
                float3 tangent = float3(1, 0, 0);
                float3 binormal = float3(0, 0, 1);
                float3 p = gridPoint;
                p += gerstnerWave(_WaveA, gridPoint, tangent, binormal);
                p += gerstnerWave(_WaveB, gridPoint, tangent, binormal);
                p += gerstnerWave(_WaveC, gridPoint, tangent, binormal);
                gridPoint.xyz = p;

                float3 normal = normalize(cross(binormal, tangent));

                vertexData.vertex = mul(unity_WorldToObject, gridPoint);
                vertexData.normal = normal;

            }

            void surf(Input IN, inout SurfaceOutputStandard o)
            {
                float2 rtUV = IN.worldPos.xz - _CamPosition.xz;
                rtUV = rtUV / (_OrthographicCamSize * 2);
                rtUV += 0.5;
                fixed4 rt = tex2D(_RenderTexture, rtUV);

                float depth = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(IN.screenPos));
                depth = LinearEyeDepth(depth);

                float fogDiff = saturate((depth - IN.screenPos.w) / _FogThreshold);
                float intersectionDiff = saturate((depth - IN.screenPos.w) / _IntersectionThreshold);
                float foamDiff = saturate((depth - IN.screenPos.w) / _FoamThreshold);
                foamDiff *= (1.0 - rt.b);

                fixed4 c = lerp(lerp(_IntersectionColor, _Color, intersectionDiff), _FogColor, fogDiff);

                float foamTex = tex2D(_FoamTexture, IN.worldPos.xz * _FoamTexture_ST.xy + _Time.y * float2(_FoamTextureSpeedX, _FoamTextureSpeedY));
                float foam = step(foamDiff - (saturate(sin((foamDiff - _Time.y * _FoamLinesSpeed) * 8 * UNITY_PI)) * (1.0 - foamDiff)), foamTex);

                float fresnel = pow(1.0 - saturate(dot(o.Normal, normalize(IN.viewDir))), _FresnelPower);

                o.Albedo = c.rgb;
                float3 normalA = UnpackNormalWithScale(tex2D(_NormalA, IN.worldPos.xz * _NormalA_ST.xy + _Time.y * _NormalPanningSpeeds.xy + rt.rg), _NormalStrength);
                float3 normalB = UnpackNormalWithScale(tex2D(_NormalB, IN.worldPos.xz * _NormalB_ST.xy + _Time.y * _NormalPanningSpeeds.zw + rt.rg), _NormalStrength);
                o.Normal = normalA + normalB;
                o.Smoothness = _Glossiness;
                o.Alpha = lerp(lerp(c.a * fresnel, 1.0, foam), _FogColor.a, fogDiff);
                o.Emission = foam * _FoamIntensity;
            }
            ENDCG
        }
            FallBack "Diffuse"
}
