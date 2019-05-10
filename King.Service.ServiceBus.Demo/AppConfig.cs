namespace King.Service.ServiceBus.Demo
{
    // Application Configuration Settings
    public class AppConfig
    {
        public string ConnectionString
        {
            get;
            set;
        }
        
        public string QueueName
        {
            get;
            set;
        }
        
        public string TopicName
        {
            get;
            set;
        }
        
        public string Subscription
        {
            get;
            set;
        }
    }
}