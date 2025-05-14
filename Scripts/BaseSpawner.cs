using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(SpawnerView))]

public abstract class BaseSpawner<TSpawnable> : MonoBehaviour where TSpawnable : Component
{
    [SerializeField] protected TSpawnable Prefab;
    [SerializeField] protected float MinPosition;
    [SerializeField] protected float MaxPosition;
    [SerializeField] protected float PositionY;

    protected SpawnerView SpawnerView;
    protected ObjectPool<TSpawnable> Pool;

    protected int CountObjectInPool = 5;
    protected int MaxSize = 5;

    protected int TotalSpawned = 0;
    protected int TotalCreated = 0;

    public int ActiveCount => Pool.CountActive;

    protected virtual void Awake()
    {
        SpawnerView = GetComponent<SpawnerView>();

        Pool = new ObjectPool<TSpawnable>
            (
                createFunc: () =>
                {
                    TSpawnable spawnable = Instantiate(Prefab);
                    TotalCreated++;
                    return spawnable;
                },
                actionOnGet: OnGet,
                actionOnRelease: OnRelease,
                actionOnDestroy: Destroy,
                collectionCheck: true,
                defaultCapacity: CountObjectInPool,
                maxSize: MaxSize
            );
    }

    protected virtual void Update()
    {
        SpawnerView.ShowStatsText(typeof(TSpawnable).Name,TotalSpawned,TotalCreated,ActiveCount);
    }

    public void SpawnPosition(Vector3 position)
    {
        var gameObject = Pool.Get();
        gameObject.transform.position = position;
        TotalSpawned++;
    }

    public void ReleasePublic(TSpawnable spawnable)
        => Release(spawnable);

    protected abstract void OnGet(TSpawnable spawnable);

    protected abstract void OnRelease(TSpawnable spawnable);

    protected void Release(TSpawnable spawnable)
    {
        Pool.Release(spawnable);
    }

    protected Vector3 GetSpawnPosition()
    {
        return new Vector3(Random.Range(MinPosition, MaxPosition), PositionY, Random.Range(MinPosition, MaxPosition));
    }
}
