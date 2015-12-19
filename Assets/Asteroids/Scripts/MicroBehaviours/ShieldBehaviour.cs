using UnityEngine;
using System.Collections;

public class ShieldBehaviour : MonoBehaviour
{
    public Color colorBase;
    public Color colorStart;
    public Color colorEnd;
    public float duration = 10f;

    float timeOn;
    Renderer rend;

    void Start() { rend = GetComponent<Renderer>(); deactivateShield(); }
    void OnEnable() { timeOn = 0f; }
    void Update() { AnimateShieldColor(); }

    public bool isShieldActive() { return gameObject.activeSelf; }
    public void deactivateShield() { gameObject.SetActive(false); }
    public void activateShield(float duration)
    {
        this.duration = duration;
        gameObject.SetActive(true);
        OnEnable();
    }

    void AnimateShieldColor()
    {
        float blinkLerp = 0.5f * (Mathf.Sin(timeOn * timeOn) + 1f);
        Color startToEnd = Color.Lerp(colorStart, colorEnd, timeOn / duration);
        rend.material.color = Color.Lerp(colorBase, startToEnd, blinkLerp);
        timeOn += Time.deltaTime;
        if (timeOn >= duration) timeOn = 0f; // for debugging in editor
    }
}
