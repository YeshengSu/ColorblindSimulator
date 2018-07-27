#ifndef COLORBLINDNESSLIB_CG_INCLUDE  
#define COLORBLINDNESSLIB_CG_INCLUDE   

float4 Gamma2Linear(float4 inputRGB)
{
	float4 RGBc = float4(0.0f, 0.0f, 0.0f, 1.0f);
	RGBc = pow(inputRGB, 2.2f);
	RGBc = saturate(RGBc);
	return RGBc;
}

float4 Linear2Gamma(float4 inputRGB)
{
	float4 RGB = float4(0.0f, 0.0f, 0.0f, 1.0f);
	RGB = pow(inputRGB, 1 / 2.2f);
	RGB = saturate(RGB);
	return RGB;
}

float3 RGB2LMS(float3 inputRGB)
{
	float3 LMS = float3(0.0f, 0.0f, 0.0f);
	LMS.x = (0.31399022f * inputRGB.r) + (0.63951294f * inputRGB.g) + (0.04649755f * inputRGB.b);
	LMS.y = (0.15537241f * inputRGB.r) + (0.75789446f * inputRGB.g) + (0.08670142f * inputRGB.b);
	LMS.z = (0.01775239f * inputRGB.r) + (0.10944209f * inputRGB.g) + (0.87256922f * inputRGB.b);
	return LMS;
}

float3 LMS2RGB(float3 inputLMS)
{
	float3 RGB = float3(0.0f, 0.0f, 0.0f);
	RGB.r = (5.47221206f * inputLMS.x) + (-4.6419601f  * inputLMS.y) + (0.16963708f * inputLMS.z);
	RGB.g = (-1.1252419f * inputLMS.x) + (2.29317094f  * inputLMS.y) + (-0.1678952f * inputLMS.z);
	RGB.b = (0.02980165f * inputLMS.x) + (-0.19318073f * inputLMS.y) + (1.16364789f * inputLMS.z);
	return RGB;
}


float3 Normal(float3 inputLMS)
{
	float3 lms = float3(0.0f, 0.0f, 0.0f);
	lms.x = 1.0f * inputLMS.x + 0.0f     * inputLMS.y + 0.0f      * inputLMS.z;
	lms.y = 0.0f * inputLMS.x + 1.0f     * inputLMS.y + 0.0f      * inputLMS.z;
	lms.z = 0.0f * inputLMS.x + 0.0f     * inputLMS.y + 1.0f      * inputLMS.z;
	return lms;
}

float3 Protanomaly(float3 inputLMS)
{
	float3 lms = float3(0.0f, 0.0f, 0.0f);
	lms.x = 0.7f * inputLMS.x + 1.05118294f * inputLMS.y * 0.3f + -0.05116099f * inputLMS.z * 0.3f;
	lms.y = 0.0f * inputLMS.x + 1.0f        * inputLMS.y + 0.0f         * inputLMS.z;
	lms.z = 0.0f * inputLMS.x + 0.0f        * inputLMS.y + 1.0f         * inputLMS.z;
	return lms;
}

float3 Deuteranomaly(float3 inputLMS)
{
	float3 lms = float3(0.0f, 0.0f, 0.0f);
	lms.x = 1.0f       * inputLMS.x + 0.0f * inputLMS.y + 0.0f        * inputLMS.z;
	lms.y = 0.9513092f * inputLMS.x * 0.3f + 0.7f * inputLMS.y + 0.04866992f * inputLMS.z * 0.3f;
	lms.z = 0.0f       * inputLMS.x + 0.0f * inputLMS.y + 1.0f        * inputLMS.z;
	return lms;
}

float3 Tritanomaly(float3 inputLMS)
{
	float3 lms = float3(0.0f, 0.0f, 0.0f);
	lms.x = 1.0f         * inputLMS.x + 0.0f        * inputLMS.y + 0.0f * inputLMS.z;
	lms.y = 0.0f         * inputLMS.x + 1.0f        * inputLMS.y + 0.0f * inputLMS.z;
	lms.z = -0.86744736f * inputLMS.x * 0.3f + 1.86727089f * inputLMS.y * 0.3f + 0.7f * inputLMS.z;
	return lms;
}

float3 Protanope(float3 inputLMS)
{
	float3 lms = float3(0.0f, 0.0f, 0.0f);
	lms.x = 0.0f * inputLMS.x + 1.05118294f * inputLMS.y + -0.05116099f * inputLMS.z;
	lms.y = 0.0f * inputLMS.x + 1.0f        * inputLMS.y + 0.0f         * inputLMS.z;
	lms.z = 0.0f * inputLMS.x + 0.0f        * inputLMS.y + 1.0f         * inputLMS.z;
	return lms;
}

