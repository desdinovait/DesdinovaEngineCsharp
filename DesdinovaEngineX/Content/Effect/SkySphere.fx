//-----------------------------------------------------------------------------
// Filename: SkySphere.fx
// Author: Daniele "Duff" Ferla
// Date: 30 August 2008
// Description: Effect file for skysphere
// Remarks: None
//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------
// Include
//-----------------------------------------------------------------------------

#include "Include.inc"


//-----------------------------------------------------------------------------
// Globals
//-----------------------------------------------------------------------------

uniform const float4 color;


//-----------------------------------------------------------------------------
// Vertex Shaders
//-----------------------------------------------------------------------------

void VS_SkySphere( float3 pos : POSITION0, out float4 SkyPos : POSITION0, out float3 SkyCoord : TEXCOORD0 )
{
    // Calculate rotation. Using a float3 result, so translation is ignored
    float3 rotatedPosition = mul(pos, viewMatrix);           
    // Calculate projection, moving all vertices to the far clip plane 
    // (w and z both 1.0)
    SkyPos = mul(float4(rotatedPosition, 1), projectionMatrix).xyww;    

    SkyCoord = pos;
};


//-----------------------------------------------------------------------------
// Pixel Shaders
//-----------------------------------------------------------------------------

float4 PS_SkySphere( float3 SkyCoord : TEXCOORD0 ) : COLOR
{
    return texCUBE(colorMap, SkyCoord) * color;
};


//-----------------------------------------------------------------------------
// Techniques 
//-----------------------------------------------------------------------------

technique Skybox
{
    pass P0
    {
        vertexShader = compile vs_2_0 VS_SkySphere();
        pixelShader = compile ps_2_0 PS_SkySphere();
    }
}
