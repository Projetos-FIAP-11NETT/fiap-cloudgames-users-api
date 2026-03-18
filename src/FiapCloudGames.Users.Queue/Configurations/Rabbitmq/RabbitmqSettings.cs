namespace FiapCloudGames.Queue.Configurations.Rabbitmq;

public class RabbitmqSettings
{
    public string Address { get; set; }
    public int Port { get; set; }
    public string VirtualHost { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}