using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PowerupManager", menuName = "PowerupManager")]
public class PowerupManager : ScriptableObject
{
    [Range(10, 30)]
    public int minSpawnWait = 10;

    [Range(10, 30)]
    public int maxSpawnWait = 30;

    public Powerup[] powerupPrefabs;
    private List<Powerup> powerupList;


    void OnDisable() { powerupList = null; }

    public void InstantiatePowerups()
    {
        powerupList = new List<Powerup>(powerupPrefabs.Length);
        foreach (var prefab in powerupPrefabs) powerupList.Add(Instantiate(prefab));
    }

    public void HideAllPowerups()
    {
        foreach (var powerup in powerupList)
            powerup.HideInScene();
    }

    public IEnumerator StartSpawner(Ship ship)
    {
        if (powerupList.Count == 0) InstantiatePowerups();
        while (true)
        {
            var wait = Random.Range(minSpawnWait, maxSpawnWait);
            yield return new WaitForSeconds(wait);
            if (ship.IsAlive)
            {
                var powerup = powerupList[Random.Range(0, powerupList.Count)];
                if (!powerup.isVisible) powerup.ShowInScene();
            }
        }
    }
}
