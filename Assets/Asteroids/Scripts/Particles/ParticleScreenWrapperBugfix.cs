using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
sealed class ParticleScreenWrapperBugfix : MonoBehaviour
{
    [SerializeField]
    [HideInInspector]
    ParticleSystem particles;

    [SerializeField]
    [HideInInspector]
    bool storedEmissionValue;

    [SerializeField]
    [HideInInspector]
    FrameCountTrigger trigger = FrameCountTrigger.Unscheduled();

    void OnEnable()
    {
        var wrapper = GetComponentInParent<ScreenWrapper>();
        if (wrapper)
            wrapper.beforeWrap.AddListener(DisableEmissionUntilNextFrame_Bugfix_Hack);

        particles = GetComponent<ParticleSystem>();
    }

    void OnDisable()
    {
        var wrapper = GetComponentInParent<ScreenWrapper>();
        if (wrapper)
            wrapper.beforeWrap.RemoveListener(DisableEmissionUntilNextFrame_Bugfix_Hack);

        if (trigger.TryUnscheduleEarly())
            RestoreEmission_Bugfix_Hack();
    }

    void DisableEmissionUntilNextFrame_Bugfix_Hack()
    {
        if (!trigger.IsScheduled)
        {
            trigger.ScheduleNextFrame();
            StoreEmission_Bugfix_Hack();
            particles.enableEmission = false;
        }
    }

    void Update()
    {
        if (trigger.TryUnschedule())
            RestoreEmission_Bugfix_Hack();
    }

    void StoreEmission_Bugfix_Hack()
    {
        // This is not super ideal, but I can't think of a good solution;
        // particles.enableEmission could be changed by another script
        // later in this frame causing a race condition because there's
        // no write restriction to enableEmission. Sorry, dear developer!
        storedEmissionValue = particles.enableEmission;
    }

    void RestoreEmission_Bugfix_Hack()
    {
        // This is not super ideal, but I can't think of a good solution;
        // particles.enableEmission could be changed by another script
        // earlier in this frame causing a race condition because there's
        // no write restriction to enableEmission. Sorry, dear developer!
        particles.enableEmission = storedEmissionValue;
    }
}