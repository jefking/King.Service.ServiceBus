namespace King.Service.ServiceBus.Demo.Tasks
{
    using global::Azure.Data.Wrappers;
    using King.Service.Azure;
    using King.Service.ServiceBus.Demo.Models;
    using King.Service.ServiceBus.Demo.Processors;

    /// <summary>
    /// Dequeue Task, for company model
    /// </summary>
    public class CompanyDequeuer : StorageDequeue<CompanyModel>
    {
        public CompanyDequeuer(string name, string connection)
            : base(new StorageQueue(name, connection), new CompanyProcessor())
        {
        }
    }
}