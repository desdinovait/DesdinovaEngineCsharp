//////////////////////////////////////////////////////////////
// File: BlackAndWhite.fx
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

float4 BlackAndWhite_PS(float2 iTexCoord : TEXCOORD0) : COLOR
{	
	float3 LuminanceConv = { 0.2125f, 0.7154f, 0.0721f };
	return dot( (float3)tex2D( PostProcessSampler, iTexCoord ), LuminanceConv );
}


//-----------------------------------------------------------------------------
// List of PostProcessing Technique
//-----------------------------------------------------------------------------
technique TechniqueBlackAndWhite
{
	Pass P0
	{	
		VertexShader = compile vs_2_0 PassThrough_VS();
		PixelShader = compile ps_2_0 BlackAndWhite_PS();
	}
}