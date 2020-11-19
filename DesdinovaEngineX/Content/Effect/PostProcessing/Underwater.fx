//////////////////////////////////////////////////////////////
// File: Underwater.fx
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

//Pass Through Vertex Shader
void PassThrough_VS(in float4 iPosition : POSITION, in float2 iTexCoord : TEXCOORD0, out float4 oPosition   : POSITION, out float2 oTexCoord   : TEXCOORD0)
{	
	oPosition=iPosition;
	oTexCoord=iTexCoord;
}

//Underwater
float4 Underwater_PS(float2 iTexCoord : TEXCOORD) : COLOR0
{
    //Vertical pertubation
    iTexCoord.y = iTexCoord.y + (sin(iTexCoord.x*50.0f)*0.01f);
    iTexCoord.x = iTexCoord.x + (cos(0.5f)*0.01f);
    
    //Orizontal perturbation
    iTexCoord.x = iTexCoord.x + (sin(iTexCoord.y*50.0f)*0.01f);
    iTexCoord.y = iTexCoord.y + (cos(0.5f)*0.01f);

    return tex2D(PostProcessSampler, iTexCoord);
}


//-----------------------------------------------------------------------------
// List of PostProcessing Technique
//-----------------------------------------------------------------------------
technique TechniqueUnderwater
{
	Pass P0
	{	
		VertexShader = compile vs_2_0 PassThrough_VS();
		PixelShader = compile ps_2_0 Underwater_PS();
	}
}