using MassTransit;
using MassTransit.BusConfigurators;
using MassTransit.Log4NetIntegration.Logging;
using System;


namespace OpcEsb.Configuration
{
   public class BusInitializer
    {
        public static IServiceBus CreateBus(string queueName, Action<ServiceBusConfigurator> moreInitialization)
        {
            Log4NetLogger.Use();
            var bus = ServiceBusFactory.New(x =>
            {
                x.UseRabbitMq();
                //x.UseJsonSerializer();
                x.ReceiveFrom("rabbitmq://localhost/OpcEsb_" + queueName);
                moreInitialization(x);
                
            });

            return bus;
        }
    }
}
