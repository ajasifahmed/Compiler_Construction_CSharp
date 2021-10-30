using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Tokens
    {
       
            public string CV = "";
            public string CP = "";
            public int lineNo = 0;
            public Tokens(string cv, string cp, int line)
            {
                this.CV = cv;
                this.CP = cp;
                this.lineNo = line;
            }


        
    }
}
