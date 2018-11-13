using System;
using System.Xml.Serialization;
using AbstractionProvider.Models;

namespace ExternalDataService
{
    [XmlRoot(ElementName = "XmlSports")]
    public class XmlSportsContainer
    {
        [XmlElement(ElementName = "Sport")]
        public Sport Sport { get; set; }
        
        [XmlAttribute(AttributeName = "CreateDate")]
        public DateTime CreateDate { get; set; }
    }

}

