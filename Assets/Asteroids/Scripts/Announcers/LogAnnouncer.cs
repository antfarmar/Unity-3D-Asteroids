namespace Announcers
{
    using UnityEngine;

    public class LogAnnouncer : Announcer
    {
        public bool ignoreEmptyAnnouncements = true;

        public static LogAnnouncer New()
        {
            return CreateInstance<LogAnnouncer>();
        }

        public override void Announce(string message)
        {
            if (ignoreEmptyAnnouncements && string.IsNullOrEmpty(message))
                return;
            else
                Debug.Log("ANNOUCEMENT: " + message);
        }
    }
}