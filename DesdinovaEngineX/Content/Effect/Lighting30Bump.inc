//-----------------------------------------------------------------------------
// Filename: Lighting30.fx
// Author: Daniele "Duff" Ferla
// Date: 30 August 2008
// Description: Lighting effect file for 3.0 shader cards
// Remarks: None
//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------
// Input/Output Structures
//-----------------------------------------------------------------------------

struct VS_INPUT30BUMP
{
	float3 position : POSITION;
	float2 texCoord : TEXCOORD0;
	float3 normal : NORMAL;
	float4 tangent : TANGENT;
};

struct VS_OUTPUT30BUMP
{
	float4 position : POSITION;
	float4 worldPos : TEXCOORD0;		//xyz:position, w:fog
	float2 texCoord : TEXCOORD1;
	float3 viewDir : TEXCOORD2;
	float3 normal : TEXCOORD3;
	float3 tangent : TEXCOORD4;
	float3 bitangent : TEXCOORD5;	
};


//-----------------------------------------------------------------------------
// Vertex Shaders
//-----------------------------------------------------------------------------

VS_OUTPUT30BUMP VS_Lighting30Bump(VS_INPUT30BUMP IN)
{
	VS_OUTPUT30BUMP OUT;
	float4x4 preWorldViewProj = mul(mul(worldMatrix,viewMatrix),projectionMatrix);
	OUT.position = mul(float4(IN.position, 1.0f), preWorldViewProj);
	OUT.worldPos.xyz = mul(float4(IN.position, 1.0f), worldMatrix).xyz;
	OUT.texCoord = IN.texCoord;
	OUT.viewDir = normalize(cameraPos - OUT.worldPos);
	OUT.normal = normalize(mul(IN.normal, (float3x3)worldInverseTransposeMatrix));
	OUT.tangent = mul(IN.tangent.xyz, (float3x3)worldInverseTransposeMatrix);
	OUT.bitangent = cross(OUT.normal, OUT.tangent) * IN.tangent.w;
	float d = length(OUT.worldPos - cameraPos);    
    OUT.worldPos.w = fog.enabled * (saturate((d - fog.start) / (fog.end - fog.start) / clamp(OUT.worldPos.y / fog.altitude + 1, 1, fog.thinning)));

	return OUT;
}


//-----------------------------------------------------------------------------
// Pixel Shaders
//-----------------------------------------------------------------------------

float4 PS_Lighting30Bump(VS_OUTPUT30BUMP IN) : COLOR
{  
	//Colors
	float3 diffuseColor = globalAmbient;
	float3 specularColor = 0;
    
    //Normal
	float3 n = normalize(IN.normal);
	
    //TBN matrix
    float3x3 tbnMatrix = float3x3(0, 0, 0, 0, 0, 0, 0, 0, 0);
    float3 v = 0;
    if (materialNormalMapEnabled == true)
    {
		float3 t = normalize(IN.tangent);
		float3 b = normalize(IN.bitangent);
		tbnMatrix = float3x3(t.x, b.x, n.x, t.y, b.y, n.y, t.z, b.z, n.z);
		
		//Texture normal
		v = normalize(mul(cameraPos - IN.worldPos, tbnMatrix));
		n = normalize(tex2D(normalMap, IN.texCoord).rgb * 2.0f - 1.0f);		//Model must have "Generate Tanget Frames" enabled into processor properties
    }
    
    //Directional lights
	for (int i=0 ;i<3; i++)
	{
		float3 h = float3(0,0,0);
		float nDotL = 0;
		float nDotH = 0;
	    if (materialNormalMapEnabled == true)
		{
			h = mul(normalize(-dirLight[i].dir + IN.viewDir), tbnMatrix);
			nDotL = saturate(dot(n, normalize(mul(-dirLight[i].dir, tbnMatrix))));
			nDotH = saturate(dot(n, h));
		}
		else
		{
			h = normalize(-dirLight[i].dir + IN.viewDir);
			nDotL = saturate(dot(IN.normal, -dirLight[i].dir));
			nDotH = saturate(dot(IN.normal, h));	
		}
		
		float power = (nDotL == 0.0f) ? 0.0f : pow(nDotH, materialShininess);   
		diffuseColor += dirLight[i].enabled * dirLight[i].diffuse * nDotL;
		specularColor += dirLight[i].enabled * dirLight[i].specular * power;
	}
	
	//Point lights
	if (useDynamicLights == true)
	{
		float3 l;
		float atten;
		float nDotL = 0;
		float nDotH = 0;	
		for(int i=0; i<4; i++)
		{
			if (materialNormalMapEnabled == true)
			{
				l = mul((pointLight[i].pos - IN.worldPos) / pointLight[i].radius, tbnMatrix);
				atten = saturate(1.0f - dot(l, l));
				l = normalize(l);
				
				nDotL = saturate(dot(n, l));
				nDotH = saturate(dot(n, normalize(l + v)));
			}
			else
			{
				l = (pointLight[i].pos - IN.worldPos) / pointLight[i].radius;
				atten = saturate(1.0f - dot(l, l));
				l = normalize(l);

				nDotL = saturate(dot(IN.normal, l));
				nDotH = saturate(dot(IN.normal, normalize(l + IN.viewDir)));
			}
			
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
		float3 envMapTex = reflect(normalize(IN.viewDir), normalize(n));
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

technique Lighting30Bump
{
	pass Pass1
	{
		VertexShader = compile vs_3_0 VS_Lighting30Bump();
		PixelShader = compile ps_3_0 PS_Lighting30Bump();
	}	   		   
}