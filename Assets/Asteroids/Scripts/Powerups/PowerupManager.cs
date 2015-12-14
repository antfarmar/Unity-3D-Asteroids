using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "PowerupManager", menuName = "PowerupManager")]
public class PowerupManager : ScriptableObject
{
    [Range(5f,10f)]
    public float maxTimeBetweenSpawns = 10f;

    public Powerup[] powerupPrefabs;


    public IEnumerator PowerupSpawner()
    {
        while (true)
        {
            var wait = Random.Range(5f, maxTimeBetweenSpawns);
            yield return new WaitForSeconds(wait);

            var prefab = powerupPrefabs[Random.Range(0, powerupPrefabs.Length)];
            Powerup powerup  = Instantiate(prefab);
            int mask = LayerMask.GetMask("Asteroid");
            float collisionSphereRadius = powerup.transform.localScale.x;
            Vector3 position = Spawn.FindSuitableSpawnLocation(mask, collisionSphereRadius);
            powerup.SpawnAt(position);
        }
    }
}
