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
        
        [XmlAttribute(AttributeName = "xsd", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsd { get; set; }
        
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
        
        [XmlAttribute(AttributeName = "CreateDate")]
        public DateTime CreateDate { get; set; }
    }

}

