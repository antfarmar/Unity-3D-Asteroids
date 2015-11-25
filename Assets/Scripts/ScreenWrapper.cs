using UnityEngine;

// Screenwrap a gameObject OnBecameInvisible() message.
// Hacky, see comments below. Affected by Scene view too.
public class ScreenWrapper : MonoBehaviour
{
    bool isWrappingX = false;
    bool isWrappingY = false;

    // Note that object is considered visible when it needs to be rendered in the scene.
    // It might not be actually visible by any camera, but still need to be rendered for shadows for example.
    // Also, when running in the editor, the scene view cameras will also cause this function to be called.
    void OnBecameInvisible()
    {
        ScreenWrap();
    }


    void OnBecameVisible()
    {
        isWrappingX = false;
        isWrappingY = false;
    }


    void ScreenWrap()
    {
        if(isWrappingX && isWrappingY)
        {
            return;
        }

        var cam = Camera.main;
        var viewportPosition = cam.WorldToViewportPoint(transform.position);
        var newPosition = transform.position;

        if(!isWrappingX && (viewportPosition.x > 1 || viewportPosition.x < 0))
        {
            newPosition.x = -newPosition.x;

            isWrappingX = true;
        }

        if(!isWrappingY && (viewportPosition.y > 1 || viewportPosition.y < 0))
        {
            newPosition.y = -newPosition.y;

            isWrappingY = true;
        }

        transform.position = newPosition;

    }
}