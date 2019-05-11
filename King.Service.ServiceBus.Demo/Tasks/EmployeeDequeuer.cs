namespace King.Service.ServiceBus.Demo.Tasks
{
    using global::Azure.Data.Wrappers;
    using King.Service.Azure;
    using King.Service.ServiceBus;
    using King.Service.ServiceBus.Demo.Models;
    using King.Service.ServiceBus.Demo.Processors;

    /// <summary>
    /// Dequeue Task, for employee model
    /// </summary>
    public class EmployeeDequeuer : BusEvents<EmployeeModel>
    {
        public EmployeeDequeuer(IBusReciever client)
            : base(client, new EmployeeProcessor())
        {
        }
    }
}