namespace Chess.Model
{
    public enum EndReason
    {
        Checkmate,
        Stalemate,
        InsufficientMaterial,
        ThreefoldRepetition,
        FiftyMoveRule,
        KingPromotion,
        NotEnoughPoofPieces,
        TimeRanOut,
        Resignation
    }
}
