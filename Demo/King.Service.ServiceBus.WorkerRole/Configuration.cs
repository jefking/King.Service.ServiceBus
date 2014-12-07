namespace King.Service.WorkerRole
{
    public class Configuration
    {
        public string PollingName
        {
            get;
            set;
        }
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
        public string Connection
        {
            get;
            set;
        }
    }
}