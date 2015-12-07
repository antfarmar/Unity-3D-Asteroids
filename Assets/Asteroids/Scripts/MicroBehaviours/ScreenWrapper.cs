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
        if (!IsVisible)
        {
            IsVisible = true;
            ScreenWrap();
        }
    }

    void OnBecameInvisible()
    {
        IsVisible = false;
    }

    void OnBecameVisible()
    {
        IsVisible = true;
    }

    void ScreenWrap()
    {
        Vector3 viewport = GetViewportPosition();
        bool isHorizontallyOutOfView = viewport.x < 0 || viewport.x > 1;
        bool isVerticallyOutOfView = viewport.y < 0 || viewport.y > 1;

        if (isHorizontallyOutOfView || isVerticallyOutOfView)
        {
            beforeWrap.Invoke();
            transform.position = NegateXY(transform.position, isHorizontallyOutOfView, isVerticallyOutOfView);
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