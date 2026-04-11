using Chess.Model;
using static Chess.Data.Repositories;

namespace Chess.Data
{
    // TODO: ADD
    public class PieceThemeRepo : IPieceThemeRepository
    {
        public Task AddThemeAsync(PieceThemeEntity newTheme)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PieceThemeEntity>> GetUserThemesAsync(UserEntity user)
        {
            throw new NotImplementedException();
        }
    }
}
