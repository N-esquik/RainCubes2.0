
public class SpawnerBomb : BaseSpawner<Bomb>
{
    protected override void OnGet(Bomb bomb)
    {
        bomb.Init(this);
        bomb.gameObject.SetActive(true);
        NotifyStats();
    }

    protected override void OnRelease(Bomb bomb)
    {
        bomb.gameObject.SetActive(false);
        NotifyStats();  
    }
}
