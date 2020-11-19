//////////////////////////////////////////////////////////////
// File: Sepia.fx
// Author: Duff
// Date Created: 18/03/2008
//////////////////////////////////////////////////////////////


//-----------------------------------------------------------------------------
// Effect File Variables
//-----------------------------------------------------------------------------
texture offscreenTexture;

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
void PassThrough_VS(in float4 iPosition : POSITION, in float2 iTexCoord : TEXCOORD0, out float4 oPosition   : POSITION, out float2 oTexCoord   : TEXCOORD0)
{	
	oPosition=iPosition;
	oTexCoord=iTexCoord;
}

//Sepia
float4 Sepia_PS(float2 iTexCoord : TEXCOORD0) : COLOR
{	
	float4 vWeightsBW=float4(0.3333,0.59,0.11,0);
	float4 vWeightsSepia=float4(0.9,0.7,0.3,1);
	float4 cColor=tex2D(PostProcessSampler,iTexCoord);
	//Apply sepia tone
	return dot(cColor,vWeightsBW) * vWeightsSepia;
}




//-----------------------------------------------------------------------------
// List of PostProcessing Technique
//-----------------------------------------------------------------------------
technique TechniqueSepia
{
	Pass P0
	{	
		VertexShader = compile vs_2_0 PassThrough_VS();
		PixelShader = compile ps_2_0 Sepia_PS();
	}
}