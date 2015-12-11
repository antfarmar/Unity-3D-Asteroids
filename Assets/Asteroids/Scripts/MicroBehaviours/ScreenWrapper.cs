using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ScreenWrapper : MonoBehaviour
{
    [SerializeField]
    [HideInInspector]
    public UnityEvent beforeWrap;

    Renderer objectRenderer;
    Bounds objectBounds;
    Rect worldRect;

    bool allowedToWrapHorizontally = true;
    bool allowedToWrapVertically = true;
    bool debug = true;

    float wrapTimeout = 0.5f;

    int screenWidth;
    int screenHeight;

    void OnEnable()
    {
        objectRenderer = GetComponent<Renderer>();
        allowedToWrapHorizontally = true;
        allowedToWrapVertically = true;
        SetCurrentScreenSize();
        ComputeWorldRectSize();
    }

    void Update()
    {
        if (debug) DrawObjectBoundsInSceneView();

        if (ScreenSizeChanged())
        {
            ComputeWorldRectSize();
            SetCurrentScreenSize();
        }

        ScreenWrap();
    }

    void ComputeWorldRectSize()
    {
        Vector2 worldMin = GetWorldPointFromViewport(new Vector3(0f, 0f, 0f));
        Vector2 worldMax = GetWorldPointFromViewport(new Vector3(1f, 1f, 0f));
        //worldRect = new Rect(tl.x, tl.y, br.x * 2, tl.y * 2);
        worldRect = Rect.MinMaxRect(worldMin.x, worldMin.y, worldMax.x, worldMax.y);
    }

    void ScreenWrap()
    {
        objectBounds = objectRenderer.bounds;

        Vector3 newPosition = transform.position;

        bool isOutOfBoundsRight = objectBounds.min.x > worldRect.xMax;
        bool isOutOfBoundsLeft = objectBounds.max.x < worldRect.xMin;
        bool isOutOfBoundsTop = objectBounds.min.y > worldRect.yMax;
        bool isOutOfBoundsBottom = objectBounds.max.y < worldRect.yMin;

        bool needToWrapHorizontally = isOutOfBoundsRight || isOutOfBoundsLeft;
        bool needToWrapVertically = isOutOfBoundsTop || isOutOfBoundsBottom;

        if (needToWrapHorizontally && allowedToWrapHorizontally)
        {
            newPosition.x = isOutOfBoundsRight ? WrapRightToLeft() : WrapLeftToRight();
            allowedToWrapHorizontally = false;
            Invoke("ReAllowHorizontalWrap", wrapTimeout);
        }


        if (needToWrapVertically && allowedToWrapVertically)
        {
            newPosition.y = isOutOfBoundsTop ? WrapTopToBottom() : WrapBottomToTop();
            allowedToWrapVertically = false;
            Invoke("ReAllowVerticalWrap", wrapTimeout);
        }


        bool didWrap = !newPosition.Equals(transform.position);
        if (didWrap)
        {
            beforeWrap.Invoke();
            transform.position = newPosition;
        }
    }

    float WrapRightToLeft() { return worldRect.xMin - objectBounds.extents.x; }
    float WrapLeftToRight() { return worldRect.xMax + objectBounds.extents.x; }
    float WrapTopToBottom() { return worldRect.yMin - objectBounds.extents.y; }
    float WrapBottomToTop() { return worldRect.yMax + objectBounds.extents.y; }

    void ReAllowHorizontalWrap() { allowedToWrapHorizontally = true; }
    void ReAllowVerticalWrap() { allowedToWrapVertically = true; }

    Vector2 GetWorldPointFromViewport(Vector3 viewportPoint) { return Camera.main.ViewportToWorldPoint(viewportPoint); }

    bool ScreenSizeChanged() { return (screenWidth != Screen.width || screenHeight != Screen.height); }
    void SetCurrentScreenSize() { screenWidth = Screen.width; screenHeight = Screen.height; }

    void DrawObjectBoundsInSceneView()
    {
        Vector3 lowerLeft = new Vector3(objectBounds.min.x, objectBounds.min.y, 0);
        Vector3 upperLeft = new Vector3(objectBounds.min.x, objectBounds.max.y, 0);
        Vector3 lowerRight = new Vector3(objectBounds.max.x, objectBounds.min.y, 0);
        Vector3 upperRight = new Vector3(objectBounds.max.x, objectBounds.max.y, 0);

        Color sceneViewDisplayColor = new Color(1.0f, 0.0f, 1.0f, 0.5f);
        Debug.DrawLine(lowerLeft, upperLeft, sceneViewDisplayColor);
        Debug.DrawLine(upperLeft, upperRight, sceneViewDisplayColor);
        Debug.DrawLine(upperRight, lowerRight, sceneViewDisplayColor);
        Debug.DrawLine(lowerRight, lowerLeft, sceneViewDisplayColor);

        Debug.DrawLine(worldRect.min, worldRect.max, sceneViewDisplayColor);
    }

}
