using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Renderer), typeof(Collider))]

public class CommonObject : MonoBehaviour
{
    public Coroutine Coroutine { get; protected set; }
    public Renderer Renderer { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    public  Material Material { get; private set; }

    protected virtual void Awake()
    {
        Renderer = GetComponent<Renderer>();
        Rigidbody = GetComponent<Rigidbody>();
        Material = GetComponent<Material>();
    }

    protected virtual void OnEnable()
    {
        Rigidbody.velocity = Vector3.zero;
    }

    protected virtual void OnDisable()
    {
        if (Coroutine != null)
        {
            StopCoroutine(Coroutine);
            Coroutine = null;
        }
    }

    protected void SetupMaterial()
    {
        Material = Renderer.material;
        Material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        Material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        Material.SetInt("_ZWrite", 0);
        Material.DisableKeyword("_ALPHATEST_ON");
        Material.EnableKeyword("_ALPHABLEND_ON");
        Material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        Material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    }
}
