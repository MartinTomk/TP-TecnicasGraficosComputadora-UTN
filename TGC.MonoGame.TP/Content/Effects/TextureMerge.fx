#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float time;
//float2 deltaScreen;

texture baseTexture;
sampler2D textureSampler = sampler_state
{
    Texture = (baseTexture);
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};

texture overlayTexture;
sampler2D overlayTextureSampler = sampler_state
{
	Texture = (overlayTexture);
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};

texture overlayTexture2;
sampler2D overlayTextureSampler2 = sampler_state
{
    Texture = (overlayTexture2);
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};

texture samplerTxt;
sampler2D elipseSampler = sampler_state
{
    Texture = (samplerTxt);
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Mirror;
    AddressV = Mirror;
};
    
struct VertexShaderInput
{
    float4 Position : POSITION0;
    float2 TextureCoordinates : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float2 TextureCoordinates : TEXCOORD0;
    float4 WorldPosition : TEXCOORD1;
};


VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;

    output.Position = input.Position;
    output.TextureCoordinates = input.TextureCoordinates;
    output.WorldPosition = input.Position;
	
    return output;
}

// 2D Random
float random(in float2 st) {
    return frac(sin(dot(st.xy, float2(12.9898, 78.233))) * 43758.5453123);
}

float noise(in float2 st) {
    float2 i = floor(st);
    float2 f = frac(st);

    // Four corners in 2D of a tile
    float a = random(i);
    float b = random(i + float2(1.0, 0.0));
    float c = random(i + float2(0.0, 1.0));
    float d = random(i + float2(1.0, 1.0));

    // Smooth Interpolation

    // Cubic Hermine Curve.  Same as SmoothStep()
    float2 u = f * f * (3.0 - 2.0 * f);
    // u = smoothstep(0.,1.,f);

    // Mix 4 coorners percentages
    return lerp(a, b, u.x) + (c - a) * u.y * (1.0 - u.x) + (d - b) * u.x * u.y;
}

float sdEllipse(in float2 p, in float2 ab)
{
    p = abs(p); if (p.x > p.y) { p = p.yx; ab = ab.yx; }
    float l = ab.y * ab.y - ab.x * ab.x;
    float m = ab.x * p.x / l;      float m2 = m * m;
    float n = ab.y * p.y / l;      float n2 = n * n;
    float c = (m2 + n2 - 1.0) / 3.0; float c3 = c * c * c;
    float q = c3 + m2 * n2 * 2.0;
    float d = c3 + m2 * n2;
    float g = m + m * n2;
    float co;
    if (d < 0.0)
    {
        float h = acos(q / c3) / 3.0;
        float s = cos(h);
        float t = sin(h) * sqrt(3.0);
        float rx = sqrt(-c * (s + t + 2.0) + m2);
        float ry = sqrt(-c * (s - t + 2.0) + m2);
        co = (ry + sign(l) * rx + abs(g) / (rx * ry) - m) / 2.0;
    }
    else
    {
        float h = 2.0 * m * n * sqrt(d);
        float s = sign(q + h) * pow(abs(q + h), 1.0 / 3.0);
        float u = sign(q - h) * pow(abs(q - h), 1.0 / 3.0);
        float rx = -s - u - c * 4.0 + 2.0 * m2;
        float ry = (s - u) * sqrt(3.0);
        float rm = sqrt(rx * rx + ry * ry);
        co = (ry / sqrt(rm - rx) + 2.0 * g / rm - m) / 2.0;
    }
    float2 r = ab * float2(co, sqrt(1.0 - co * co));
    return length(r - p) * sign(p.y - r.y);
}

float sdBox(in float2 p, in float2 b)
{
    float2 d = abs(p) - b;
    return length(max(d, 0.0)) + min(max(d.x, d.y), 0.0);
}

float4 MergePS(VertexShaderOutput input) : COLOR
{
    float2 coords = input.TextureCoordinates * 2 - 1;
    float distanceToElipse = sdEllipse(coords, float2(.8, .6));
    float distanceToBox = sdBox(coords, float2(.8, .7));
    float mask = saturate(distanceToBox * 4);

    float noiseMix = noise(12 * input.WorldPosition.z + time * 0.1) + (sin(time * 0.1) + 1) / 2;
    float noiseMix2 = noise(12 * input.WorldPosition.z + time * 0.1) + (sin(time * 0.1 + 3.14159) + 1) / 2;

    float4 baseColor = tex2D(textureSampler, input.TextureCoordinates);
    float4 samplerG = tex2D(textureSampler, input.TextureCoordinates * 1.0004);
    float4 samplerB = tex2D(textureSampler, input.TextureCoordinates * 0.9993);
	float4 overlayColor = tex2D(overlayTextureSampler, input.TextureCoordinates);
    float4 overlayColor2 = tex2D(overlayTextureSampler2, input.TextureCoordinates);

    float4 mixOverlay = mask * overlayColor * noiseMix + overlayColor2 * noiseMix2;



    float3 color = float3(baseColor.r, 
        baseColor.g * (1 - mask) + samplerG.g * mask,
        baseColor.b * (1 - mask) + samplerB.b * mask);

    float timeFactor = 1 * noise(10 * input.WorldPosition.x + time * 2) + sin(input.WorldPosition.z) * .05 + .05;
    float4 finalColor = float4(color.rgb, 1) + float4(mixOverlay.rgb, (mixOverlay.r + mixOverlay.a * 0.1) * saturate(timeFactor)) * 0.1;
    
    //float4 finalColor = float4(color * (1 - mask),1);

    return finalColor;

}

technique Merge
{
    pass Pass0
    {
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MergePS();
	}
};