float3 Deuteranope(float3 inputLMS)
{
	float3 lms = float3(0.0f, 0.0f, 0.0f);
	lms.x = 1.0f       * inputLMS.x + 0.0f * inputLMS.y + 0.0f        * inputLMS.z;
	lms.y = 0.9513092f * inputLMS.x + 0.0f * inputLMS.y + 0.04866992f * inputLMS.z;
	lms.z = 0.0f       * inputLMS.x + 0.0f * inputLMS.y + 1.0f        * inputLMS.z;
	return lms;
}

float3 Tritanope(float3 inputLMS)
{
	float3 lms = float3(0.0f, 0.0f, 0.0f);
	lms.x = 1.0f         * inputLMS.x + 0.0f        * inputLMS.y + 0.0f * inputLMS.z;
	lms.y = 0.0f         * inputLMS.x + 1.0f        * inputLMS.y + 0.0f * inputLMS.z;
	lms.z = -0.86744736f * inputLMS.x + 1.86727089f * inputLMS.y + 0.0f * inputLMS.z;
	return lms;
}

float3 RedConeMonochromats(float3 inputLMS)
{
	float3 lms = float3(0.0f, 0.0f, 0.0f);
	lms.x = 1.0f * inputLMS.x + 0.0f * inputLMS.y + 0.0f * inputLMS.z;
	lms.y = 1.0f * inputLMS.x + 0.0f * inputLMS.y + 0.0f * inputLMS.z;
	lms.z = 1.0f * inputLMS.x + 0.0f * inputLMS.y + 0.0f * inputLMS.z;
	return lms;
}

float3 GreenConeMonochromats(float3 inputLMS)
{
	float3 lms = float3(0.0f, 0.0f, 0.0f);
	lms.x = 0.0f * inputLMS.x + 1.0f * inputLMS.y + 0.0f * inputLMS.z;
	lms.y = 0.0f * inputLMS.x + 1.0f * inputLMS.y + 0.0f * inputLMS.z;
	lms.z = 0.0f * inputLMS.x + 1.0f * inputLMS.y + 0.0f * inputLMS.z;
	return lms;
}

float3 BlueConeMonochromats(float3 inputLMS)
{
	float3 lms = float3(0.0f, 0.0f, 0.0f);
	lms.x = 0.0f * inputLMS.x + 0.0f * inputLMS.y + 1.0f * inputLMS.z;
	lms.y = 0.0f * inputLMS.x + 0.0f * inputLMS.y + 1.0f * inputLMS.z;
	lms.z = 0.0f * inputLMS.x + 0.0f * inputLMS.y + 1.0f * inputLMS.z;
	return lms;
}

float3 RodMonochromats(float3 inputLMS)
{
	float3 lms = float3(0.0f, 0.0f, 0.0f);
	lms.x = 0.0f * inputLMS.x + 1.0f * inputLMS.y + 0.0f * inputLMS.z;
	lms.y = 0.0f * inputLMS.x + 1.0f * inputLMS.y + 0.0f * inputLMS.z;
	lms.z = 0.0f * inputLMS.x + 1.0f * inputLMS.y + 0.0f * inputLMS.z;
	return lms;
}

float4 ProtanopeDaltonization(float4 inputErrorRGB)
{
	float4 RGBc = float4(0.0f, 0.0f, 0.0f, 1.0f);
	RGBc.x = -1.0f * inputErrorRGB.x + 0.0f * inputErrorRGB.y + 0.0f * inputErrorRGB.z;
	RGBc.y = 0.5f * inputErrorRGB.x + 1.0f * inputErrorRGB.y + 0.0f * inputErrorRGB.z;
	RGBc.z = 1.5f * inputErrorRGB.x + 0.0f * inputErrorRGB.y + 1.0f * inputErrorRGB.z;
	return RGBc;
}

float4 DeuteranopeDaltonization(float4 inputErrorRGB)
{
	float4 RGBc = float4(0.0f, 0.0f, 0.0f, 1.0f);
	RGBc.x = 1.0f * inputErrorRGB.x + 0.0f * inputErrorRGB.y + 0.0f * inputErrorRGB.z;
	RGBc.y = 0.0f * inputErrorRGB.x + -1.0f * inputErrorRGB.y + 0.0f * inputErrorRGB.z;
	RGBc.z = 2.0f * inputErrorRGB.x + 0.0f * inputErrorRGB.y + 1.0f * inputErrorRGB.z;
	return RGBc;
}

float4 TritanopeDaltonization(float4 inputErrorRGB)
{
	float4 RGBc = float4(0.0f, 0.0f, 0.0f, 1.0f);
	RGBc.x = 1.0f * inputErrorRGB.x + 0.0f * inputErrorRGB.y + 0.0f * inputErrorRGB.z;
	RGBc.y = 0.0f * inputErrorRGB.x + 1.0f * inputErrorRGB.y + 2.5f * inputErrorRGB.z;
	RGBc.z = 0.0f * inputErrorRGB.x + 0.0f * inputErrorRGB.y + -1.5f * inputErrorRGB.z;
	return RGBc;
}


#endif
