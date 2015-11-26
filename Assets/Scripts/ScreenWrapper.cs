using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// Screenwrap a gameObject OnBecameInvisible() message.
// Hacky, see comments below. Affected by Scene view too.
public class ScreenWrapper : MonoBehaviour
{
    [SerializeField]
    [HideInInspector]    
    public UnityEvent beforeWrap;

    bool IsVisible = true;


    void Update()
    {
        if(IsVisible)
            return;
        else
        {
            ScreenWrap();
            IsVisible = true;
        }
    }


    // Note that object is considered visible when it needs to be rendered in the scene.
    // It might not be actually visible by any camera, but still need to be rendered for shadows for example.
    // Also, when running in the editor, the scene view cameras will also cause this function to be called.
    IEnumerator OnBecameInvisible()
    {
        IsVisible = false;
        yield return new WaitForSeconds(1f); // give object time to wrap before another call?
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
            // Some components have a dependency on transform.position for effects.
            // To avoid maintaining code regarding specific components here, events 
            // delegate responsibility to components who impose the dependency.
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