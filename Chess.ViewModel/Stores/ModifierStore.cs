using Chess.Model;

namespace Chess.ViewModel.Stores
{
    public interface IModifierStore
    {
        ActiveModifier ActivelyInspectedModifier { get; set; }
    }

    public class ModifierStore : IModifierStore
    {
        public ActiveModifier ActivelyInspectedModifier { get; set; }
    }
}
