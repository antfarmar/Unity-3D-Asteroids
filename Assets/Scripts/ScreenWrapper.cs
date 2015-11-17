using UnityEngine;
using System.Collections;

// Screenwrap a gameObject OnBecameInvisible() message.
// Hacky, see comments below. Affected by Scene view too.
public class ScreenWrapper : MonoBehaviour
{

    // Note that object is considered visible when it needs to be rendered in the scene.
    // It might not be actually visible by any camera, but still need to be rendered for shadows for example.
    // Also, when running in the editor, the scene view cameras will also cause this function to be called.
    public void OnBecameInvisible()
    {

        if (gameObject.activeSelf) // && gameObject != null && Camera.main.isActiveAndEnabled)
        {
            Vector3 viewportPosition = Vector3.zero;
            if (Camera.main != null)
                viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

            Vector3 newPosition = transform.position;

            if (viewportPosition.x < 0 || viewportPosition.x > 1)
                newPosition.x = -newPosition.x;

            if (viewportPosition.y < 0 || viewportPosition.y > 1)
                newPosition.y = -newPosition.y;

            // Deactivate the gameObject temporarily when we wrap/teleport.
            // Prevents funky behaviour. (eg. particle systems based on distance emission)
            //gameObject.SetActive(false);
            transform.position = newPosition;
            //gameObject.SetActive(true);
        }
    }
}