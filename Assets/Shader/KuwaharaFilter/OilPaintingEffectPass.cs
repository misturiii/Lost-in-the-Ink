using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OilPaintingEffectPass : ScriptableRenderPass
{
    // private Material structureTensorMaterial;
    private readonly Material kuwaharaFilterMaterial;
    private RTHandle source;
    private RTHandle destination;
    // private RTHandle structureTensorTex;
    private RTHandle kuwaharaFilterTex;

    public OilPaintingEffectPass(Material kuwaharaFilterMaterial)
    {
        this.kuwaharaFilterMaterial = kuwaharaFilterMaterial;
    }

    public override void Configure(CommandBuffer cmd,
        RenderTextureDescriptor cameraTextureDescriptor)
    {
        // Configure the custom RTHandle
        RenderTextureDescriptor blitTargetDescriptor = cameraTextureDescriptor;
        blitTargetDescriptor.depthBufferBits = 0;
        blitTargetDescriptor.msaaSamples = 1;
        // RenderingUtils.ReAllocateIfNeeded(
        //     ref structureTensorTex, 
        //     blitTargetDescriptor, 
        //     FilterMode.Point,
        //     TextureWrapMode.Clamp,
		    // name: "_StructureTensorTex");
        RenderingUtils.ReAllocateIfNeeded(
            ref kuwaharaFilterTex, 
            blitTargetDescriptor, 
            FilterMode.Point, 
            TextureWrapMode.Clamp,
		    name: "_KuwaharaFilterTex");

        // Set the RTHandle as the output target
        // ConfigureTarget(structureTensorTex);
        ConfigureTarget(kuwaharaFilterTex);
    }
    
    public void Setup(Settings settings)
    {
        // SetupKuwaharaFilter(settings.anisotropicKuwaharaFilterSettings);
        kuwaharaFilterMaterial.SetInt("_Radius", settings.radius);
    }

    // private void SetupKuwaharaFilter(AnisotropicKuwaharaFilterSettings kuwaharaFilterSettings)
    // {
    //     kuwaharaFilterMaterial.SetInt("_FilterKernelSectors", kuwaharaFilterSettings.filterKernelSectors);
    //     kuwaharaFilterMaterial.SetTexture("_FilterKernelTex", kuwaharaFilterSettings.filterKernelTexture);
    //     kuwaharaFilterMaterial.SetFloat("_FilterRadius", kuwaharaFilterSettings.filterRadius);
    //     kuwaharaFilterMaterial.SetFloat("_FilterSharpness", kuwaharaFilterSettings.filterSharpness);
    //     kuwaharaFilterMaterial.SetFloat("_Eccentricity", kuwaharaFilterSettings.eccentricity);
    // }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get("Oil Paint Effect");

        ScriptableRenderer renderer = renderingData.cameraData.renderer;
        source = renderer.cameraColorTargetHandle;
        destination = renderer.cameraColorTargetHandle;

        Blitter.BlitCameraTexture(cmd, source, kuwaharaFilterTex, kuwaharaFilterMaterial, 0);
        // kuwaharaFilterMaterial.SetTexture("_StructureTensorTex", structureTensorTex);
        
        // Blitter.BlitCameraTexture(cmd, source, kuwaharaFilterTex, kuwaharaFilterMaterial, 0);
        Blitter.BlitCameraTexture2D(cmd, kuwaharaFilterTex, destination);
        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    public override void FrameCleanup(CommandBuffer cmd)
    {
        source = null;
        destination = null;
    }

    public void Dispose()
    {
        // RTHandles.Release(structureTensorTex);
        RTHandles.Release(kuwaharaFilterTex);
    }
}
