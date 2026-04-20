using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Model
{
    public class DoubleMove : IModifier
    {
        public List<ModifierType> Conflicts => null;

        public void Apply(Game game)
        {
            throw new NotImplementedException();
        }

        public void Remove(Game game)
        {
            throw new NotImplementedException();
        }
    }
}
