using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicAPI.Services
{
    public class MyAppSettings
    {
        public const string SectionName = "Authorization";
        public string JsonAppKey { get; set; }
        public string XmlAppKey { get; set; }
    }
}
