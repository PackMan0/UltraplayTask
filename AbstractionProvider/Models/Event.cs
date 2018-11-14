using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace AbstractionProvider.Models
{
    [XmlRoot(ElementName = "Event")]
    public class Event : Base
    {
        public Event()
        {
            this.Matches = new List<Match>();
        }

        [XmlElement(ElementName = "Match")]
        public List<Match> Matches { get; set; }
        
        [XmlAttribute(AttributeName = "IsLive")]
        public bool IsLive { get; set; }
        
        [XmlAttribute(AttributeName = "CategoryID")]
        public int CategoryID { get; set; }
        
        public int SportExternaID { get; set; }
        
        public Guid SportID { get; set; }

        public Sport Sport { get; set; }
    }
}