using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Model
{
    public static class ModifierFactory
    {
        public static IModifier Create(ModifierType type)
        {
            return type switch
            {
                ModifierType.KingPromotion => new KingPromotion(),
                ModifierType.TimeLimit => new TimeLimit(),
                ModifierType.DoubleMoves => new DoubleMove(),
                //ModifierType.FogOfWar => new FogOfWar(),
                ModifierType.Poof => new Poof(),
                _ => null
            };
        }
    }
}
