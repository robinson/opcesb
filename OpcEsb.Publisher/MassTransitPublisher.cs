// Copyright 2016 lth
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.

namespace OpcEsb.Publisher
{
    using MassTransit;
    using MassTransit.Log4NetIntegration.Logging;
    using OpcEsb.Contracts;
    using System;

    public class MassTransitPublisher : IDisposable
    {
        IBusControl _bus;
        public MassTransitPublisher() {
            Log4NetLogger.Use();
            _bus = Bus.Factory.CreateUsingRabbitMq(x =>
              x.Host(new Uri("rabbitmq://localhost/"), h => { }));
            var busHandle = _bus.Start();
        }     

        public void Publish(string jsonMessage)
        {
            var message = new MessageActivity()
            {
                Message = jsonMessage,
                Happened = DateTime.Now
            };
            _bus.Publish<Activity>(message);
            
        }
        public void Dispose()
        {
            _bus.Stop();
        }

        
    }
}
