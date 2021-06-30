#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif
#define SMOOTH(r,R) (1.0-smoothstep(R-1.0,R+1.0, r))

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


float circle(float2 uv, float2 center, float radius, float width)
{
    float r = length(uv - center);
    return SMOOTH(r - width / 2.0, radius) - SMOOTH(r + width / 2.0, radius);
}


float4 MergePS(VertexShaderOutput input) : COLOR
{
    float2 uv = input.TextureCoordinates;

    float2 coords = input.TextureCoordinates * 2 - 1;
    float distanceToElipse = sdEllipse(coords, float2(.9, .8));
    float distanceToBox = sdBox(coords, float2(.8, .7));
    float mask = saturate(distanceToElipse * 2);

    float4 baseColor = tex2D(textureSampler, input.TextureCoordinates);
    float4 mixOverlay = saturate(mask + 0.5) * baseColor;

    float3 color = float3(0, baseColor.g * (1 - mask), 0);

    float4 finalColor = float4(color.rgb, 1);

    float circleColor = circle(input.TextureCoordinates, float2(1280,720), 100.0, 1.0) + circle(input.TextureCoordinates, float2(1280, 720), 165.0, 1.0);
    
    float aspectRatio = 1.77778;


    float2 circleCenter = float2(0.5 * aspectRatio, 0.5);
    float distanceToCircle = distance(float2(uv.x * aspectRatio, uv.y), circleCenter);
    float boxMask = step(sdBox(coords, float2(.04, .002)), 0.001);
    float target = (step(distanceToCircle, 0.15) - step(distanceToCircle, 0.148)) * 0.4;
    target += (step(distanceToCircle, 0.3) - step(distanceToCircle, 0.297)) * 0.3;
    float boxMask2 = step(sdBox(coords, float2(.8, sin(time * 2) * 0.1 + 0.4)), 0.4);
    target += (step(distanceToCircle, 0.6) - step(distanceToCircle, 0.596)) * 0.2 * boxMask2;

    
    //circle *= boxMask;

    //finalColor.rgb += float3(target * 0.5, target * 0.5, target * 0.5);

    finalColor.rgb += float3(boxMask, boxMask, boxMask);
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

/*


#define RANGE(a,b,x) ( step(a,x)*(1.0-step(b,x)) )
#define RS(a,b,x) ( smoothstep(a-1.0,a+1.0,x)*(1.0-smoothstep(b-1.0,b+1.0,x)) )
#define M_PI 3.1415926535897932384626433832795

#define blue1 vec3(0.74,0.95,1.00)
#define blue2 vec3(0.87,0.98,1.00)
#define blue3 vec3(0.35,0.76,0.83)
#define blue4 vec3(0.953,0.969,0.89)
#define red   vec3(1.00,0.38,0.227)

#define MOV(a,b,c,d,t) (vec2(a*cos(t)+b*cos(0.1*(t)), c*sin(t)+d*cos(0.1*(t))))

float movingLine(vec2 uv, vec2 center, float radius)
{
    //angle of the line
    float theta0 = 90.0 * iTime;
    vec2 d = uv - center;
    float r = sqrt(dot(d, d));
    if (r < radius)
    {
        //compute the distance to the line theta=theta0
        vec2 p = radius * vec2(cos(theta0 * M_PI / 180.0),
            -sin(theta0 * M_PI / 180.0));
        float l = length(d - p * clamp(dot(d, p) / dot(p, p), 0.0, 1.0));
        d = normalize(d);
        //compute gradient based on angle difference to theta0
        float theta = mod(180.0 * atan(d.y, d.x) / M_PI + theta0, 360.0);
        float gradient = clamp(1.0 - theta / 90.0, 0.0, 1.0);
        return SMOOTH(l, 1.0) + 0.5 * gradient;
    }
    else return 0.0;
}

float circle(vec2 uv, vec2 center, float radius, float width)
{
    float r = length(uv - center);
    return SMOOTH(r - width / 2.0, radius) - SMOOTH(r + width / 2.0, radius);
}

float circle2(vec2 uv, vec2 center, float radius, float width, float opening)
{
    vec2 d = uv - center;
    float r = sqrt(dot(d, d));
    d = normalize(d);
    if (abs(d.y) > opening)
        return SMOOTH(r - width / 2.0, radius) - SMOOTH(r + width / 2.0, radius);
    else
        return 0.0;
}
float circle3(vec2 uv, vec2 center, float radius, float width)
{
    vec2 d = uv - center;
    float r = sqrt(dot(d, d));
    d = normalize(d);
    float theta = 180.0 * (atan(d.y, d.x) / M_PI);
    return smoothstep(2.0, 2.1, abs(mod(theta + 2.0, 45.0) - 2.0)) *
        mix(0.5, 1.0, step(45.0, abs(mod(theta, 180.0) - 90.0))) *
        (SMOOTH(r - width / 2.0, radius) - SMOOTH(r + width / 2.0, radius));
}

float triangles(vec2 uv, vec2 center, float radius)
{
    vec2 d = uv - center;
    return RS(-8.0, 0.0, d.x - radius) * (1.0 - smoothstep(7.0 + d.x - radius, 9.0 + d.x - radius, abs(d.y)))
        + RS(0.0, 8.0, d.x + radius) * (1.0 - smoothstep(7.0 - d.x - radius, 9.0 - d.x - radius, abs(d.y)))
        + RS(-8.0, 0.0, d.y - radius) * (1.0 - smoothstep(7.0 + d.y - radius, 9.0 + d.y - radius, abs(d.x)))
        + RS(0.0, 8.0, d.y + radius) * (1.0 - smoothstep(7.0 - d.y - radius, 9.0 - d.y - radius, abs(d.x)));
}

float _cross(vec2 uv, vec2 center, float radius)
{
    vec2 d = uv - center;
    int x = int(d.x);
    int y = int(d.y);
    float r = sqrt(dot(d, d));
    if ((r < radius) && ((x == y) || (x == -y)))
        return 1.0;
    else return 0.0;
}
float dots(vec2 uv, vec2 center, float radius)
{
    vec2 d = uv - center;
    float r = sqrt(dot(d, d));
    if (r <= 2.5)
        return 1.0;
    if ((r <= radius) && ((abs(d.y + 0.5) <= 1.0) && (mod(d.x + 1.0, 50.0) < 2.0)))
        return 1.0;
    else if ((abs(d.y + 0.5) <= 1.0) && (r >= 50.0) && (r < 115.0))
        return 0.5;
    else
        return 0.0;
}
float bip1(vec2 uv, vec2 center)
{
    return SMOOTH(length(uv - center), 3.0);
}
float bip2(vec2 uv, vec2 center)
{
    float r = length(uv - center);
    float R = 8.0 + mod(87.0 * iTime, 80.0);
    return (0.5 - 0.5 * cos(30.0 * iTime)) * SMOOTH(r, 5.0)
        + SMOOTH(6.0, r) - SMOOTH(8.0, r)
        + smoothstep(max(8.0, R - 20.0), R, r) - SMOOTH(R, r);
}
void mainImage(out vec4 fragColor, in vec2 fragCoord)
{
    vec3 finalColor;
    vec2 uv = fragCoord.xy;
    //center of the image
    vec2 c = iResolution.xy / 2.0;
    finalColor = vec3(0.3 * _cross(uv, c, 240.0));
    finalColor += (circle(uv, c, 100.0, 1.0)
        + circle(uv, c, 165.0, 1.0)) * blue1;
    finalColor += (circle(uv, c, 240.0, 2.0));//+ dots(uv,c,240.0)) * blue4;
    finalColor += circle3(uv, c, 313.0, 4.0) * blue1;
    finalColor += triangles(uv, c, 315.0 + 30.0 * sin(iTime)) * blue2;
    finalColor += movingLine(uv, c, 240.0) * blue3;
    finalColor += circle(uv, c, 10.0, 1.0) * blue3;
    finalColor += 0.7 * circle2(uv, c, 262.0, 1.0, 0.5 + 0.2 * cos(iTime)) * blue3;
    if (length(uv - c) < 240.0)
    {
        //animate some bips with random movements
        vec2 p = 130.0 * MOV(1.3, 1.0, 1.0, 1.4, 3.0 + 0.1 * iTime);
        finalColor += bip1(uv, c + p) * vec3(1, 1, 1);
        p = 130.0 * MOV(0.9, -1.1, 1.7, 0.8, -2.0 + sin(0.1 * iTime) + 0.15 * iTime);
        finalColor += bip1(uv, c + p) * vec3(1, 1, 1);
        p = 50.0 * MOV(1.54, 1.7, 1.37, 1.8, sin(0.1 * iTime + 7.0) + 0.2 * iTime);
        finalColor += bip2(uv, c + p) * red;
    }

    fragColor = vec4(finalColor, 1.0);
}

*/