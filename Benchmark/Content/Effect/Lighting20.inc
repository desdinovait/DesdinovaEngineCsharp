//-----------------------------------------------------------------------------
// Filename: Lighting20.fx
// Author: Daniele "Duff" Ferla
// Date: 30 August 2008
// Description: Lighting effect file for 2.0 shader cards
// Remarks: None
//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------
// Structs
//-----------------------------------------------------------------------------

struct VS_INPUT20
{
	float3 position : POSITION;
	float2 texCoord : TEXCOORD0;
	float3 normal : NORMAL;
};

struct VS_OUTPUT20
{
	float4 position : POSITION;
	float4 worldPos : TEXCOORD0;		//xyz:position, w:fog
	float2 texCoord : TEXCOORD1;
	float3 viewDir : TEXCOORD2;
	float3 normal : TEXCOORD3;
};



//-----------------------------------------------------------------------------
// Vertex Shaders
//-----------------------------------------------------------------------------

VS_OUTPUT20 VS_Lighting20(VS_INPUT20 IN)
{
	VS_OUTPUT20 OUT;
	float4x4 preWorldViewProj = mul(mul(worldMatrix,viewMatrix),projectionMatrix);	
	OUT.position = mul(float4(IN.position, 1.0f), preWorldViewProj);
	OUT.worldPos.xyz = mul(float4(IN.position, 1.0f), worldMatrix).xyz;
	OUT.texCoord = IN.texCoord;
	OUT.viewDir = normalize(cameraPos - OUT.worldPos);
	OUT.normal = normalize(mul(IN.normal, (float3x3)worldInverseTransposeMatrix));
	float d = length(OUT.worldPos - cameraPos);    
    OUT.worldPos.w = fog.enabled * (saturate((d - fog.start) / (fog.end - fog.start) / clamp(OUT.worldPos.y / fog.altitude + 1, 1, fog.thinning)));
		
	return OUT;
}


//-----------------------------------------------------------------------------
// Pixel Shaders
//-----------------------------------------------------------------------------

float4 PS_Lighting20(VS_OUTPUT20 IN) : COLOR
{  
	float3 diffuseColor = globalAmbient;
	float3 specularColor = 0;
	
    //Directional lights
	for (int i=0 ;i<3; i++)
	{
		float3 h = normalize(-dirLight[i].dir + IN.viewDir);
		float nDotL = saturate(dot(IN.normal, -dirLight[i].dir));
		float nDotH = saturate(dot(IN.normal, h));
		float power = (nDotL == 0.0f) ? 0.0f : pow(nDotH, materialShininess);   

		diffuseColor += dirLight[i].enabled * dirLight[i].diffuse * nDotL;
		specularColor += dirLight[i].enabled * dirLight[i].specular * power;
	}

	//Material contribution
	diffuseColor *= materialDiffuse;
	diffuseColor += materialEmissive;
	specularColor *= materialSpecular;
	
	//Texture
	float3 finalColor = (tex2D(colorMap, IN.texCoord) * diffuseColor) + specularColor;
	//or... float3 finalColor = tex2D(colorMap, IN.texCoord) * (diffuseColor + specularColor);

	//Reflection
	if (materialReflection > 0.0f)
	{
		float3 envMapTex = reflect(normalize(IN.viewDir), normalize(IN.normal));
		float3 reflectedColor = texCUBE(environmentMap, envMapTex);
		finalColor += reflectedColor * materialReflection;
	}
	
	//Fog
	if (fog.enabled == true)
	{
		finalColor = lerp(finalColor, fog.color, IN.worldPos.w);
	}
		
	//Color with alpha	
	return float4(finalColor, materialAlpha);
}


//-----------------------------------------------------------------------------
// Techniques
//-----------------------------------------------------------------------------

technique Lighting20
{
	pass Pass1
	{
		VertexShader = compile vs_2_0 VS_Lighting20();
		PixelShader = compile ps_2_0 PS_Lighting20();
	}  	   		   
}