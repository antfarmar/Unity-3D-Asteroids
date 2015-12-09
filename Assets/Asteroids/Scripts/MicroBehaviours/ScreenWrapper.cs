using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ScreenWrapper : MonoBehaviour
{
    [SerializeField]
    [HideInInspector]
    public UnityEvent beforeWrap;

    public float wrapTimeout = 0.5f;

    Renderer objectRenderer;
    Bounds objectBounds;
    Rect worldRect;

    Color sceneViewDisplayColor = new Color(1.0f, 0.0f, 1.0f, 0.5f);
    bool debug = true;

    bool allowedToWrapHorizontally = true;
    bool allowedToWrapVertically = true;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (debug) DrawObjectBoundsInSceneView();

        // Compute world rect boundds.
        // Only need to do this if screen size changes? How to check?
        Vector2 worldMin = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
        Vector2 worldMax = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));
        Debug.DrawLine(worldMin, worldMax, sceneViewDisplayColor);

        //worldRect = new Rect(tl.x, tl.y, br.x * 2, tl.y * 2);
        worldRect = Rect.MinMaxRect(worldMin.x, worldMin.y, worldMax.x, worldMax.y);
        objectBounds = objectRenderer.bounds;

        Vector3 newPosition = transform.position;

        bool wrapRightToLeft = objectBounds.min.x > worldRect.xMax;
        bool wrapLeftToRight = objectBounds.max.x < worldRect.xMin;
        bool wrapTopToBottom = objectBounds.min.y > worldRect.yMax;
        bool wrapBottomToTop = objectBounds.max.y < worldRect.yMin;

        // Check horizontally out-of-view.

        if (allowedToWrapHorizontally)
        {
            if (wrapRightToLeft)
            {
                newPosition.x = worldRect.xMin - objectBounds.extents.x;
                allowedToWrapHorizontally = false;
            }
            else if (wrapLeftToRight)
            {
                newPosition.x = worldRect.xMax + objectBounds.extents.x;
                allowedToWrapHorizontally = false;
            }

            if (!allowedToWrapHorizontally)
                Invoke("AllowHorizontalWrap", wrapTimeout);
        }

        // Check vertically out-of-view.
        if (allowedToWrapVertically)
        {
            if (wrapTopToBottom)
            {
                newPosition.y = worldRect.yMin - objectBounds.extents.y;
                allowedToWrapVertically = false;
            }
            else if (wrapBottomToTop)
            {
                newPosition.y = worldRect.yMax + objectBounds.extents.y;
                allowedToWrapVertically = false;
            }

            if (!allowedToWrapVertically)
                Invoke("AllowVerticalWrap", wrapTimeout);
        }

        // if position actually changed...should go here.
        if (!newPosition.Equals(transform.position))
        {
            beforeWrap.Invoke();
            transform.position = newPosition;
        }

    }

    void AllowHorizontalWrap() { allowedToWrapHorizontally = true; }
    void AllowVerticalWrap() { allowedToWrapVertically = true; }

    void DrawObjectBoundsInSceneView()
    {
        Vector3 lowerLeft = new Vector3(objectBounds.min.x, objectBounds.min.y, 0);
        Vector3 upperLeft = new Vector3(objectBounds.min.x, objectBounds.max.y, 0);
        Vector3 lowerRight = new Vector3(objectBounds.max.x, objectBounds.min.y, 0);
        Vector3 upperRight = new Vector3(objectBounds.max.x, objectBounds.max.y, 0);

        Debug.DrawLine(lowerLeft, upperLeft, sceneViewDisplayColor);
        Debug.DrawLine(upperLeft, upperRight, sceneViewDisplayColor);
        Debug.DrawLine(upperRight, lowerRight, sceneViewDisplayColor);
        Debug.DrawLine(lowerRight, lowerLeft, sceneViewDisplayColor);
    }

}


/* PREVIOUS IMPLEMENTATION

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

*/
