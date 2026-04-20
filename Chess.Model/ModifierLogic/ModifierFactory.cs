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
                //ModifierType.Wormholes => new Wormholes(),
                _ => null
            };
        }
    }
}
