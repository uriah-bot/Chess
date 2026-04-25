using Chess.Model;
using static Chess.Data.Repositories;

namespace Chess.Data
{
    // TODO: ADD
    public class BoardThemeRepo : IBoardThemeRepository
    {
        public Task AddThemeAsync(BoardThemeEntity newTheme)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BoardThemeEntity>> GetUserThemesAsync(UserEntity user)
        {
            throw new NotImplementedException();
        }

        public Task RemoveThemeAsync(BoardThemeEntity newTheme)
        {
            throw new NotImplementedException();
        }
    }
}
