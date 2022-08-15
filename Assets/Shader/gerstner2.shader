Shader "Custom/gerstner2"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color1("Color1", Color) = (256,256,256,1)
		_Color2("Color2", Color) = (256,256,256,1)
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

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
					float3 worldPos : TEXCOORD1;
				};

				sampler2D _MainTex;
				float4 _Color1;
				float4 _Color2;
				float4 _MainTex_ST;

				float3 Gerstner(float a, float b, float c , float k)
				{
					float t = _Time;
					float e = 2.71828;
					float x = a + ((pow(e,k * b) / k) * sin(k * (a + c * t)));
					float z = b - ((pow(e,k * b) / k) * cos(k * (a + c * t)));
					return float3(x - a,0,z);
				}

				float3 Rotate(float3 pos, float degrees)
				{
					float sinX = sin(degrees * 1 / 180 * 3.14159);
					float cosX = cos(degrees * 1 / 180 * 3.14159);
					float2x2 rotationMatrix = float2x2(cosX, -sinX, sinX, cosX);
					pos.xy = mul(pos.xy, rotationMatrix);
					return pos;
				}

				v2f vert(appdata v)
				{
					v2f o;
					float3 offset = Gerstner(Rotate(v.vertex, 20).x,-.07,3,12)
							+ Gerstner(Rotate(v.vertex, 12).x,-.01,0.6,42) * 0.5
							+ Gerstner(Rotate(v.vertex, 24).x,-0.01,0.2,60) * 0.3;
					v.vertex.xyz += offset;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					o.worldPos = mul(unity_ObjectToWorld, v.vertex);
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					UNITY_APPLY_FOG(i.fogCoord, col);
					float top = 5;
					float bottom = -5;
					float y = i.worldPos.y * 0.7;
					fixed4 col = lerp(_Color1, _Color2, y);
					return col;
				}
				ENDCG
			}
		}
}
