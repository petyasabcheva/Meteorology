using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Extensions.Localization;

namespace XMLSource.Data
{
    [Serializable]
    [XmlRootAttribute("response",
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
