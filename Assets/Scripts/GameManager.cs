using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject m_ShipPrefab;
    public GameObject m_Asteroid_Big_Prefab;
    public GameObject m_Asteroid_Small_Prefab;
    public int m_AsteroidCount = 6;
    //List<GameObject> m_AsteroidList = new List<GameObject>();


    // For objects added to the scene, the Awake and OnEnable functions for all scripts will be called before Start, Update, etc are called for any of them.
    // Cannot be enforced when an object is instantiated during gameplay.

    // This function is always called before any Start functions and also just after a prefab is instantiated.
    // If a GameObject is inactive during start up Awake is not called until it is made active, or a function in any script attached to it is called.
    void Awake()
    {

    }

    // Only called if the Object is active. This function is called just after the object is enabled.
    // This happens when a MonoBehaviour instance is created, such as when a level is loaded or a GameObject with the script component is instantiated.
    void OnEnable()
    {

    }


    // Start is called before the first frame update only if the script instance is enabled.
    // Use this for initialization.
    void Start()
    {
        // Spawn the ship.
        Instantiate(m_ShipPrefab);

        // Spawn some asteroids.
        Transform asteroidHolder = new GameObject("AsteroidHolder").transform;
        GameObject asteroid;

        for (int i = 0; i < m_AsteroidCount; i++)
        {
            if (i < m_AsteroidCount / 2)
                asteroid = Instantiate(m_Asteroid_Big_Prefab);
            else
                asteroid = Instantiate(m_Asteroid_Small_Prefab);

            asteroid.transform.SetParent(asteroidHolder);
        }
    }


    // Update is called once per frame. It is the main workhorse function for frame updates.
    void Update()
    {

    }


    // FixedUpdate is often called more frequently than Update.
    // It can be called multiple times per frame, if the frame rate is low and it may not be called between frames at all if the frame rate is high.
    // All physics calculations and updates occur immediately after FixedUpdate.
    // When applying movement calculations inside FixedUpdate, you do not need to multiply your values by Time.deltaTime.
    // This is because FixedUpdate is called on a reliable timer, independent of the frame rate.
    void FixedUpdate()
    {

    }


    // LateUpdate is called once per frame, after Update has finished.
    // Any calculations that are performed in Update will have completed when LateUpdate begins.
    // A common use for LateUpdate would be a following third-person camera.
    // If you make your character move and turn inside Update, you can perform all camera movement and rotation calculations in LateUpdate.
    // This will ensure that the character has moved completely before the camera tracks its position.
    void LateUpdate()
    {

    }

    // This is called at the end of the frame where the pause is detected, effectively between the normal frame updates.
    // One extra frame will be issued after OnApplicationPause is called to allow the game to show graphics that indicate the paused state.
    void OnApplicationPause()
    {

    }


    // WHEN QUITTING
    // These functions get called on all the active objects in your scene:

    // This function is called on all game objects before the application is quit.
    // In the editor it is called when the user stops playmode. In the web player it is called when the web view is closed.
    void OnApplicationQuit()
    {

    }

    // This function is called after all frame updates for the last frame of the object’s existence.
    // The object might be destroyed in response to Object.Destroy or at the closure of a scene).
    void OnDestroy()
    {

    }

} // end class

