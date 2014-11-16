namespace King.Service.ServiceBus.Queue
{
    using King.Azure.Data;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// Processes dequeued data of type ExampleModel
    /// </summary>
    public class ExampleProcessor : IProcessor<ExampleModel>
    {
        public Task<bool> Process(ExampleModel data)
        {
            Trace.TraceInformation("Action: {0} Id: {1}", data.Action, data.Identifier);

            return Task.FromResult<bool>(true);
        }
    }
}