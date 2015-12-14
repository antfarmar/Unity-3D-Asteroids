using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "PowerupManager", menuName = "PowerupManager")]
public class PowerupManager : ScriptableObject
{
    public Powerup[] powerupPrefabs;

    public void ChanceSpawn()
    {
        foreach (var pu in powerupPrefabs)
        {
            if (Random.value > 0.5f)
            {
                Instantiate(pu, Vector3.zero, Quaternion.identity);
            }
        }
    }

}
