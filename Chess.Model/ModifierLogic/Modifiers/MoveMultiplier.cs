using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace Chess.Model
{
    public class MoveMultiplier : IModifier
    {
        private int Multiplier;

        public MoveMultiplier(int? param)
        {
            if (param != null)
            {
                Multiplier = param.Value;
            }
            else
            {
                Multiplier = AppConstants.MOVE_MULTIPLIER_DEFAULT_MULTIPLIER;
            }
        }

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
