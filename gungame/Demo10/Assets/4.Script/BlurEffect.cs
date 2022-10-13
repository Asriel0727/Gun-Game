using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Blur")]
public class BlurEffect : MonoBehaviour
{
    /// Blur iterations - larger number means more blur.
    public int iterations = 3;

    /// Blur spread for each iteration. Lower values
    /// give better looking blur, but require more iterations to
    /// get large blurs. Value is usually between 0.5 and 1.0.
    public float blurSpread = 0.6f;
   
    static Material m_Material = null;
    protected static Material material
    {
        get
        {
            if (m_Material == null)
            {
                m_Material = new Material(Shader.Find("BlurConeTap"));

                m_Material.hideFlags = HideFlags.HideAndDontSave;
                m_Material.shader.hideFlags = HideFlags.HideAndDontSave;
            }
            return m_Material;
        }
    }

    protected void OnDisable()
    {
        if (m_Material)
            DestroyImmediate(m_Material);
    }

    // --------------------------------------------------------

    protected void Awake()
    {
        enabled = false;
    }

    // Performs one blur iteration.
    public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
    {
        float off = 0.5f + iteration * blurSpread;
        Graphics.BlitMultiTap(source, dest, material,
            new Vector2(-off, -off),
            new Vector2(-off, off),
            new Vector2(off, off),
            new Vector2(off, -off)
        );
    }

    // Downsamples the texture to a quarter resolution.
    private void DownSample4x(RenderTexture source, RenderTexture dest)
    {
        float off = 1.0f;
        Graphics.BlitMultiTap(source, dest, material,
            new Vector2(-off, -off),
            new Vector2(-off, off),
            new Vector2(off, off),
            new Vector2(off, -off)
        );
    }

    // Called by the camera to apply the image effect
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        RenderTexture buffer = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0);
        RenderTexture buffer2 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0);

        // Copy source to the 4x4 smaller texture.
        DownSample4x(source, buffer);

        // Blur the small texture
        bool oddEven = true;
        for (int i = 0; i < iterations; i++)
        {
            if (oddEven)
                FourTapCone(buffer, buffer2, i);
            else
                FourTapCone(buffer2, buffer, i);
            oddEven = !oddEven;
        }
        if (oddEven)
            ImageEffects.Blit(buffer, destination);
        else
            ImageEffects.Blit(buffer2, destination);

        RenderTexture.ReleaseTemporary(buffer);
        RenderTexture.ReleaseTemporary(buffer2);
    }
}
