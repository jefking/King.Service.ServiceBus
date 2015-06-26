namespace King.Service.WorkerRole.Models
{
    using System;
    
    public class BufferedModel : ExampleModel
    {
        public DateTime ShouldProcessAt
        {
            get;
            set;
        }
    }
}
