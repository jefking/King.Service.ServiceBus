namespace King.Service.ServiceBus.Demo.Tasks
{
    using global::Azure.Data.Wrappers;
    using King.Service.Azure;
    using King.Service.ServiceBus;
    using King.Service.ServiceBus.Demo.Models;
    using King.Service.ServiceBus.Demo.Processors;

    /// <summary>
    /// Dequeue Task, for company model
    /// </summary>
    public class CompanyDequeuer : BusEvents<CompanyModel>
    {
        public CompanyDequeuer(IBusQueueClient client)
            : base(client, new CompanyProcessor())
        {
        }
    }
}