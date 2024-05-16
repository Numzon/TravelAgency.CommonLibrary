namespace TravelAgency.SharedLibrary.Enums;
public static class CognitoGroups
{
    public static readonly string ClientAccount = "Client";
    public static readonly string TravelAgencyAccount = "TravelAgency";
    public static readonly string Employee = "Employee";
    public static readonly string FinancialManager = "FinancialManager";
    public static readonly string FleetManager = "FleetManager";
    public static readonly string HumanResourcesManager = "HRManager";
    public static readonly string TravelManager = "TravelManager";
    public static readonly string SystemAdmin = "SystemAdmin";

    public static IEnumerable<string> GetCognitoGroups()
    {
        yield return ClientAccount;
        yield return TravelAgencyAccount;
        yield return Employee;
        yield return FinancialManager;
        yield return FleetManager;
        yield return HumanResourcesManager;
        yield return TravelManager;
        yield return SystemAdmin;
    }

    public static IEnumerable<string> GetManagersGroups()
    {
        yield return FinancialManager;
        yield return FleetManager;
        yield return HumanResourcesManager;
        yield return TravelManager;
    }

    public static bool IsManager(string group)
    {
        var managers = GetManagersGroups();
        return managers.Contains(group);
    }
}

