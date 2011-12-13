//@author: vvvv group
//@help: draws a mesh with a constant color
//@tags: template, basic
//@credits:

// --------------------------------------------------------------------------------------------------
// PARAMETERS:
// --------------------------------------------------------------------------------------------------

#define PI 3.14159265
//transforms
float4x4 tW: WORLD;        //the models world matrix
float4x4 tWIT: WORLDINVERSETRANSPOSE;
float4x4 tV: VIEW;         //view matrix as set via Renderer (EX9)
float4x4 tP: PROJECTION;   //projection matrix as set via Renderer (EX9)
float4x4 tWVP: WORLDVIEWPROJECTION;
float4x4 tWV: WORLDVIEW;
float4x4 tVP: VIEWPROJECTION;
float3 posCam : CAMERAPOSITION;
//material properties
float4 cAmb : COLOR <String uiname="Color";>  = {1, 1, 1, 1};

//texture
texture TexEnvironment <string uiname="Environment CubeMap";>;
samplerCUBE SampEnvironment = sampler_state    //sampler for doing the texture-lookup
{
    Texture   = (TexEnvironment);       //apply a texture to the sampler
    MipFilter = LINEAR;         //sampler states
    MinFilter = LINEAR;
    MagFilter = LINEAR;
};

float4x4 tTex: TEXTUREMATRIX <string uiname="Texture Coordinate Transform";>;
float4x4 tCub <string uiname="View Vector Transform";>;

//the data structure: vertexshader to pixelshader
//used as output data with the VS function
//and as input data with the PS function
struct vs2ps
{
    float4 Pos : POSITION;
    float4 TexCd : TEXCOORD0;
};

// --------------------------------------------------------------------------------------------------
// VERTEXSHADERS
// --------------------------------------------------------------------------------------------------

vs2ps VS(
    float4 Pos : POSITION,
    float4 TexCd : TEXCOORD0)
{
    //inititalize all fields of output struct with 0
    vs2ps Out = (vs2ps)0;

    //transform position
    Out.Pos = mul(Pos, tWVP);

    //transform texturecoordinates
    Out.TexCd = mul(TexCd, tTex);

    return Out;
}

// --------------------------------------------------------------------------------------------------
// PIXELSHADERS:
// --------------------------------------------------------------------------------------------------

float4 PS(vs2ps In): COLOR
{
    //In.TexCd = In.TexCd / In.TexCd.w; // for perpective texture projections (e.g. shadow maps) ps_2_0
	float3 look = 0;
	look.x = sin(In.TexCd.y*PI)*cos(In.TexCd.x*PI);
	look.y = sin(In.TexCd.y*PI)*sin(In.TexCd.x*PI);
	look.z = cos(In.TexCd.y*PI);
	look = mul(look, tCub);
    float4 col = texCUBE(SampEnvironment, look);
    return col;
}

// --------------------------------------------------------------------------------------------------
// TECHNIQUES:
// --------------------------------------------------------------------------------------------------

technique TConstant
{
    pass P0
    {
        //Wrap0 = U;  // useful when mesh is round like a sphere
        VertexShader = compile vs_2_0 VS();
        PixelShader = compile ps_2_0 PS();
    }
}