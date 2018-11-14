using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AbstractionProvider.Models
{
    [XmlRoot(ElementName = "Odd")]
    public class Odd : Base
    {
        [XmlAttribute(AttributeName = "Value")]
        public double Value { get; set; }
        
        [XmlAttribute(AttributeName = "SpecialBetValue")]
        public double SpecialBetValue { get; set; }

        public int BetExtarnalID { get; set; }

        public Guid BetID { get; set; }
    }
}