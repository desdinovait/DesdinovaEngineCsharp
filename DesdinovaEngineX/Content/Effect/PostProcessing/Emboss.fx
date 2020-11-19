//////////////////////////////////////////////////////////////
// File: Emboss.fx
// Author: Duff
// Date Created: 18/03/2008
//////////////////////////////////////////////////////////////

//-----------------------------------------------------------------------------
// Effect File Variables
//-----------------------------------------------------------------------------
texture offscreenTexture;
float2 embossTexelsize;

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

//Emboss
float4 Emboss_PS(float2 iTexCoord : TEXCOORD0) : COLOR
{	
	float4 cAvgColor=tex2D(PostProcessSampler, iTexCoord);	
	cAvgColor-=tex2D(PostProcessSampler, iTexCoord + embossTexelsize*float2(-1,-1));		
	cAvgColor-=0.5*tex2D(PostProcessSampler, iTexCoord + embossTexelsize*float2(0,-1));		
	cAvgColor-=0.5*tex2D(PostProcessSampler, iTexCoord + embossTexelsize*float2(-1,0));		
	cAvgColor+=0.5*tex2D(PostProcessSampler, iTexCoord + embossTexelsize*float2(1,0));		
	cAvgColor+=0.5*tex2D(PostProcessSampler, iTexCoord + embossTexelsize*float2(0,1));		
	cAvgColor+=tex2D(PostProcessSampler, iTexCoord + embossTexelsize*float2(1,1));	
				
	return cAvgColor;
}


//-----------------------------------------------------------------------------
// List of PostProcessing Technique
//-----------------------------------------------------------------------------
technique TechniqueEmboss
{
	Pass P0
	{	
		VertexShader = compile vs_2_0 PassThrough_VS();
		PixelShader = compile ps_2_0 Emboss_PS();
	}
}