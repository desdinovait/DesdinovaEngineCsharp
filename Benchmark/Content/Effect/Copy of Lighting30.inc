//-----------------------------------------------------------------------------
// Filename: Lighting30.fx
// Author: Daniele "Duff" Ferla
// Date: 30 August 2008
// Description: Lighting effect file for 3.0 shader cards
// Remarks: None
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Structs
//-----------------------------------------------------------------------------

struct VS_INPUT30
{
	float3 position : POSITION;
	float2 texCoord : TEXCOORD0;
	float3 normal : NORMAL;
};

struct VS_OUTPUT30
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

VS_OUTPUT30 VS_Lighting30(VS_INPUT30 IN)
{
	VS_OUTPUT30 OUT;
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

float4 PS_Lighting30(VS_OUTPUT30 IN) : COLOR
{  
	//Colors
	float3 diffuseColor = globalAmbient;
	float3 specularColor = 0;
		
	//Normal	
	float3 n = normalize(IN.normal);		
			
    //Directional lights
	for (int i=0 ;i<3; i++)
	{
		float3 h = normalize(-dirLight[i].dir + IN.viewDir);
		float nDotL = saturate(dot(IN.normal, -dirLight[i].dir));
		float nDotH = saturate(dot(IN.normal, h));
		float power = (nDotL == 0.0f) ? 0.0f : pow(nDotH, materialShininess);   

		diffuseColor  += dirLight[i].enabled * dirLight[i].diffuse * nDotL;
		specularColor += dirLight[i].enabled * dirLight[i].specular * power;
	}
	
	//Point lights
	if (useDynamicLights == true)
	{
		float3 l;
		float atten;
		for(int i=0; i<4; i++)
		{
			l = (pointLight[i].pos - IN.worldPos) / pointLight[i].radius;
			atten = saturate(1.0f - dot(l, l));
			l = normalize(l);

			float nDotL = saturate(dot(n, l));
			float nDotH = saturate(dot(n, normalize(l + IN.viewDir)));
			float power = (nDotL == 0.0f) ? 0.0f : pow(nDotH, materialShininess);

			diffuseColor  += pointLight[i].enabled * pointLight[i].diffuse * nDotL * atten;
			specularColor += pointLight[i].enabled * pointLight[i].specular * power * atten;			
		}
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

technique Lighting30
{
	pass Pass1
	{
		VertexShader = compile vs_3_0 VS_Lighting30();
		PixelShader = compile ps_3_0 PS_Lighting30();
	}	   		   
}