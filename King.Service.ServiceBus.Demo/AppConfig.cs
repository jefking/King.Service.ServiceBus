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
        
        public string AtQueueName
        {
            get;
            set;
        }
        
        public string CompanyQueueName
        {
            get;
            set;
        }
        
        public string TopicName
        {
            get;
            set;
        }
    }
}