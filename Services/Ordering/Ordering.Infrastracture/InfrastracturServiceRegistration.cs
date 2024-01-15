using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Contracts.Infrastracture;
using Ordering.Application.Contracts.Persistence;
using Ordering.Infrastracture.Mail;
using Ordering.Infrastracture.Persistence;
using Ordering.Infrastracture.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastracture
{
    public static class InfrastracturServiceRegistration
    {
        public static IServiceCollection AddInfrastractureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSQL(configuration);

            services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IEmailService, MailService>();

            return services;
        }
    }
}
