using UnityEngine;

public class SpawnerBomb : BaseSpawner<Bomb>
{
    public void Spawn(Vector3 position)
    {
        var spawnedObject = Pool.Get();
        spawnedObject.transform.position = position;
        TotalSpawned++;
        NotifyStats();
    }

    protected override void OnGet(Bomb bomb)
    {
        bomb.Exploded += Release;
        bomb.gameObject.SetActive(true);
        NotifyStats();
    }

    protected override void OnRelease(Bomb bomb)
    {
        bomb.Exploded -= Release;
        bomb.gameObject.SetActive(false);
        NotifyStats();
    }
}
