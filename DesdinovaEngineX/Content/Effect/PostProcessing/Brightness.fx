//////////////////////////////////////////////////////////////
// File: Brightness.fx
// Author: Duff
// Date Created: 18/03/2008
//////////////////////////////////////////////////////////////

//-----------------------------------------------------------------------------
// Effect File Variables
//-----------------------------------------------------------------------------
texture offscreenTexture;
float brightnessValue;

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

//Brightness
float4 Brightness_PS(float2 iTexCoord : TEXCOORD) : COLOR0
{
    float4 Color = tex2D(PostProcessSampler, iTexCoord); 
    return Color * brightnessValue;
}


//-----------------------------------------------------------------------------
// List of PostProcessing Technique
//-----------------------------------------------------------------------------
technique TechniqueBrightness
{
	Pass P0
	{	
		VertexShader = compile vs_2_0 PassThrough_VS();
		PixelShader = compile ps_2_0 Brightness_PS();
	}
}