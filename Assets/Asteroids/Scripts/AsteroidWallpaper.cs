using System.Collections.Generic;
using UnityEngine;

public class AsteroidWallpaper : ScriptableObject
{
    public ObjectPool big, small;

    List<AsteroidBehaviour> asteroids = new List<AsteroidBehaviour>();

    public static AsteroidWallpaper New(ObjectPool big, ObjectPool small)
    {
        var instance = CreateInstance<AsteroidWallpaper>();
        instance.big = big;
        instance.small = small;
        return instance;
    }

    public virtual void ShowAsteroids()
    {
        SpawnAllAsteroids(big);
        SpawnAllAsteroids(small);
    }

    public virtual void HideAsteroids()
    {
        foreach (var asteroid in asteroids)
            asteroid.RemoveFromGame();
        asteroids.Clear();
    }

    protected virtual void SpawnAllAsteroids(ObjectPool pool)
    {
        while (!pool.IsEmpty)
            asteroids.Add(SpawnAsteroidFromPool(pool));
    }

    protected virtual AsteroidBehaviour SpawnAsteroidFromPool(ObjectPool pool)
    {
        AsteroidBehaviour asteroid = pool.GetRecyclable<AsteroidBehaviour>();
        asteroid.Spawn();
        return asteroid;
    }
}