using System;
using System.Collections;
using UnityEngine;

public class Bomb : MaterialEntity
{
    [SerializeField] private float _explosionForce = 500f;
    [SerializeField] private float _explosionRadius = 5f;
    [SerializeField] private float _minFadeTime = 2f;
    [SerializeField] private float _maxFadeTime = 5f;

    public event Action<Bomb> Exploded;
 
    protected override void Awake()
    {
        base.Awake();
        SetupMaterial();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        ResetColor();
        Coroutine = StartCoroutine(Fade());
    }

    private void ResetColor()
    {
        Color color = Color.black;
        color.a = 1f;
        Material.color = color;
    }

    private IEnumerator Fade()
    {
        float fadeTime = UnityEngine.Random.Range(_minFadeTime, _maxFadeTime);
        float elapsedTime = 0f;

        Color color = Material.color;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeTime);
            Material.color = color;
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

        Exploded?.Invoke(this);
    }
}