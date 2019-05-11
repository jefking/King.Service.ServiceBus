namespace King.Service.ServiceBus.Demo.Tasks
{
    using global::Azure.Data.Wrappers;
    using King.Service.Azure;
    using King.Service.ServiceBus;
    using King.Service.ServiceBus.Demo.Models;
    using King.Service.ServiceBus.Demo.Processors;

    /// <summary>
    /// Dequeue Task, for at model
    /// </summary>
    public class AtDequeuer : BufferedReciever<AtModel>
    {
        public AtDequeuer(IBusQueueClient client)
            : base(client, new AtProcessor())
        {
        }
    }
}