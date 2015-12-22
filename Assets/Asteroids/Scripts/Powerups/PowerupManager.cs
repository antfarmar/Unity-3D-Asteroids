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

    public void HideAllPowerups()
    {
        foreach (var powerup in powerupList)
            powerup.RemoveFromGame();
    }

    public void DenyAllPower()
    {
        foreach (var powerup in powerupList)
            powerup.DenyPower();
    }

    public void InstantiatePowerups(GameObject receiver)
    {
        powerupList = new List<Powerup>(powerupPrefabs.Length);
        foreach (var prefab in powerupPrefabs)
        {
            Powerup powerup = Instantiate(prefab);
            powerup.SetReceiver(receiver);
            powerupList.Add(powerup);
        }
    }

    public IEnumerator SpawnPowerupsFor(GameObject receiver)
    {
        if (powerupList.Count == 0) InstantiatePowerups(receiver);
        while (true)
        {
            var wait = Random.Range(minSpawnWait, maxSpawnWait);
            yield return new WaitForSeconds(wait);
            if (receiver.activeInHierarchy)
            {
                var powerup = powerupList[Random.Range(0, powerupList.Count)];
                if (!powerup.isVisible) powerup.ActivateTemporarily();
            }
        }
    }
}
