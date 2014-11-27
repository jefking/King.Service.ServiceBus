namespace King.Service.ServiceBus
{
    using Microsoft.ServiceBus;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Initialize Topic
    /// </summary>
    public class InitializeTopic : InitializeTask
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
        /// <param name="name"></param>
        /// <param name="connectionString">Connection String</param>
        public InitializeTopic(string name, string connectionString)
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
        /// <returns></returns>
        public override async Task RunAsync()
        {
            //var created = false;
            var exists = await manager.TopicExistsAsync(name);
            if (!exists)
            {
                await manager.CreateTopicAsync(name);
                //created = true;
            }

            //return created;
        }
        #endregion
    }
}