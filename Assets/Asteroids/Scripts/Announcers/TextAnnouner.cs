using UnityEngine.UI;

public class TextComponentAnnouncer : Announcer
{
    public Text text;

    public static TextComponentAnnouncer New(Text text)
    {
        var instance = CreateInstance<TextComponentAnnouncer>();
        instance.text = text;
        return instance;
    }

    public override void Announce(string message)
    {
        if (text)
            text.text = message;
    }
}