using TMPro;
using UnityEngine;

public class SpawnerView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _stats;

    public void ShowStatsText(string name, int totalSpawned, int totalCreated, int activeCount)
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