/*
/// DEBUG ///////////////////////////////////////////////////////////////////
Debug.Log()         // Logs message to the Unity Console.
Debug.Assert()	    // Assert the condition.
Debug.DrawLine()    // Draws a line between specified start and end points.
Debug.DrawRay()     // Draws a line from start to start + dir in world coordinates.
Debug.Break()       // Pauses the editor.


/// GIZMOS.XXX
Implement OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn.
This allows you to quickly pick important objects in your scene.
Note that OnDrawGizmos will use a mouse position that is relative to the Scene View.
This function does not get called if the component is collapsed in the inspector. Use OnDrawGizmosSelected to draw gizmos when the game object is selected.

OnDrawGizmos() { }
OnDrawGizmosSelected() { }

GIZMO STATIC FUNCTIONS
DrawCube	Draw a solid box with center and size.
DrawFrustum	Draw a camera frustum using the currently set Gizmos.matrix for it's location and rotation.
DrawGUITexture	Draw a texture in the scene.
DrawIcon	Draw an icon at a position in the scene view.
DrawLine	Draws a line starting at from towards to.
DrawMesh	Draws a mesh.
DrawRay	Draws a ray starting at from to from + direction.
DrawSphere	Draws a solid sphere with center and radius.
DrawWireCube	Draw a wireframe box with center and size.
DrawWireMesh	Draws a wireframe mesh.
DrawWireSphere	Draws a wireframe sphere with center and radius.


/// INPUT ///////////////////////////////////////////////////////////////////

/// MOUSE
Called every frame while the mouse is over the GUIElement or Collider.
These functions are not called on objects that belong to Ignore Raycast layer.
These functions are called on Colliders marked as Trigger if and only if Physics.queriesHitTriggers is true.
OnMouseXXX can be a co-routine, simply use the yield statement in the function. This event is sent to all scripts attached to the Collider or GUIElement.

void OnMouseEnter() { }         // called when the mouse enters the GUIElement or Collider.
void OnMouseOver() { }          // called every frame while the mouse is over the GUIElement or Collider.
void OnMouseDrag() { }          // called when the user has clicked on a GUIElement or Collider and is still holding down the mouse.
void OnMouseExit() { }          // called when the mouse is not any longer over the GUIElement or Collider.
void OnMouseDown() { }          // called when the user has pressed the mouse button while over the GUIElement or Collider.
void OnMouseUp() { }            // called when the user has released the mouse button
void OnMouseUpAsButton() { }    // called when the mouse is released over the SAME GUIElement or Collider as it was pressed.
   
   
/// PHYSICS     ///////////////////////////////////////////////////////////////////
 OnCollisionEnter is passed the Collision class and not a Collider.
 The Collision class contains information about contact points, impact velocity etc.
 If you don't use collisionInfo in the function, leave out the collisionInfo parameter as this avoids unnecessary calculations.
 Collision events are only sent if one of the colliders also has a non-kinematic rigidbody attached.
 Collision events will be sent to disabled MonoBehaviours, to allow enabling Behaviours in response to collisions.
 
void OnTriggerXXX() { }         // Enter|Exit|Stay
void OnCollisionXXX() { }


/// COROUTINES ///////////////////////////////////////////////////////////////////

Normal coroutine updates are run after the Update function returns. A coroutine is a function that can suspend its execution (yield) until the given YieldInstruction finishes. Different uses of Coroutines:

yield                       The coroutine will continue after all Update functions have been called on the next frame.
yield WaitForSeconds        Continue after a specified time delay, after all Update functions have been called for the frame
yield WaitForFixedUpdate    Continue after all FixedUpdate has been called on all scripts
yield WaitForEndOfFrame     Continue after all FixedUpdate has been called on all scripts
yield WWW                   Continue after a WWW download has completed.
yield StartCoroutine        Chains the coroutine, and will wait for the MyFunc coroutine to complete first.


/// RENDERING ///////////////////////////////////////////////////////////////////

OnPreCull: Called before the camera culls the scene. Culling determines which objects are visible to the camera. OnPreCull is called just before culling takes place.
OnBecameVisible/OnBecameInvisible: Called when an object becomes visible/invisible to any camera.
OnWillRenderObject: Called once for each camera if the object is visible.
OnPreRender: Called before the camera starts rendering the scene.
OnRenderObject: Called after all regular scene rendering is done. You can use GL class or Graphics.DrawMeshNow to draw custom geometry at this point.
OnPostRender: Called after a camera finishes rendering the scene.
OnRenderImage: Called after scene rendering is complete to allow postprocessing of the screen image.
OnGUI: Called multiple times per frame in response to GUI events. The Layout and Repaint events are processed first, followed by a Layout and keyboard/mouse event for each input event.
OnDrawGizmos: Used for drawing Gizmos in the scene view for visualisation purposes.

*/

