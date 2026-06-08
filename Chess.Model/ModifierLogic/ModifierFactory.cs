namespace Chess.Model
{
    public static class ModifierFactory
    {
        public static IModifier Create(ModifierType type, string selectedParam = null)
        {
            _ = int.TryParse(selectedParam, out var param);

            return type switch
            {
                ModifierType.KingPromotion => new KingPromotion(),
                ModifierType.TimeLimit => new TimeLimit(param),
                ModifierType.MoveMultiplier => new MoveMultiplier(param),
                ModifierType.ZombieChess => new ZombieChess(),
                ModifierType.Poof => new Poof(param),
                ModifierType.Wormholes => new Wormholes(param),
                _ => null
            };
        }
    }
}
