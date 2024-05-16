using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public abstract class PostEffectsBase : MonoBehaviour
{
    public Shader shader;
    public Material material;

    private Camera _camera;
    private Transform _cameraTrans;

    public Material Material
    {
        get
        {
            material = CheckShaderAndMaterial(shader, material);
            return material;
        }
    }

    public Camera Camera
    {
        get
        {
            if (_camera == null)
            {
                _camera = GetComponent<Camera>();
            }
            return _camera;
        }
    }

    public Transform CameraTrans
    {
        get
        {
            if (_cameraTrans == null)
            {
                _cameraTrans = Camera.transform;
            }
            return _cameraTrans;
        }
    }

    private void OnEnable()
    {
        GetComponent<Camera>().depthTextureMode |= DepthTextureMode.DepthNormals;
    }

    private void Start()
    {
        CheckResources();
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (Material)
        {
            MaterialSetProperties();
            Graphics.Blit(source, destination, Material);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

    public abstract void MaterialSetProperties();

    private Material CheckShaderAndMaterial(Shader shader, Material material)
    {
        if (shader == null)
        {
            return null;
        }

        if (shader.isSupported && material.shader == shader)
        {
            return material;
        }

        if (!shader.isSupported)
        {
            return null;
        }
        else
        {
            material = new Material(shader);
            material.hideFlags = HideFlags.DontSave;
            if (material)
            {
                return material;
            }
            else
            {
                return null;
            }
        }
    }

    private void CheckResources()
    {
        if (!CheckSupport())
        {
            NotSupported();
        }
    }

    private bool CheckSupport()
    {
        return true;
    }

    private void NotSupported()
    {
        enabled = false;
    }
}
