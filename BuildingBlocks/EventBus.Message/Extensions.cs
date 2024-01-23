using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Message
{
    public static class Extensions
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services)
        {
            #region Add Config MassTransit RabbitMQ

            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ctx, conf) =>
                {
                    conf.Host("localhost", "/", c =>
                    {
                        c.Username("guest");
                        c.Password("guest");
                    });
                });
            });
            
            #endregion

            return services;
        }
    }
}
