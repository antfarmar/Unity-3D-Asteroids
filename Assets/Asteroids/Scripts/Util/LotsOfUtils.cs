using UnityEngine;

[System.Serializable]
class UniformRandomVector3
{
    [Range(0, 5)]
    [SerializeField]
    float minScale = 1.0f;

    [Range(0, 5)]
    [SerializeField]
    float maxScale = 1.5f;

    public Vector3 Randomize()
    {
        return Vector3X.RandomUniform(minScale, maxScale);
    }
}

public static class Vector3X
{
    public static Vector3 RandomUniform(float min, float max)
    {
        float r = Random.Range(min, max);
        return new Vector3(r, r, r);
    }
}

public static class Viewport
{
    public static Vector3 GetRandomWorldPositionXY()
    {
        Vector3 randomXY = new Vector3(Random.value, Random.value);
        return ViewportToWorldPointXY(randomXY);
    }

    static Vector3 ViewportToWorldPointXY(Vector3 viewPoint)
    {
        Vector3 world = Camera.main.ViewportToWorldPoint(viewPoint);
        world.z = 0; // Eh, is that even necessary?
        return world;
    }
}

public static class RigidbodyExt
{
    public static void Reset(Rigidbody rb)
    {
        rb.position = Vector3.zero;
        rb.rotation = Quaternion.identity;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public static void SetRandomForce(Rigidbody rb, float maxForce)
    {
        Vector3 randomForce = maxForce * Random.insideUnitSphere;
        rb.velocity = Vector3.zero;
        rb.AddForce(randomForce);
    }

    public static void SetRandomTorque(Rigidbody rb, float maxTorque)
    {
        Vector3 randomTorque = maxTorque * Random.insideUnitSphere;
        rb.angularVelocity = Vector3.zero;
        rb.AddTorque(randomTorque);
    }
}