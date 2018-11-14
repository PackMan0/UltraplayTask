using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AbstractionProvider.Models
{
    [XmlRoot(ElementName = "Bet")]
    public class Bet : Base
    {
        public Bet()
        {
            this.Odds = new List<Odd>();
        }

        [XmlElement(ElementName = "Odd")]
        public List<Odd> Odds { get; set; }
        
        [XmlAttribute(AttributeName = "IsLive")]
        public bool IsLive { get; set; }
        
        public int MatchExternalID { get; set; }

        public Guid MatchID { get; set; }
    }
}