//////////////////////////////////////////////////////////////
// File: Pixelate.fx
// Author: Duff
// Date Created: 28/03/2008
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


half NumberOfPixels = 40.0;
half EdgeWidth = 0.15;
half3 EdgeColor = {0.7f, 0.7f, 0.7f};




//-----------------------------------------------------------------------------
// Pixel and Vertex Shaders
//-----------------------------------------------------------------------------
void PassThrough_VS(in float4 iPosition : POSITION, in float2 iTexCoord : TEXCOORD0, out float4 oPosition   : POSITION, out float2 oTexCoord   : TEXCOORD0)
{	
	oPosition=iPosition;
	oTexCoord=iTexCoord;
}

//Inverse Color
float4 Pixelate_PS(float2 iTexCoord : TEXCOORD0) : COLOR
{	
    half size = 1.0/NumberOfPixels;
    half2 Pbase = iTexCoord - fmod(iTexCoord,size.xx);
    half2 PCenter = Pbase + (size/2.0).xx;
    half2 st = (iTexCoord - Pbase)/size;
    half4 c1 = (half4)0;
    half4 c2 = (half4)0;
    half4 invOff = half4((1-EdgeColor),1);
    if (st.x > st.y) { c1 = invOff; }
    half threshholdB =  1.0 - EdgeWidth;
    if (st.x > threshholdB) { c2 = c1; }
    if (st.y > threshholdB) { c2 = c1; }
    half4 cBottom = c2;
    c1 = (half4)0;
    c2 = (half4)0;
    if (st.x > st.y) { c1 = invOff; }
    if (st.x < EdgeWidth) { c2 = c1; }
    if (st.y < EdgeWidth) { c2 = c1; }
    half4 cTop = c2;
    float4 tileColor = tex2D(PostProcessSampler,PCenter);
    float4 result = tileColor + cTop - cBottom;
    
    return result;
}


//-----------------------------------------------------------------------------
// List of PostProcessing Technique
//-----------------------------------------------------------------------------
technique TechniquePixelate
{
	Pass P0
	{	
		VertexShader = compile vs_2_0 PassThrough_VS();
		PixelShader = compile ps_2_0 Pixelate_PS();
	}
}