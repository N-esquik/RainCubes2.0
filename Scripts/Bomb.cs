using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Renderer), typeof(Collider))]

public class Bomb : MonoBehaviour
{
    [SerializeField] private float _fadeDuration = 2f;
    [SerializeField] private float _explosionForce = 500f;
    [SerializeField] private float _explosionRadius = 5f;

    private Renderer _renderer;
    private Coroutine _fadeCoroutine;
    private SpawnerBomb _spawner;
    private Material _material;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();

        _material = _renderer.material;
        SetupMaterial();
    }

    private void OnEnable()
    {
        ResetColor();
        _fadeCoroutine = StartCoroutine(FadeAndExplode());
    }

    private void OnDisable()
    {
        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
        }
    }

    public void Init(SpawnerBomb spawner)
    {
        _spawner = spawner;
    }

    private void SetupMaterial()
    {
        _material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        _material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        _material.SetInt("_ZWrite", 0);
        _material.DisableKeyword("_ALPHATEST_ON");
        _material.EnableKeyword("_ALPHABLEND_ON");
        _material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        _material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    }

    private void ResetColor()
    {
        Color color = Color.black;
        color.a = 1f;
        _material.color = color;
    }

    private IEnumerator FadeAndExplode()
    {
        float fadeTime = Random.Range(2f, 5f);
        float elapsedTime = 0f;

        Color color = _material.color;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeTime);
            _material.color = color;
            yield return null;
        }

        Explode();
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (Collider hit in colliders)
        {
            Rigidbody rigidbody = hit.GetComponent<Rigidbody>();

            if (rigidbody != null)
            {
                rigidbody.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
            }
        }

        _spawner.PublicRelease(this);
    }
}