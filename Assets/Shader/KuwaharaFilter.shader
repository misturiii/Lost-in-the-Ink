Shader "Custom/KuwaharaFilter"
{
    HLSLINCLUDE
    
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        // The Blit.hlsl file provides the vertex shader (Vert),
        // the input structure (Attributes), and the output structure (Varyings)
        #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

        float _Radius = 0;
    
        float4 _BlitTexture_TexelSize;

        float4 KuwaharaFilter(Varyings input) : SV_Target
        {
            int numSample = 5;
            float2 corners[4] = {float2(1, 0), float2(0, 0), float2(1, 1), float2(0, 1)};
            UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
            float3 result = float3(1, 1, 1);
            float minVariance = 10e6;
            float weight = 1.0 / (numSample * numSample);

            for (int i = 0; i < 4; i++) {
                float mean = 0;
                float meansqr = 0;
                float3 color = float3(0, 0, 0);
                float2 offset = float2(corners[i].x * _Radius /1920, corners[i].y * _Radius / 1080);
                float2 corner = -numSample * offset;
                for (int y = 0; y < numSample; y++)
                {
                    for (int x = 0; x < numSample; x++)
                    {
                        float3 sample = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, 
                            input.texcoord + corner + float2(offset.x * x, offset.y * y)).rgb;
                        float factor = (0.2 * sample.r + 0.7 * sample.g + 0.1 * sample.b);
                        mean += factor;
                        meansqr += factor * factor;
                        color += sample;
                    }
                }
                float var = meansqr - mean * mean * weight;
                if (var < minVariance) {
                    minVariance = var;
                    result = color * weight;
                }
            }
            return float4(result, 1);
        }

    
    ENDHLSL
    
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline"}
        LOD 100
        ZWrite Off Cull Off

        Pass
        {
            Name "KuwaharaFilter"

            HLSLPROGRAM
            
            #pragma vertex Vert
            #pragma fragment KuwaharaFilter
            
            ENDHLSL
        }
    }
}
