using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XMLSource.Data
{
    [XmlRoot("response")]
    [Serializable()]
    public class ResultToReturn
    {
        public int Success { get; set; }
        public int Error { get; set; }
        public double Temperature { get; set; }
        public double Pressure { get; set; }

    }
}
