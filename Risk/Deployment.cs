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

        public Deployment()
        { }

        public Deployment(int armies, CountryInfo from, CountryInfo to)
        {
            Armies = armies;
            From = from;
            To = to;
        }
    }
}
