using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AbstractionProvider.Models
{
    [XmlRoot(ElementName = "Match")]
    public class Match : Base
    {
        public Match()
        {
            this.Bets = new List<Bet>();
        }

        [XmlAttribute(AttributeName = "StartDate")]
        public DateTime StartDate { get; set; }
        
        [XmlAttribute(AttributeName = "MatchType")]
        public string MatchType { get; set; }
        
        [XmlElement(ElementName = "Bet")]
        public List<Bet> Bets { get; set; }
        
        public int EventExtarnalID { get; set; }

        public Guid EventID { get; set; }
    }
}