using TMPro;
using UnityEngine;

public abstract class SpawnerStatsView<TComponent,TSpawnable> : MonoBehaviour 
    where TComponent : Component
    where TSpawnable : BaseSpawner<TComponent> 
{
    [SerializeField] private TextMeshProUGUI _stats;

    protected TSpawnable Spawner { get; private set; }

    protected virtual void Awake()
    {
        Spawner = GetComponent<TSpawnable>();
    }

    protected virtual void Start()
    {
        ShowStatsText();
    }

    protected virtual void OnEnable() 
    {
        if (Spawner != null)
        {
            Spawner.StatsUpdated += ShowStatsText;
        }
    }

    protected virtual void OnDisable() 
    {
        if (Spawner != null)
        {
            Spawner.StatsUpdated -= ShowStatsText;
        }
    }

    protected void ShowStatsText(string name = "GameObject", int totalSpawned = 0, int totalCreated = 0, int activeCount = 0)
    {
        if (_stats != null)
        {
            _stats.text =
                (
                    $"Статистика: {name}\n" +
                    $"Количество заспавненых объектов: {totalSpawned}\n" +
                    $"Количество созданых объектов: {totalCreated}\n" +
                    $"Количество активных объектов: {activeCount}"
                );
        }
    }
}
