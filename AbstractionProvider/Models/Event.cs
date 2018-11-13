using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AbstractionProvider.Models
{
    [XmlRoot(ElementName = "Event")]
    public class Event : Base, IEquatable<Event>
    {
        public Event()
        {
            this.Matches = new HashSet<Match>();
        }

        [XmlElement(ElementName = "Match")]
        public ICollection<Match> Matches { get; set; }
        
        [XmlAttribute(AttributeName = "IsLive")]
        public bool IsLive { get; set; }
        
        [XmlAttribute(AttributeName = "CategoryID")]
        public int CategoryID { get; set; }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Event);
        }

        public bool Equals(Event other)
        {
            return other != null &&
                   base.Equals(other) &&
                   EqualityComparer<ICollection<Match>>.Default.Equals(this.Matches, other.Matches) &&
                   this.IsLive == other.IsLive &&
                   this.CategoryID == other.CategoryID;
        }

        public static bool operator ==(Event event1, Event event2)
        {
            return EqualityComparer<Event>.Default.Equals(event1, event2);
        }

        public static bool operator !=(Event event1, Event event2)
        {
            return !(event1 == event2);
        }
    }
}