using Chess.Model;

namespace Chess.ViewModel.Stores
{
    public interface IModifierStore
    {
        ActiveModifier ActiveModifier { get; set; }
    }

    public class ModifierStore : IModifierStore
    {
        public ActiveModifier ActiveModifier { get; set; }
    }
}
