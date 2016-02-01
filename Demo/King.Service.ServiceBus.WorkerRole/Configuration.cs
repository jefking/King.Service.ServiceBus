namespace King.Service.WorkerRole
{
    public class Configuration
    {
        public string ScalingQueueName
        {
            get;
            set;
        }
        public string EventsName
        {
            get;
            set;
        }
        public string BufferedEventsName
        {
            get;
            set;
        }
        public string TopicName
        {
            get;
            set;
        }
        public string DynamicQueueName
        {
            get;
            set;
        }
        public string FactoryQueueName
        {
            get;
            set;
        }

        public string Connection
        {
            get;
            set;
        }
    }
}