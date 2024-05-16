namespace TravelAgency.SharedLibrary.Models;
public sealed class RabbitMqSettingsDto
{
    public required string Host { get; set; }
    public required string Port { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
};