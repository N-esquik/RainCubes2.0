using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public abstract class BaseSpawner<TSpawnable> : MonoBehaviour where TSpawnable : Component
{
    [SerializeField] protected TSpawnable Prefab;
    [SerializeField] protected TextMeshProUGUI Stats;
    [SerializeField] protected float MinPosition;
    [SerializeField] protected float MaxPosition;
    [SerializeField] protected float PositionY;

    protected ObjectPool<TSpawnable> Pool;

    protected int CountObjectInPool = 5;
    protected int MaxSize = 5;

    protected int TotalSpawned = 0;
    protected int TotalCreated = 0;

    protected virtual void Awake()
    {
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
        UpdateStatsText();
    }

    public void SpawnPosition(Vector3 position)
    {
        var gameObject = Pool.Get();
        gameObject.transform.position = position;
        TotalSpawned++;
    }

    public void PublicRelease(TSpawnable spawnable) => Release(spawnable);

    protected virtual void UpdateStatsText()
    {
        if (Stats != null)
        {
            Stats.text =
                (
                    $"Статистика: {typeof(TSpawnable).Name}\n" +
                    $"Количество заспавненых объектов: {TotalSpawned}\n" +
                    $"Количество созданых объектов: {TotalCreated}\n" +
                    $"Количество активных объектов: {Pool.CountActive}"
                );
        }
    }

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
