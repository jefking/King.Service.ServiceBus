namespace King.Service.ServiceBus.Demo.Processors
{
    using global::Azure.Data.Wrappers;
    using King.Service.ServiceBus.Demo.Models;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// Company Processor, occurs post dequeue action
    /// </summary>
    public class CompanyProcessor : IProcessor<CompanyModel>
    {
        public Task<bool> Process(CompanyModel data)
        {
            Trace.TraceInformation("Take action on company data: '{0}/{1}'", data.Name, data.Id);

            return Task.FromResult<bool>(true);
        }
    }
}