//////////////////////////////////////////////////////////////
// File: Blur.fx
// Author: Duff
// Date Created: 18/03/2008
//////////////////////////////////////////////////////////////

//-----------------------------------------------------------------------------
// Effect File Variables
//-----------------------------------------------------------------------------
texture offscreenTexture;
float2 blurTexelsize;

sampler2D PostProcessSampler = sampler_state
{
	Texture   = <offscreenTexture>;
	MinFilter = LINEAR;
	MagFilter = LINEAR;
	MipFilter = LINEAR;
	AddressU  = Wrap;
	AddressV  = Wrap;
};



//-----------------------------------------------------------------------------
// Pixel and Vertex Shaders
//-----------------------------------------------------------------------------

//Pass Through Vertex Shader
void PassThrough_VS(in float4 iPosition : POSITION, in float2 iTexCoord : TEXCOORD0, out float4 oPosition   : POSITION, out float2 oTexCoord   : TEXCOORD0)
{	
	oPosition=iPosition;
	oTexCoord=iTexCoord;
}

//Blur
float4 Blur_PS(float2 iTexCoord : TEXCOORD0) : COLOR
{	
	float2 usedTexelBlur[8] = {
		-1, -1,
		 0, -1,
		 1, -1,
		-1,  0,
		 1,  0,
		-1,  1,
		 0,  1,
	     1,  1,	
	};

	float4 cAvgColor=tex2D(PostProcessSampler, iTexCoord);	
	
	cAvgColor+=tex2D(PostProcessSampler, iTexCoord + blurTexelsize*usedTexelBlur[0]);
	cAvgColor+=tex2D(PostProcessSampler, iTexCoord + blurTexelsize*usedTexelBlur[1]);
	cAvgColor+=tex2D(PostProcessSampler, iTexCoord + blurTexelsize*usedTexelBlur[2]);
	cAvgColor+=tex2D(PostProcessSampler, iTexCoord + blurTexelsize*usedTexelBlur[3]);
	cAvgColor+=tex2D(PostProcessSampler, iTexCoord + blurTexelsize*usedTexelBlur[4]);
	cAvgColor+=tex2D(PostProcessSampler, iTexCoord + blurTexelsize*usedTexelBlur[5]);
	cAvgColor+=tex2D(PostProcessSampler, iTexCoord + blurTexelsize*usedTexelBlur[6]);
	cAvgColor+=tex2D(PostProcessSampler, iTexCoord + blurTexelsize*usedTexelBlur[7]);
	cAvgColor/=9;
		
	return cAvgColor;
}



//-----------------------------------------------------------------------------
// List of PostProcessing Technique
//-----------------------------------------------------------------------------
technique TechniqueBlur
{
	Pass P0
	{	
		VertexShader = compile vs_2_0 PassThrough_VS();
		PixelShader = compile ps_2_0 Blur_PS();
	}
}