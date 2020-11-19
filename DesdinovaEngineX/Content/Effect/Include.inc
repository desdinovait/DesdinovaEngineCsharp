//-----------------------------------------------------------------------------
// Filename: Include.inc
// Author: Daniele "Duff" Ferla
// Date: 30 August 2008
// Description: Include files for almost all .fx files into engine
// Remarks: This variables are identically named into all files, but it's no shared
//          There's no effect pool (with 'shared' keyword)
//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------
// Global Structure
//-----------------------------------------------------------------------------

struct DirectionalLight
{
	float3	dir;
	float4	diffuse;
	float4	specular;
	bool	enabled;
};

struct PointLight
{
	float3	pos;
	float4	diffuse;
	float4	specular;
	float	radius;
	bool	enabled;
};

struct Fog
{
	float4	color;
	float	start;
	float	end;
	float	altitude;
	float	thinning;
	bool	enabled;
};


//-----------------------------------------------------------------------------
// Textures
//-----------------------------------------------------------------------------

texture colorMapTexture;
sampler colorMap = sampler_state
{
	Texture = <colorMapTexture>;
    MagFilter = Linear;
    MinFilter = Linear;
    MipFilter = Linear;
 	AddressU = Clamp;
	AddressV = Clamp;      
};

texture normalMapTexture;
sampler normalMap = sampler_state
{
	Texture = <normalMapTexture>;
    MagFilter = Linear;
    MinFilter = Linear;
    MipFilter = Linear;
 	AddressU = Clamp;
	AddressV = Clamp;      
};

texture heightMapTexture;
sampler heightMap = sampler_state
{
	Texture = <heightMapTexture>;
    MagFilter = Linear;
    MinFilter = Linear;
    MipFilter = Linear;
 	AddressU = Clamp;
	AddressV = Clamp;      
};

texture environmentMapTexture;
uniform const sampler environmentMap = sampler_state
{
	Texture = <environmentMapTexture>;
    MagFilter = Linear;
    MinFilter = Linear;
    MipFilter = Linear;
 	AddressU = Clamp;
	AddressV = Clamp;      
};


//-----------------------------------------------------------------------------
// Globals
//-----------------------------------------------------------------------------

uniform const float4x4 worldMatrix;
uniform const float4x4 viewMatrix;
uniform const float4x4 projectionMatrix;

uniform const float4x4 worldInverseTransposeMatrix;

uniform const float3 cameraPos;
uniform const float4 globalAmbient;

uniform const Fog fog;

uniform const float4	materialDiffuse;
uniform const float4	materialEmissive;
uniform const float4	materialSpecular;
uniform const float		materialShininess;
uniform const float		materialAlpha;
uniform const float		materialReflection;
uniform const bool		materialNormalMapEnabled;

//-----------------------------------------------------------------------------
// Globals
//-----------------------------------------------------------------------------

uniform const DirectionalLight dirLight[3];	//Max directional lights array
uniform const PointLight pointLight[4];		//Max dynamic point lights array (per-model)
uniform const bool useDynamicLights;		//Use dynamic lights in shader
