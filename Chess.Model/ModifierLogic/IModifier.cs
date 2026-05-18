namespace Chess.Model
{
    public enum ModifierType
    {
        Empty,
        KingPromotion,
        Wormholes,
        Poof,
        FogOfWar,
        MoveMultiplier,
        TimeLimit,
    }
    public interface IModifier
    {
        void Apply(Game game);
        void Remove(Game game);

        List<ModifierType> Conflicts { get; }
    }
}
