namespace ChatCodeChallenge.RabbitMQ
{
    public interface IRabitMQService
    {
        public void SendStockMessage<T>(T message);

        //public void ConsumeStockMessage();
    }
}
