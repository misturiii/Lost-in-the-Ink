static float factors[5] = {1, 0.8, 0.6, 0.8, 1};

// Pixel shader for Gaussian blur
void blur_float(float2 uv, float lineWidth, out float OUT) {
    // Accumulate the color from multiple samples
    float color = 0;
    lineWidth *= 0.005;

    // Sample horizontally and vertically
    for (int i = -2; i <= 2; i++) {
        for (int j = -2; j <= 2; j++) {
            int diff = abs(i) - abs(j);
            color += _MainTex.Sample(sampler_MainTex, uv + float2(i * factors[diff + 2], j * factors[2 - diff]) * lineWidth).w;
        }
    OUT = color / 25.0; // Average the horizontal and vertical blur results
    }
}
