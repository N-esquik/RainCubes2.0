using UnityEngine;

public class SpawnerCube : BaseSpawner<Cube>
{
    [SerializeField] private SpawnerBomb _spawnerBomb;
    [SerializeField] private float _startTime = 0.1f;
    [SerializeField] private float _periodDrop = 0.2f;

    private void Start()
    {
        InvokeRepeating(nameof(GetObject), _startTime, _periodDrop);
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
        cube.CubeDeactivated += OnCubeDeactivated;
    }

    protected override void OnRelease(Cube cube)
    {
        cube.gameObject.SetActive(false);
        cube.CubeDeactivated -= OnCubeDeactivated;
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