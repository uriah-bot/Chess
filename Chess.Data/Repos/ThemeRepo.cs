using Chess.Model;
using static Chess.Data.Repositories;

namespace Chess.Data
{
    // TODO: ADD
    public class ThemeRepo : IThemeRepository
    {
        public Task AddThemeAsync(ThemeEntity newTheme)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ThemeEntity>> GetUserThemesAsync(UserEntity user)
        {
            throw new NotImplementedException();
        }
    }
}
