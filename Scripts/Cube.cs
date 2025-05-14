using System.Collections;
using System;
using UnityEngine;

public class Cube : CommonObject
{
    private WaitForSecondsRealtime _wait;

    private int _minTimeLife = 2;
    private int _maxTimeLife = 5;
    private int _timeLife;

    private bool _is—ollision = false;

    public event Action<Cube> Deactivated;

    protected override void OnEnable()
    {
        base.OnEnable();
        _is—ollision = false;
        Renderer.material.color = Color.white;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_is—ollision == false)
        {
            if (collision.collider.TryGetComponent(out Platform platform))
            {
                _is—ollision = true;
                Renderer.material.color = UnityEngine.Random.ColorHSV();

                _timeLife = GetRandomTimeLife();

                _wait = new WaitForSecondsRealtime(_timeLife);

                RestartWait();
            }
        }
    }

    private void RestartWait()
    {
        if (Coroutine != null)
        {
            StopCoroutine(Coroutine);
        }

        Coroutine = StartCoroutine(WaitRelease());
    }

    private IEnumerator WaitRelease()
    {
        yield return _wait;

        Deactivated?.Invoke(this);
    }

    private int GetRandomTimeLife()
    {
        return UnityEngine.Random.Range(_minTimeLife, _maxTimeLife + 1);
    }
}
