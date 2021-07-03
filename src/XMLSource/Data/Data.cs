using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XMLSource.Data
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
