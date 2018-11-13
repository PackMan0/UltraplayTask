using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AbstractionProvider.Models
{
    [XmlRoot(ElementName = "Sport")]
    public class Sport : Base, IEquatable<Sport>
    {
        public Sport()
        {
            this.Events = new List<Event>();
        }

        [XmlElement(ElementName = "Event")]
        public List<Event> Events { get; set; }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Sport);
        }

        public bool Equals(Sport other)
        {
            return other != null &&
                   base.Equals(other) &&
                   EqualityComparer<List<Event>>.Default.Equals(this.Events, other.Events);
        }

        public static bool operator ==(Sport sport1, Sport sport2)
        {
            return EqualityComparer<Sport>.Default.Equals(sport1, sport2);
        }

        public static bool operator !=(Sport sport1, Sport sport2)
        {
            return !(sport1 == sport2);
        }
    }
}