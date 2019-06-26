using System;
using System.Collections.Generic;
using System.Text;

namespace Risk
{
    public class ValidationParameter<TestObject>
    {
        public TestObject Object { get; set; }
        public Player Player { get; set; }
        public CountryInfo[] Countries { get; set; }
        public Deployment PreviousDeployment { get; set; }
        public List<Card> Cards { get; set; }
        public int ArmiesToDistribute { get; set; }
        public string Response { get; set; }
    }
}
