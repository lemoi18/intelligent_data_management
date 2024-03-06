namespace Site
{
    public class KafkaConfiguration
    {
        public string Brokers { get; set; }
        public string Topic { get; set; }
        public string Key { get; set; }
        public string ConsumerGroup { get; set; }
    }
}