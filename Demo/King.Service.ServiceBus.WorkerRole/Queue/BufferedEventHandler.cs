namespace King.Service.WorkerRole.Queue
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using King.Azure.Data;
    using King.Service.ServiceBus;
    using King.Service.WorkerRole.Models;

    public class BufferedEventHandler : IProcessor<BufferedModel>, IBusEventHandler<BufferedModel>
    {
        public Task<bool> Process(BufferedModel model)
        {
            Trace.TraceInformation("Buffer should process at: {0}, off by {1}", model.ShouldProcessAt, DateTime.UtcNow.Subtract(model.ShouldProcessAt));

            return Task.FromResult<bool>(true);
        }

        public void OnError(string action, Exception ex)
        {
            Trace.TraceError(ex.ToString());
        }
    }
}