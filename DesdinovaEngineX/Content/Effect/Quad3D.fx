//-----------------------------------------------------------------------------
// Filename: Quad3D.fx
// Author: Daniele "Duff" Ferla
// Date: 30 August 2008
// Description: Effect file for Quad3D (aka Billboard)
// Remarks: None
//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------
// Include
//-----------------------------------------------------------------------------

#include "Include.inc"


//-----------------------------------------------------------------------------
// Globals
//-----------------------------------------------------------------------------

uniform const float4	billboardColor;
uniform const float		billboardAlpha;


//-----------------------------------------------------------------------------
// Input/Output Structures
//-----------------------------------------------------------------------------

struct A2V_SimpleModel
{ 
	float4 position : POSITION;
	float2 texCoord : TEXCOORD0;
};

struct V2P_SimpleModel
{
	float4 position  : POSITION;
	float4 worldPos : TEXCOORD0;
	float2 texCoord : TEXCOORD1;
};


//-----------------------------------------------------------------------------
// Vertex Shaders
//-----------------------------------------------------------------------------

void VS_Quad3D( in A2V_SimpleModel IN, out V2P_SimpleModel OUT) 
{
	float4x4 preWorldViewProj = mul(mul(worldMatrix,viewMatrix),projectionMatrix);	
	OUT.position = mul(IN.position, preWorldViewProj);
	OUT.texCoord = IN.texCoord;
	OUT.worldPos.xyz = mul(IN.position, worldMatrix).xyz;
	float d = length(OUT.worldPos - cameraPos);    
    OUT.worldPos.w = fog.enabled * (saturate((d - fog.start) / (fog.end - fog.start) / clamp(OUT.worldPos.y / fog.altitude + 1, 1, fog.thinning)));

}


//-----------------------------------------------------------------------------
// Pixel Shaders
//-----------------------------------------------------------------------------

float4 PS_Quad3D( in V2P_SimpleModel IN) : COLOR
{
	//Texture
	float4 color = tex2D(colorMap, IN.texCoord) * billboardColor;
				
	//Fog
	//if (fog.enabled)
	//{
	//	color = lerp(color, fog.color, IN.worldPos.w);
	//}
		
	//Alpha
	color.a = billboardAlpha;
	
	return color;
}


//-----------------------------------------------------------------------------
// Techniques
//-----------------------------------------------------------------------------

technique Quad3D
{
	pass Pass1
	{
		VertexShader = compile vs_2_0 VS_Quad3D();
		PixelShader  = compile ps_2_0 PS_Quad3D();
	}
}
