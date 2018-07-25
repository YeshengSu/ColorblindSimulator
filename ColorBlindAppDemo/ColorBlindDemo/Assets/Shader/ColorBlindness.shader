Shader "Unlit/ColorBlindness"
{
	/*
	COLORBLIND MODE
	0 Normal
	1 Protanomaly
	2 Deuteranomaly
	3 Tritanomaly
	4 Protanope 
	5 Deuteranope 
	6 Tritanope  
	7 Red-Cone Monochromats
	8 Green-Cone Monochromats
	9 Blue-Cone Monochromats
	10 Rod Monochromats
	*/

	/*
	DALTONIZATION MODE
	0 Normal
	1 Protanope
	2 Deuteranope
	3 Tritanope
	*/

	Properties
	{
		[PerRendererData]_MainTex ("Sprite Texture", 2D) = "white" {}
		_COLORBLIND_MODE("COLORBLIND MODE", int) = 0
		_Correction("Correction", int) = 0
		_DALTONIZATION_MODE("DALTONIZATION MODE", int) = 0
	}
	SubShader
	{
		Tags{ 
			"QUEUE" = "Transparent"
			"IGNOREPROJECTOR" = "true"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "true"
		}
		Cull Off
		Lighting Off
		ZWrite Off

		Pass
		{
			CGPROGRAM
			// make fog work
			
			#include "UnityCG.cginc"
			#include "ColorBlindnessLib.cginc"

			#pragma vertex vert
			#pragma fragment frag

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

			sampler2D _MainTex;
			float4 _MainTex_ST;
			bool _Correction;
			int _COLORBLIND_MODE;
			int _DALTONIZATION_MODE;

			float4 CulculateColorBlind(float4 input)
			{
				float4 RGBc = Gamma2Linear(input);

				// RGB to LMS matrix conversion
				float3 LMS = RGB2LMS(RGBc);

				// reducing the normal colour domain to the certain colour domain
				float3 lms = float3(0.0f, 0.0f, 0.0f);
				switch (_COLORBLIND_MODE)
				{
				case 0 :
					lms = Normal(LMS);
					break;
				case 1 :
					lms = Protanomaly(LMS);
					break;
				case 2 :
					lms = Deuteranomaly(LMS);
					break;
				case 3 :
					lms = Tritanomaly(LMS);
					break;
				case 4 : 
					lms = Protanope(LMS);
					break;
				case 5 :
					lms = Deuteranope(LMS);
					break;
				case 6:
					lms = Tritanope(LMS);
					break;
				case 7 :
					lms = RedConeMonochromats(LMS);
					break;
				case 8 :
					lms = GreenConeMonochromats(LMS);
					break;
				case 9 :
					lms = BlueConeMonochromats(LMS);
					break;
				case 10:
					lms = RodMonochromats(LMS);
					break;
				}

				// LMS to RGB matrix conversion
				RGBc = float4(LMS2RGB(lms),1.0f);
				
				RGBc = saturate(RGBc);
				float4 RGB = Linear2Gamma(RGBc);
				
				return RGB;
			}

			float4 CulculateDaltonization(float4 input)
			{
				float4 RGBc = Gamma2Linear(input);

				// RGB to LMS matrix conversion
				float3 LMS = RGB2LMS(RGBc);

				// reducing the normal colour domain to the certain colour domain
				float3 lms = float3(0.0f, 0.0f, 0.0f);

				//calculate color blind
				switch (_DALTONIZATION_MODE)
				{
				case 0:
					lms = Normal(LMS);
					break;
				case 1:
					lms = Protanomaly(LMS);
					break;
				case 2:
					lms = Deuteranomaly(LMS);
					break;
				case 3:
					lms = Tritanomaly(LMS);
					break;
				}

				// LMS to RGB matrix conversion
				float4 colorBlindRGBc = float4(LMS2RGB(lms), 1.0f);

				//daltonization
				float4 errorRGBc = RGBc - colorBlindRGBc;

				//perform daltonization
				float4 modifiedRGBc = float4(0.0f, 0.0f, 0.0f, 1.0f);
				switch (_DALTONIZATION_MODE)
				{
				case 0:
					modifiedRGBc = float4(0.0f, 0.0f, 0.0f, 1.0f);
					break;
				case 1:
					modifiedRGBc = ProtanopeDaltonization(errorRGBc);
					break;
				case 2:
					modifiedRGBc = DeuteranopeDaltonization(errorRGBc);
					break;
				case 3:
					modifiedRGBc = TritanopeDaltonization(errorRGBc);
					break;
				}

				//daltonization
				RGBc = RGBc + modifiedRGBc;

				RGBc = saturate(RGBc);

				float4 RGB = Linear2Gamma(RGBc);

				return RGB;
			}


			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			float4 frag (v2f i) : SV_Target
			{
				// sample the texture
				float4 color = tex2D(_MainTex, i.uv);

				if (_Correction == 1)
					color = CulculateDaltonization(color);
				else
					color = CulculateColorBlind(color);
				
				return color;
			}



			ENDCG
		}
	}
}
