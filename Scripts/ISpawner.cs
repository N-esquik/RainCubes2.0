using System;

public interface ISpawner<TSpawnable>
{
    public int ActiveCount { get; }
    public int TotalSpawned { get; }
    public int TotalCreated { get; }

    public event Action<string, int, int, int> StatsUpdated;

    public void NotifyStats();
}
