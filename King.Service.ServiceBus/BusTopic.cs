namespace King.Service.ServiceBus
{
    using System;
    using System.Threading.Tasks;
    using Azure.Data;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;

    /// <summary>
    /// Bus Topic
    /// </summary>
    public class BusTopic : IAzureStorage
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
        /// <param name="name">Topic name</param>
        /// <param name="connectionString">Connection String</param>
        public BusTopic(string name, string connectionString)
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

        #region Properties
        /// <summary>
        /// Topic Name
        /// </summary>
        public virtual string Name
        {
            get
            {
                return this.name;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Create If Not Exists
        /// </summary>
        /// <returns>Created</returns>
        public virtual async Task<bool> CreateIfNotExists()
        {
            var exists = await this.manager.TopicExistsAsync(this.name);
            if (!exists)
            {
                var td = new TopicDescription(this.name)
                {
                    EnableExpress = true,
                };

                await this.manager.CreateTopicAsync(td);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <returns>Task</returns>
        public virtual async Task Delete()
        {
            var exists = await this.manager.TopicExistsAsync(this.name);
            if (exists)
            {
                await this.manager.DeleteTopicAsync(this.name);
            }
        }
        #endregion
    }
}