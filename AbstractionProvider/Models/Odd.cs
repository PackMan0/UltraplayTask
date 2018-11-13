using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AbstractionProvider.Models
{
    [XmlRoot(ElementName = "Odd")]
    public class Odd : Base, IEquatable<Odd>
    {
        [XmlAttribute(AttributeName = "Value")]
        public double Value { get; set; }
        
        [XmlAttribute(AttributeName = "SpecialBetValue")]
        public double SpecialBetValue { get; set; }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Odd);
        }

        public bool Equals(Odd other)
        {
            return other != null &&
                   base.Equals(other) &&
                   this.Value == other.Value &&
                   this.SpecialBetValue == other.SpecialBetValue;
        }

        public static bool operator ==(Odd odd1, Odd odd2)
        {
            return EqualityComparer<Odd>.Default.Equals(odd1, odd2);
        }

        public static bool operator !=(Odd odd1, Odd odd2)
        {
            return !(odd1 == odd2);
        }
    }
}