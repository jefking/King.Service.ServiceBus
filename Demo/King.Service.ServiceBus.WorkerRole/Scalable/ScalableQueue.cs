namespace King.Service.WorkerRole.Scalable
{
    using King.Azure.Data;
    using King.Service.Data;
    using King.Service.ServiceBus;
    using King.Service.ServiceBus.Queue;
    using System.Collections.Generic;

    public class ScalableQueue : QueueAutoScaler<Configuration>
    {
        public ScalableQueue(IQueueCount count, Configuration config)
            : base(count, 1, config, 1, 15, 1)
        {
        }

        public override IEnumerable<IScalable> ScaleUnit(Configuration config)
        {
            yield return new AdaptiveRunner(new BusDequeue<ExampleModel>(new BusQueueReciever(config.ScalingQueueName, config.Connection), new ExampleProcessor()));
        }
    }
}