using System;
using UnityEngine;
using UnityEngine.Pool;

public abstract class BaseSpawner<TSpawnable> : MonoBehaviour where TSpawnable : Component
{
    [SerializeField] protected TSpawnable Prefab;
    [SerializeField] protected float MinPosition;
    [SerializeField] protected float MaxPosition;
    [SerializeField] protected float PositionY;

    protected ObjectPool<TSpawnable> Pool;

    protected int CountObjectInPool = 5;
    protected int MaxSize = 5;

    protected int TotalSpawned = 0;
    protected int TotalCreated = 0;

    public int ActiveCount => Pool.CountActive;

    public event Action<string, int, int, int> StatsUpdated;

    protected virtual void Awake()
    {
        Pool = new ObjectPool<TSpawnable>
            (
                createFunc: () =>
                {
                    TSpawnable spawnable = Instantiate(Prefab);
                    TotalCreated++;
                    NotifyStats();
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

    public void SpawnPosition(Vector3 position)
    {
        var gameObject = Pool.Get();
        gameObject.transform.position = position;
        TotalSpawned++;
        NotifyStats();
    }

    public void ReleasePublic(TSpawnable spawnable)
        => Release(spawnable);

    protected abstract void OnGet(TSpawnable spawnable);

    protected abstract void OnRelease(TSpawnable spawnable);

    protected void Release(TSpawnable spawnable)
    {
        Pool.Release(spawnable);
        NotifyStats();
    }

    protected Vector3 GetSpawnPosition()
    {
        return new Vector3(UnityEngine.Random.Range(MinPosition, MaxPosition), PositionY, UnityEngine.Random.Range(MinPosition, MaxPosition));
    }

    protected void NotifyStats()
    {
        StatsUpdated?.Invoke(typeof(TSpawnable).Name, TotalSpawned, TotalCreated, ActiveCount);
    }
}
