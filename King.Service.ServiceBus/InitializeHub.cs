namespace King.Service.ServiceBus
{
    using Microsoft.ServiceBus;
    using System;
    using System.Threading.Tasks;

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
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Hub Name</param>
        /// <param name="connectionString">Connection String</param>
        public InitializeHub(string name, string connectionString)
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
        }
        #endregion

        #region Methods
        /// <summary>
        /// Run Async
        /// </summary>
        /// <returns>Task</returns>
        public override async Task RunAsync()
        {
            await this.manager.CreateEventHubIfNotExistsAsync(name);
        }
        #endregion
    }
}