// Copyright 2007-2012 Chris Patterson, Dru Sellers, Travis Smith, et. al.
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
namespace MassTransit.Distributor.Configuration
{
    using System.Collections.Generic;
    using System.Linq;
    using BusConfigurators;
    using Configurators;
    using MassTransit.Builders;

    /// <summary>
    /// Decorates subscriptions added through this interface to the subscription
    /// bus service with the distributor components
    /// </summary>
    public class WorkerConfiguratorImpl :
        WorkerConfigurator,
        BusBuilderConfigurator
    {
        readonly IList<WorkerBuilderConfigurator> _configurators;

        public WorkerConfiguratorImpl()
        {
            _configurators = new List<WorkerBuilderConfigurator>();
        }

        public IEnumerable<ValidationResult> Validate()
        {
            return _configurators.SelectMany(configurator => configurator.Validate(),
                (configurator, result) => result.WithParentKey("Worker"));
        }

        public BusBuilder Configure(BusBuilder builder)
        {
            var configurator = new WorkerBusServiceConfigurator(_configurators);

            builder.AddBusServiceConfigurator(configurator);

            return builder;
        }

        public void AddConfigurator(WorkerBuilderConfigurator configurator)
        {
            _configurators.Add(configurator);
        }
    }
}