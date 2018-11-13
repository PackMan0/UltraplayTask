using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AbstractionProvider.Models
{
    [XmlRoot(ElementName = "Bet")]
    public class Bet : Base, IEquatable<Bet>
    {
        public Bet()
        {
            this.Odds = new HashSet<Odd>();
        }

        [XmlElement(ElementName = "Odd")]
        public ICollection<Odd> Odds { get; set; }
        
        [XmlAttribute(AttributeName = "IsLive")]
        public bool IsLive { get; set; }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Bet);
        }

        public bool Equals(Bet other)
        {
            return other != null &&
                   base.Equals(other) &&
                   EqualityComparer<ICollection<Odd>>.Default.Equals(this.Odds, other.Odds) &&
                   this.IsLive == other.IsLive;
        }

        public static bool operator ==(Bet bet1, Bet bet2)
        {
            return EqualityComparer<Bet>.Default.Equals(bet1, bet2);
        }

        public static bool operator !=(Bet bet1, Bet bet2)
        {
            return !(bet1 == bet2);
        }
    }
}