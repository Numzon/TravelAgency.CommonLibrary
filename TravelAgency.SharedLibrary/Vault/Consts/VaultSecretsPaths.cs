namespace TravelAgency.SharedLibrary.Vault.Consts;
public static class VaultSecretsPaths
{
    public const string RabbitMQ = "/rabbit-mq";
    public const string Cognito = "/aws/cognito";
    public const string SimpleNotificationService = "/aws/simplenotificationservice";
    public const string SimpleEmailService = "/aws/simpleemailservice";
    public const string AgencyServiceDatabase = "/connectionstrings/agencyservicedatabase";
    public const string UserServiceDatabase = "/connectionstrings/userservicedatabase";
    public const string EmployeeServiceDatabase = "/databases/employeservicedatabase";
}