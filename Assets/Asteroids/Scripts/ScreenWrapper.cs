using System.Collections;
using UnityEngine;

// Screenwrap a gameObject OnBecameInvisible() message.
// Hacky, see comments below. Affected by Scene view too.
public class ScreenWrapper : MonoBehaviour
{

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
        Vector3 viewportPosition = Vector3.zero;
        viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

        Vector3 newPosition = transform.position;

        if(viewportPosition.x < 0 || viewportPosition.x > 1)
            newPosition.x = -newPosition.x;

        if(viewportPosition.y < 0 || viewportPosition.y > 1)
            newPosition.y = -newPosition.y;

        // Deactivate the gameObject temporarily when we wrap/teleport.
        // Prevents funky behaviour. (eg. particle systems based on distance emission)
        //gameObject.SetActive(false);
        transform.position = newPosition;
        //gameObject.SetActive(true);
    }
}