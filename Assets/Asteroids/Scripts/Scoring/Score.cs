using System;

public static class Score
{
    public delegate void PointsAdded(int points);

    public static event PointsAdded onEarn;
    public static int earned { get; private set; }

    public static void Reset()
    {
        earned = 0;
        Invoke_onEarn(0);
    }

    public static void Earn(int points)
    {
        earned += points;
        Invoke_onEarn(points);
    }

    public static void Tally()
    {
        // The idea is that UI can have a way to 
        // present a breakdown of the tally of 
        // all earnings. For now, let's just 
        // trick the listeners we got zero points
        // so they can display feedback to user.
        Invoke_onEarn(0);
    }

    static void Invoke_onEarn(int points)
    {
        if (onEarn != null)
            onEarn(points);
    }

    public static void LevelCleared(int level)
    {
        Earn(level * 100);
    }
}
