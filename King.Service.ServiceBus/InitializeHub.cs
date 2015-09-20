namespace King.Service.ServiceBus
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;

    /// <summary>
    /// Initialize Hub
    /// </summary>
    public class InitializeHub : InitializeTask
    {
        #region Members
        /// <summary>
        /// Namespace Manager
        /// </summary>
        protected readonly NamespaceManager manager = null;

        /// <summary>
        /// Name
        /// </summary>
        protected readonly string name = null;

        /// <summary>
        /// partition count
        /// </summary>
        protected readonly int partitionCount = 16;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Hub Name</param>
        /// <param name="connectionString">Connection String</param>
        /// <param name="partitionCount"></param>
        public InitializeHub(string name, string connectionString, int partitionCount = 16)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("name");
            }
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("connectionString");
            }

            this.name = name;
            this.manager = NamespaceManager.CreateFromConnectionString(connectionString);
            this.partitionCount = partitionCount < 8 ? 8 : partitionCount > 32 ? 32 : partitionCount;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Number of partitons to create the Event Hub With
        /// </summary>
        public int PartitionCount
        {
            get
            {
                return this.partitionCount;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Run Async
        /// </summary>
        /// <returns>Task</returns>
        public override async Task RunAsync()
        {
            var description = new EventHubDescription(name)
            {
                PartitionCount = this.partitionCount,
            };

            await this.manager.CreateEventHubIfNotExistsAsync(description);
        }
        #endregion
    }
}