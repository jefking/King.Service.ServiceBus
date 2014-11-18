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
        public Task<bool> Process(ExampleModel model)
        {
            Trace.TraceInformation("Recieved from queue via {0}: '{1}'", model.Action, model.Identifier);

            return Task.FromResult<bool>(true);
        }
    }
}