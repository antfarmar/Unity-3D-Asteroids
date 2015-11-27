using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ScreenWrapper : MonoBehaviour
{
    [SerializeField]
    [HideInInspector]
    public UnityEvent beforeWrap;

    bool IsVisible = true;

    void Update()
    {
        if (IsVisible)
            return;
        else
        {
            ScreenWrap();
            IsVisible = true;
        }
    }

    IEnumerator OnBecameInvisible()
    {
        IsVisible = false;
        yield return new WaitForSeconds(1f);
    }


    void OnBecameVisible()
    {
        IsVisible = true;
    }

    void ScreenWrap()
    {
        Vector3 viewport = GetViewportPosition();
        bool wrapX = viewport.x < 0 || viewport.x > 1;
        bool wrapY = viewport.y < 0 || viewport.y > 1;

        if (wrapX || wrapY)
        {
            beforeWrap.Invoke();
            transform.position = NegateXY(transform.position, wrapX, wrapY);
        }
    }

    Vector3 GetViewportPosition()
    {
        return Camera.main.WorldToViewportPoint(transform.position);
    }

    static Vector3 NegateXY(Vector3 vector, bool negateX, bool negateY)
    {
        return new Vector3
        {
            x = negateX ? -vector.x : vector.x,
            y = negateY ? -vector.y : vector.y,
            z = vector.z
        };
    }
}