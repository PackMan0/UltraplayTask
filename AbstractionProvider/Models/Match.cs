using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AbstractionProvider.Models
{
    [XmlRoot(ElementName = "Match")]
    public class Match : Base, IEquatable<Match>
    {
        public Match()
        {
            this.Bets = new HashSet<Bet>();
        }

        [XmlAttribute(AttributeName = "StartDate")]
        public DateTime StartDate { get; set; }
        
        [XmlAttribute(AttributeName = "MatchType")]
        public string MatchType { get; set; }
        
        [XmlElement(ElementName = "Bet")]
        public ICollection<Bet> Bets { get; set; }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Match);
        }

        public bool Equals(Match other)
        {
            return other != null &&
                   base.Equals(other) &&
                   this.StartDate == other.StartDate &&
                   this.MatchType == other.MatchType &&
                   EqualityComparer<ICollection<Bet>>.Default.Equals(this.Bets, other.Bets);
        }

        public static bool operator ==(Match match1, Match match2)
        {
            return EqualityComparer<Match>.Default.Equals(match1, match2);
        }

        public static bool operator !=(Match match1, Match match2)
        {
            return !(match1 == match2);
        }
    }
}