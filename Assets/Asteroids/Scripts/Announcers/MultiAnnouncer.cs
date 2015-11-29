namespace Announcers
{
    using System.Collections.Generic;

    public class MultiAnnouncer : Announcer
    {
        public List<Announcer> announcers;

        public static MultiAnnouncer New(Announcer[] announcers)
        {
            var c = CreateInstance<MultiAnnouncer>();
            c.announcers = new List<Announcer>(announcers);
            return c;
        }

        public override void Announce(string message)
        {
            for (int i = 0; i < announcers.Count; ++i)
                announcers[i].Announce(message);
        }
    }
}