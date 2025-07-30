using System.Collections;
using UnityEngine;

public class SpawnerCube : BaseSpawner<Cube>
{
    [SerializeField] private SpawnerBomb _spawnerBomb;
    [SerializeField] private float _startTime = 0.1f;
    [SerializeField] private float _periodDrop = 0.2f;

    private Coroutine _coroutine;
    private WaitForSeconds _startWait;
    private WaitForSeconds _spawnWait;

    private void Start()
    {
        _coroutine = StartCoroutine(SpawnObject());
        _startWait = new WaitForSeconds(_startTime);
        _spawnWait = new WaitForSeconds(_periodDrop);
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
        yield return _startWait;

        while (enabled)
        {
            GetObject();
            yield return _spawnWait;
        }
    }

    private void OnCubeDeactivated(Cube cube)
    {
        Release(cube);

        if (_spawnerBomb != null)
        {
            _spawnerBomb.Spawn(cube.transform.position);
        }
    }
}