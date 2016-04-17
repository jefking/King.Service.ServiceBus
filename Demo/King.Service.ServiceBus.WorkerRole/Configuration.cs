namespace King.Service.WorkerRole
{
    public class Configuration
    {
        public string EventsName;
        public string BufferedQueueName;
        public string BufferedTopicName;
        public string BufferedSubscriptionName;
        public string TopicName;
        public string HubName;
        public string FactoryQueueName;
        public string Connection;
        public string ScalingQueueName;
        public string ShardsQueueName;
        public byte ShardsCount;
        public string TopicSubscriptionName;
        public string TopicSubscriptionSqlFilter;
    }
}