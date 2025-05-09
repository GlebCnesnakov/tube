using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Contracts;
using Data.Services;

namespace Data
{
    public static class DataExtensions
    {
        public static IServiceCollection AddDataExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            string confString = configuration.GetConnectionString("Data");
            services.AddDbContext<AppContext>(x =>
            {
                x.UseNpgsql(confString);
            });
            services.AddScoped<IPostContract, PostService>();
            services.AddScoped<ICommentService, CommentService>();
            return services;
        }
    }
}
