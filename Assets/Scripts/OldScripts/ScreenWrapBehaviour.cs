using UnityEngine;
using System.Collections;


// http://gamedevelopment.tutsplus.com/articles/create-an-asteroids-like-screen-wrapping-effect-with-unity--gamedev-15055
// https://github.com/tutsplus/screen-wrapping-unity/blob/master/src/Assets/Scripts/ScreenWrapBehaviour.cs



public class ScreenWrapBehaviour : MonoBehaviour
{
    // Whether to use advancedWrapping or not
    public bool advancedWrapping = true;

    Renderer[] renderers;

    bool isWrappingX = false;
    bool isWrappingY = false;

    // We use ghosts in advanced wrapping to create a nice wrapping illusion
    Transform[] ghosts = new Transform[8];

    float screenWidth;
    float screenHeight;

    void Start()
    {
        // Fetch all the renderers that display ship graphics.
        // In the demo we only have one mesh for the ship and thus
        // only one renderer.
        // We could have a complicated ship, made out of several meshes
        // and this would fetch all the renderers.
        // We use the renderer(s) so we can check if the ship is
        // visible or not.
        renderers = GetComponentsInChildren<Renderer>();

        var cam = Camera.main;

        // We need the screen width in world units, relative to the ship.
        // To do this, we transform viewport coordinates of the screen edges to 
        // world coordinates that lie on on the same Z-axis as the player.
        //
        // Viewport coordinates are screen coordinates that go from 0 to 1, ie
        // x = 0 is the coordinate of the left edge of the screen, while,
        // x = 1 is the coordinate of the right edge of the screen.
        // Similarly,
        // y = 0 is the bottom screen edge coordinate, while
        // y = 1 is the top screen edge coordinate.
        //
        // Which gives us this:
        // (0, 0) is the bottom left corner, to
        // (1, 1) is the top right corner.
        var screenBottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));
        var screenTopRight = cam.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));

        // The width is then equal to difference between the rightmost and leftmost x-coordinates
        screenWidth = screenTopRight.x - screenBottomLeft.x;
        // The height, similar to above is the difference between the topmost and the bottom yycoordinates
        screenHeight = screenTopRight.y - screenBottomLeft.y;

        if(advancedWrapping)
        {
            // Create a eights ship for the illusion of a nice wrapping effect
            CreateGhostShips();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(advancedWrapping)
        {
            AdvancedScreenWrap();
        }
        else
        {
            ScreenWrap();
        }
    }

    void ScreenWrap()
    {
        // If all parts of the object are invisible we wrap it around
        foreach(var renderer in renderers)
        {
            if(renderer.isVisible)
            {
                isWrappingX = false;
                isWrappingY = false;
                return;
            }
        }

        // If we're already wrapping on both axes there is nothing to do
        if(isWrappingX && isWrappingY)
        {
            return;
        }

        var cam = Camera.main;
        var newPosition = transform.position;

        // We need to check whether the object went off screen along the horizontal axis (X),
        // or along the vertical axis (Y).
        //
        // The easiest way to do that is to convert the ships world position to
        // viewport position and then check.
        //
        // Remember that viewport coordinates go from 0 to 1?
        // To be exact they are in 0-1 range for everything on screen.
        // If something is off screen, it is going to have
        // either a negative coordinate (less than 0), or
        // a coordinate greater than 1
        //
        // So, we get the ships viewport position
        var viewportPosition = cam.WorldToViewportPoint(transform.position);


        // Wrap it is off screen along the x-axis and is not being wrapped already
        if(!isWrappingX && (viewportPosition.x > 1 || viewportPosition.x < 0))
        {
            // The scene is laid out like a mirror:
            // Center of the screen is position the camera's position - (0, 0),
            // Everything to the right is positive,
            // Everything to the left is negative;
            // Everything in the top half is positive
            // Everything in the bottom half is negative
            // So we simply swap the current position with it's negative one
            // -- if it was (15, 0), it becomes (-15, 0);
            // -- if it was (-20, 0), it becomes (20, 0).
            newPosition.x = -newPosition.x;

            // If you had a camera that isn't at X = 0 and Y = 0,
            // you would have to use this instead
            // newPosition.x = Camera.main.transform.position - newPosition.x;

            isWrappingX = true;
        }

        // Wrap it is off screen along the y-axis and is not being wrapped already
        if(!isWrappingY && (viewportPosition.y > 1 || viewportPosition.y < 0))
        {
            newPosition.y = -newPosition.y;

            isWrappingY = true;
        }

        //Apply new position
        transform.position = newPosition;
    }

    void AdvancedScreenWrap()
    {
        // Move to separate function
        var isVisible = false;
        foreach(var renderer in renderers)
        {
            if(renderer.isVisible)
            {
                isVisible = true;
                break;
            }
        }

        if(!isVisible)
        {
            SwapShips();
        }
    }

    void CreateGhostShips()
    {
        for(int i = 0; i < 8; i++)
        {
            // Ghost ships should be a copy of the main ship
            ghosts[i] = Instantiate(transform, Vector3.zero, Quaternion.identity) as Transform;

            // But without the screen wrapping component
            DestroyImmediate(ghosts[i].GetComponent<ScreenWrapBehaviour>());
        }

        PositionGhostShips();
    }

    void PositionGhostShips()
    {
        // All ghost positions will be relative to the ships (this) transform,
        // so let's star with that.
        var ghostPosition = transform.position;

        // We're positioning the ghosts clockwise behind the edges of the screen.
        // Let's start with the far right.
        ghostPosition.x = transform.position.x + screenWidth;
        ghostPosition.y = transform.position.y;
        ghosts[0].position = ghostPosition;

        // Bottom-right
        ghostPosition.x = transform.position.x + screenWidth;
        ghostPosition.y = transform.position.y - screenHeight;
        ghosts[1].position = ghostPosition;

        // Bottom
        ghostPosition.x = transform.position.x;
        ghostPosition.y = transform.position.y - screenHeight;
        ghosts[2].position = ghostPosition;

        // Bottom-left
        ghostPosition.x = transform.position.x - screenWidth;
        ghostPosition.y = transform.position.y - screenHeight;
        ghosts[3].position = ghostPosition;

        // Left
        ghostPosition.x = transform.position.x - screenWidth;
        ghostPosition.y = transform.position.y;
        ghosts[4].position = ghostPosition;

        // Top-left
        ghostPosition.x = transform.position.x - screenWidth;
        ghostPosition.y = transform.position.y + screenHeight;
        ghosts[5].position = ghostPosition;

        // Top
        ghostPosition.x = transform.position.x;
        ghostPosition.y = transform.position.y + screenHeight;
        ghosts[6].position = ghostPosition;

        // Top-right
        ghostPosition.x = transform.position.x + screenWidth;
        ghostPosition.y = transform.position.y + screenHeight;
        ghosts[7].position = ghostPosition;

        // All ghost ships should have the same rotation as the main ship
        for(int i = 0; i < 8; i++)
        {
            ghosts[i].rotation = transform.rotation;
        }
    }

    void SwapShips()
    {
        // When the main ship is off screen we want to teleport it to
        // the position of the ghost that is currently on screen.
        // A ghost is on screen if its position is
        // between -screenWidth and +screenWidth along the X axis
        // and
        // between -screenHeight and +screenHeight along the Y axis,
        // so we'll look for that.

        foreach(var ghost in ghosts)
        {
            if(ghost.position.x < screenWidth && ghost.position.x > -screenWidth &&
                ghost.position.y < screenHeight && ghost.position.y > -screenHeight)
            {
                transform.position = ghost.position;

                // We found the one we needed,
                // no need to iterate through the loop anymore
                // so we break
                break;
            }
        }

        // Reposition the ghost ships because we changed the player ship position
        PositionGhostShips();
    }

    void OnGUI()
    {
        if(GUI.Button(new Rect(20, 20, 160, 48), "Simple Wrapping"))
        {
            SwitchToSimpleWrapping();
        }

        if(GUI.Button(new Rect(200, 20, 160, 48), "Advanced Wrapping"))
        {
            SwitchToAdvancedWrapping();
        }
    }

    void SwitchToSimpleWrapping()
    {
        advancedWrapping = false;

        foreach(var ghost in ghosts)
        {
            Destroy(ghost.gameObject);
        }
    }

    void SwitchToAdvancedWrapping()
    {
        advancedWrapping = true;

        CreateGhostShips();
    }
}