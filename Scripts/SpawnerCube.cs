using System.Collections;
using UnityEngine;

public class SpawnerCube : BaseSpawner<Cube>
{
    [SerializeField] private SpawnerBomb _spawnerBomb;
    [SerializeField] private float _startTime = 0.1f;
    [SerializeField] private float _periodDrop = 0.2f;

    private Coroutine _coroutine;

    private void Start()
    {
        _coroutine = StartCoroutine(SpawnObject());
    }

    private void OnDisable()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }

    public void GetObject()
    {
        Pool.Get();
        TotalSpawned++;
    }

    protected override void OnGet(Cube cube)
    {
        cube.transform.position = GetSpawnPosition();
        cube.gameObject.SetActive(true);
        cube.Deactivated += OnCubeDeactivated;
    }

    protected override void OnRelease(Cube cube)
    {
        cube.gameObject.SetActive(false);
        cube.Deactivated -= OnCubeDeactivated;
    }

    private IEnumerator SpawnObject()
    {
        yield return new WaitForSeconds(_startTime);

        while (true)
        {
            GetObject();
            yield return new WaitForSeconds(_periodDrop);
        }
    }

    private void OnCubeDeactivated(Cube cube)
    {
        Release(cube);

        if (_spawnerBomb != null)
        {
            _spawnerBomb.SpawnPosition(cube.transform.position);
        }
    }
}