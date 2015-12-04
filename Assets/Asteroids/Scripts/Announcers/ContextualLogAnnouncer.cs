namespace Announcers
{
    using UnityEngine;

    public class ContextualLogAnnouncer : Announcer
    {
        public bool ignoreEmptyAnnouncements = true;

        public Object context;

        public static ContextualLogAnnouncer New(Object context)
        {
            var instance = CreateInstance<ContextualLogAnnouncer>();
            instance.context = context;
            return instance;
        }

        public override void Announce(string message)
        {
            if (ignoreEmptyAnnouncements && string.IsNullOrEmpty(message))
                return;
            else
                Debug.Log("ANNOUNCEMENT: " + message, context);
        }
    }
}