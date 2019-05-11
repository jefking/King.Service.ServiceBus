namespace King.Service.ServiceBus.Demo.Models
{
    using System;

    /// <summary>
    /// Example model for Topics
    /// </summary>
    [Serializable]
    public class AtModel
    {
        public Guid Id
        {
            get;
            set;
        }
        public DateTime At
        {
            get;
            set;
        }
    }
}