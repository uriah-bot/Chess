using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Model
{
    public enum ModifierType
    {
        None,
        KingPromotion,
        Wormholes,
        Poof,
        FogOfWar,
        DoubleMoves,
        TimeLimit,
    }
    public interface IModifier
    {
        void Apply(Game game);
        void Remove(Game game);
    }
}
