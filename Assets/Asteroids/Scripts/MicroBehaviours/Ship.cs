using UnityEngine;

public class Ship : GameBehaviour
{
    public static Ship Spawn(GameObject prefab)
    {
        GameObject clone = Instantiate(prefab);
        var existingShip = clone.GetComponent<Ship>();
        return existingShip ? existingShip : clone.AddComponent<Ship>();
    }

    public virtual void Recover()
    {
        if (!IsAlive)
        {
            ResetTransform();
            gameObject.SetActive(true);
            ResetRigidbody();
        }
    }

    public virtual bool IsAlive { get { return gameObject.activeSelf; } }

    public void ResetTransform()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    public void ResetRigidbody() { RigidbodyExt.Reset(GetComponent<Rigidbody>()); }

    public void EnableControls()
    {
        GetComponent<ShipMovement>().enabled = true;
        GetComponent<ShipShooter>().enabled = true;
    }

    public void DisableControls()
    {
        GetComponent<ShipMovement>().enabled = false;
        GetComponent<ShipShooter>().enabled = false;
    }

    protected override void RequestDestruction()
    {
        DisableControls();
        gameObject.SetActive(false);
    }

    // This needs to be moved/refactored.
    public static bool ActiveShield(GameObject ship)
    {
        return ship.transform.FindChild("Shield").gameObject.activeSelf;
    }

    protected virtual void OnTriggerEnter(Collider bulletCollider)
    {
        if (ActiveShield(gameObject))
        {
            RemoveFromGame(bulletCollider.gameObject);
            return;
        }

        EnemyToken.exploder.Explode(gameObject.tag, transform.position);
        RequestDestruction();
    }
}
