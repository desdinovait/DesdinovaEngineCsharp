//////////////////////////////////////////////////////////////
// File: EdgeDetection.fx
// Author: Duff
// Date Created: 18/03/2008
//////////////////////////////////////////////////////////////

//-----------------------------------------------------------------------------
// Effect File Variables
//-----------------------------------------------------------------------------
texture offscreenTexture;
float2 edgeTexelsize;

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

//Edge Detection
float4 EdgeDetection_PS(float2 iTexCoord : TEXCOORD0) : COLOR
{	
	float2 usedTexelED[8] = 
	{		
		-1, -1,
		0, -1,
		1, -1,
		-1,  0,
		1,  0,
		-1,  1,
		0,  1,
		1,  1,	
	};	
	float4 cAvgColor=9*tex2D(PostProcessSampler, iTexCoord);	
	
	for(int t=0; t<8; t++)
	{
		cAvgColor-=tex2D(PostProcessSampler, iTexCoord + edgeTexelsize*usedTexelED[t]);		
	}
	cAvgColor-=tex2D(PostProcessSampler, iTexCoord);
	cAvgColor=1-cAvgColor;
	return cAvgColor;
}


//-----------------------------------------------------------------------------
// List of PostProcessing Technique
//-----------------------------------------------------------------------------
technique TechniqueEdgeDetection
{
	Pass P0
	{	
		VertexShader = compile vs_2_0 PassThrough_VS();
		PixelShader = compile ps_2_0 EdgeDetection_PS();
	}
}