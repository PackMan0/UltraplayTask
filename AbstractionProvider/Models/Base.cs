using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AbstractionProvider.Models
{
    public class Base : IEquatable<Base>
    {
        public Guid ID { get; set; }
        
        [XmlAttribute(AttributeName = "ID")]
        public int ExternalID { get; set; }
        
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Base);
        }

        public bool Equals(Base other)
        {
            return other != null &&
                   this.ID.Equals(other.ID) &&
                   this.ExternalID == other.ExternalID &&
                   this.Name == other.Name;
        }

        public static bool operator ==(Base base1, Base base2)
        {
            return EqualityComparer<Base>.Default.Equals(base1, base2);
        }

        public static bool operator !=(Base base1, Base base2)
        {
            return !(base1 == base2);
        }
    }
}
