using System;
using UnityEngine;

[Serializable]
public class FrameCountTrigger
{
    [SerializeField]
    int scheduledFrame;

    const int UNSCHEDULED_FRAME = -1;
    
    FrameCountTrigger() { }

    public static FrameCountTrigger Unscheduled()
    {
        var trigger = new FrameCountTrigger();
        trigger.Unschedule();
        return trigger;
    }

    public void Unschedule()
    {
        scheduledFrame = UNSCHEDULED_FRAME;
    }

    public bool IsScheduled
    {
        get
        {
            return scheduledFrame != UNSCHEDULED_FRAME;
        }
    }

    public bool IsTriggerDue
    {
        get
        {
            return IsScheduled && Time.frameCount >= scheduledFrame;
        }
    }

    public void ScheduleThisFrame()
    {
        ScheduleAfterCurrentFrame(0);
    }

    public void ScheduleNextFrame()
    {
        ScheduleAfterCurrentFrame(1);
    }

    public void ScheduleAfterCurrentFrame(int framesAheadInTime)
    {
        scheduledFrame = Time.frameCount + framesAheadInTime;
    }

    /// <summary>
    /// <para /> Unschedules trigger, if trigger is due. 
    /// <para /> Return true, if unscheduled.
    /// </summary>
    public bool TryUnschedule()
    {
        if (IsTriggerDue)
        {
            Unschedule();
            return true;
        }
        return false;
    }

    /// <summary>
    /// <para /> Unschedules trigger, if trigger was scheduled.
    /// <para /> TryUnscheduleEarly will unschedule the frame, ignoring current frame.
    /// <para /> Return true, if unscheduled.
    /// </summary>
    public bool TryUnscheduleEarly()
    {
        if (IsScheduled)
        {
            Unschedule();
            return true;
        }
        return false;
    }
}