using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class GaussianBlurEffect : MonoBehaviour
{
    public Shader blurShader;
    [Range(0, 4)] public int downsample = 1;
    [Range(1, 6)] public int iterations = 2;
    [Range(0.5f, 3f)] public float blurSize = 1.2f;

    Material _mat;
    void OnEnable()
    {
        if (blurShader == null) blurShader = Shader.Find("Hidden/GaussianBlur");
        if (_mat == null && blurShader != null) _mat = new Material(blurShader);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (_mat == null) { Graphics.Blit(src, dst); return; }

        int w = src.width >> downsample;
        int h = src.height >> downsample;

        RenderTexture rt1 = RenderTexture.GetTemporary(w, h, 0);
        RenderTexture rt2 = RenderTexture.GetTemporary(w, h, 0);

        _mat.SetFloat("_BlurSize", blurSize);

        Graphics.Blit(src, rt1); // downsample

        for (int i = 0; i < iterations; i++)
        {
            // Horizontal
            Graphics.Blit(rt1, rt2, _mat, 0);
            // Vertical
            Graphics.Blit(rt2, rt1, _mat, 1);
        }

        Graphics.Blit(rt1, dst);

        RenderTexture.ReleaseTemporary(rt1);
        RenderTexture.ReleaseTemporary(rt2);
    }
}
