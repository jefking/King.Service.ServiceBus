namespace King.Service.WorkerRole.Models
{
    using System;
    
    [Serializable]
    public class BufferedModel : ExampleModel
    {
        public DateTime ShouldProcessAt
        {
            get;
            set;
        }
    }
}