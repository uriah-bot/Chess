using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.Data.Repositories;

namespace Chess.Data
{
    public static class RepositoryDependencyInjection 
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IUserRepository, UserRepo>();
            services.AddSingleton<IGameRepository, GameRepo>();

            return services;
        }
    }
}
