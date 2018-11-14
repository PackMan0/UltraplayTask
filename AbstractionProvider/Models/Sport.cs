using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AbstractionProvider.Models
{
    [XmlRoot(ElementName = "Sport")]
    public class Sport : Base
    {
        public Sport()
        {
            this.Events = new List<Event>();
        }

        [XmlElement(ElementName = "Event")]
        public List<Event> Events { get; set; }
    }
}