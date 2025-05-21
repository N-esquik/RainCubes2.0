using TMPro;
using UnityEngine;

public class SpawnerView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _stats;

    private SpawnerCube _cubeSpawner;
    private SpawnerBomb _bombSpawner;

    private void Awake()
    {
        _cubeSpawner = GetComponent<SpawnerCube>();
        _bombSpawner = GetComponent<SpawnerBomb>();
    }

    private void Start()
    {
        ShowStatsText();
    }

    private void OnEnable()
    {
        if (_cubeSpawner != null)
        {
            _cubeSpawner.StatsUpdated += ShowStatsText;
        }

        if (_bombSpawner != null)
        {
            _bombSpawner.StatsUpdated += ShowStatsText;
        }
    }
    
    private void OnDisable()
    {
        if (_cubeSpawner != null)
        {
            _cubeSpawner.StatsUpdated -= ShowStatsText;
        }

        if (_bombSpawner != null)
        {
            _bombSpawner.StatsUpdated -= ShowStatsText;
        }
    }

    private void ShowStatsText(string name = "GameObject", int totalSpawned = 0, int totalCreated = 0, int activeCount = 0)
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
