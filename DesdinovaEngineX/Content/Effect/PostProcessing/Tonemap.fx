//////////////////////////////////////////////////////////////
// File: Tonemap.fx
// Author: Duff
// Date Created: 18/03/2008
//////////////////////////////////////////////////////////////

//-----------------------------------------------------------------------------
// Effect File Variables
//-----------------------------------------------------------------------------
texture offscreenTexture;
float tonemapLuminance = 0.09f;

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

//Tone Mapping
float4 Tonemap_PS( float2 Tex : TEXCOORD ) : COLOR0
{
	static const float fMiddleGray = 0.18f;
	static const float fWhiteCutoff = 0.8f;
	float4 Color;
	Color = tex2D( PostProcessSampler, Tex ) * fMiddleGray / ( tonemapLuminance + 0.001f );
	Color *= ( 1.0f + ( Color / ( fWhiteCutoff * fWhiteCutoff ) ) );
	Color /= ( 1.0f + Color );
	return Color;
}

//-----------------------------------------------------------------------------
// List of PostProcessing Technique
//-----------------------------------------------------------------------------
technique TechniqueTonemap
{
	Pass P0
	{	
		VertexShader = compile vs_2_0 PassThrough_VS();
		PixelShader = compile ps_2_0 Tonemap_PS();
	}
}