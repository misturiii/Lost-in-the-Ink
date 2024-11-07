static float3x3 filter = float3x3(-1, -2, -1, 0, 0, 0, 1, 2, 1);

static float factors[5] = {1, 0.8, 0.6, 0.8, 1};

void GaussianBlur_float (float2 IN, out float4 OUT, out float OUTLINE) {
    // Accumulate the color from multiple samples
    float colorx = 0;
    float colory = 0;
    float amount = _BlurAmount / 1024;
    // Sample horizontally and vertically
    for (int i = -1; i <= 1; i++) {
        for (int j = -1; j <= 1; j++) {
            float4 sample = _MainTex.Sample(sampler_MainTex, IN + float2(i, j) * amount);
            float grey = sample.r * 0.2 + sample.g * 0.7 + sample.b * 0.1;
            colorx += grey * filter[i + 1, j + 1];
            colory += grey * filter[j + 1, i + 1];
        }
    }
    amount *= 0.1;
    OUTLINE = sqrt(colorx * colorx + colory * colory); // Average the horizontal and vertical blur results
    float4 color = 0;
    for (int i = -2; i <= 2; i++) {
        for (int j = -2; j <= 2; j++) {
            int diff = abs(i) - abs(j);
            color += _MainTex.Sample(sampler_MainTex, IN + float2(i * factors[diff + 2], j * factors[2 - diff]) * amount * (1 - OUTLINE));
        }
    }
    OUT = color / 25.0; // Average the horizontal and vertical blur results
}