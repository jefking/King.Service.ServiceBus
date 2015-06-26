namespace King.Service.WorkerRole.Models
{
    using System;

    [Serializable]
    public class ExampleModel
    {
        public Guid Identifier
        {
            get;
            set;
        }
        public string Action
        {
            get;
            set;
        }
    }
}