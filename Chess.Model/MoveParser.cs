using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Model
{
    public class MoveParser
    {
        public Move ParseMove(string move)
        {
            return new NormalMove(new Position(1,1), new Position(1,1));
        }
    }
}
