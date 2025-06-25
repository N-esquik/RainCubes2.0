
public class SpawnerBomb : BaseSpawner<Bomb>
{
    protected override void OnGet(Bomb bomb)
    {
        bomb.Exploded += ReturnToPool;
        bomb.gameObject.SetActive(true);
        NotifyStats();
    }

    protected override void OnRelease(Bomb bomb)
    {
        bomb.Exploded -= ReturnToPool;
        bomb.gameObject.SetActive(false);
        NotifyStats();
    }

    private void ReturnToPool(Bomb bomb)
    {
        Pool.Release(bomb);
        NotifyStats();
    }
}
