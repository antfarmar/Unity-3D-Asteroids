using Announcers;
using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class Announcer : ScriptableObject
{
    public abstract void Announce(string message);
    public virtual void ClearAnnouncements() { Announce(""); }

    public virtual void Announce(string format, object arg0)
    {
        Announce(string.Format(format, arg0));
    }

    public virtual void Announce(string format, object arg0, object arg1)
    {
        Announce(string.Format(format, arg0, arg1));
    }

    public virtual void Announce(string format, object arg0, object arg1, object arg2)
    {
        Announce(string.Format(format, arg0, arg1, arg2));
    }

    public virtual void Announce(string format, params object[] args)
    {
        Announce(string.Format(format, args));
    }

    public virtual void Announce(IFormatProvider provider, string format, params object[] args)
    {
        Announce(string.Format(provider, format, args));
    }

    public static Announcer TextComponent(Text text)
    {
        return TextComponentAnnouncer.New(text);
    }

    public static Announcer Log()
    {
        return LogAnnouncer.New();
    }

    public static Announcer Log(UnityEngine.Object context)
    {
        return ContextualLogAnnouncer.New(context);
    }

    public static Announcer Many(params Announcer[] annoncers)
    {
        return MultiAnnouncer.New(annoncers);
    }
}