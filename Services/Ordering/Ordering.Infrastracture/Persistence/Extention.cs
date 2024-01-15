using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastracture.Persistence
{
    public static class Extention
    {
        public static IServiceCollection AddSQL(this IServiceCollection services, IConfiguration configuration)
        {
            var options = configuration.GetConnectionString("DataBaseConnectionString");

            services.AddDbContext<Context>(ctx =>
                ctx.UseSqlServer(options));

            return services;
        }
    }
}
