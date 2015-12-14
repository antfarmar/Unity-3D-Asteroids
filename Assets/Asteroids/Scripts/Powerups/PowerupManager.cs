using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "PowerupManager", menuName = "PowerupManager")]
public class PowerupManager : ScriptableObject
{
    public Powerup[] powerupPrefabs;

    public void ChanceSpawn()
    {
        foreach (var prefab in powerupPrefabs)
        {
            if (Random.value > 0.5f)
            {
                int mask = LayerMask.GetMask("Asteroid");
                float collisionSphereRadius = 2f;
                Vector3 position = Spawn.FindSuitableSpawnLocation(mask, collisionSphereRadius);
                Powerup powerup  = Instantiate<Powerup>(prefab);
                powerup.SpawnAt(position);
            }
        }
    }

}
