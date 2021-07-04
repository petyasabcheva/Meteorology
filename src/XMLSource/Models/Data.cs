using System;
using System.Xml.Serialization;

namespace XmlSource.Models
{
    [Serializable]
    public class Data
    {
        [XmlElement("temperature")]
        public double Temperature { get; set; }
        [XmlElement("pressure")]
        public double Pressure { get; set; }
    }
}
