using System;
using System.Collections.Generic;
using System.Text;

namespace Risk
{
    public class Deployment
    {
        public int Armies { get; set; }
        public CountryInfo From { get; set; }
        public CountryInfo To { get; set; }
        public bool IsRequired { get; set; }
    }
}
