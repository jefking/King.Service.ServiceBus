namespace King.Service.ServiceBus.Demo.Models
{
    using System;

    /// <summary>
    /// Example model for Topics
    /// </summary>
    [Serializable]
    public class EmployeeModel
    {
        public Guid Id
        {
            get;
            set;
        }

        public int Count
        {
            get;
            set;
        }

        public bool IsRad
        {
            get;
            set;
        }
    }
}