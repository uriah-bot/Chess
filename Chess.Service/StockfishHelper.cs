using Chess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Service
{
    public class StockfishHelper
    {
        public Move ApplyStockfishMove(Game game, string stockfishOutput)
        {
            return new NormalMove(new Position(0,0), new Position(0, 0));
        }
    }
}
