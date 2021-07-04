using System;
using System.Xml.Serialization;

namespace XmlSource.Models
{
    [Serializable]
    [XmlRoot("response",
        IsNullable = false)]
    public class ResultToReturn
    {
        [XmlElement("success")]
        public bool Success { get; set; }
        [XmlElement("error")]
        public int Error { get; set; }

        [XmlElement("data")]
        public Data? Data { get; set; }
    }
}
