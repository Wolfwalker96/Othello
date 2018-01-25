using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello
{
    class Quote
    {
        public String Winning;
        public String Losing;
        public String Pass;
        public String Drew;

        public String GetQuote(int score) {
            if (score <= -5)
            {
                return Losing;
            }
            else if (score >= 5)
            {
                return Winning;
            }
            else {
                return Drew;
            }
        }
    }
}
