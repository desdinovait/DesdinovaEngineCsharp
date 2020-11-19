//////////////////////////////////////////////////////////////
// File: Bloom.fx
// Author: Duff
// Date Created: 18/03/2008
//////////////////////////////////////////////////////////////

//-----------------------------------------------------------------------------
// Effect File Variables
//-----------------------------------------------------------------------------
texture offscreenTexture;
float bloomScale;

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

//Bloom
float4 Bloom_PS( float2 Tex : TEXCOORD0 ) : COLOR0
{
	static const int g_cKernelSize = 13;
	float4 Color = 0;
	float2 PixelKernel[g_cKernelSize] =
	{
	    { -6, 0 },
	    { -5, 0 },
	    { -4, 0 },
	    { -3, 0 },
	    { -2, 0 },
	    { -1, 0 },
	    {  0, 0 },
	    {  1, 0 },
	    {  2, 0 },
	    {  3, 0 },
	    {  4, 0 },
	    {  5, 0 },
	    {  6, 0 },
	};
	static const float BlurWeights[g_cKernelSize] = 
	{
	    0.002216,
	    0.008764,
	    0.026995,
	    0.064759,
	    0.120985,
	    0.176033,
	    0.199471,
	    0.176033,
	    0.120985,
	    0.064759,
	    0.026995,
	    0.008764,
	    0.002216,
	};

	for (int i = 0; i < g_cKernelSize; i++)
	{    
		Color += tex2D( PostProcessSampler, Tex ) * BlurWeights[i];
	}
	return Color * bloomScale;
}




//-----------------------------------------------------------------------------
// List of PostProcessing Technique
//-----------------------------------------------------------------------------
technique TechniqueBloom
{
	Pass P0
	{	
		VertexShader = compile vs_2_0 PassThrough_VS();
		PixelShader = compile ps_2_0 Bloom_PS();
	}
}