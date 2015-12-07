using System;
using UnityEngine.UI;

public class GameAnnouncer : Announcer
{
    const string title = "A S T E R O I D S";
    const string cleared = "Level Cleared!";
    const string gameover = "GAME OVER";

    const string fmtLevel = "Level {0}"; // "Level 1" etc

    public Announcer strategy;

    public static GameAnnouncer AnnounceTo(Text text)
    {
        return AnnounceTo(TextComponent(text));
    }

    public static GameAnnouncer AnnounceTo(params Announcer[] strategies)
    {
        return AnnounceTo(Many(strategies));
    }

    public static GameAnnouncer AnnounceTo(Announcer strategy)
    {
        var instance = CreateInstance<GameAnnouncer>();
        instance.strategy = strategy;
        return instance;
    }

    public virtual void LevelPlaying()
    {
        Announce("");
    }

    public virtual void Title()
    {
        Announce(title);
    }

    public virtual void LevelCleared()
    {
        Announce(cleared);
    }

    public virtual void LevelStarts(int level)
    {
        Announce(fmtLevel, level);
    }

    public virtual void GameOver()
    {
        Announce(gameover);
    }

    public override void Announce(string message)
    {
        strategy.Announce(message);
    }
}
