using VaultSharp;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.Token;

namespace TravelAgency.SharedLibrary.Vault;
public sealed class VaultFacadeBuilder
{
    private string? _token;
    private string? _host;
    private int? _port;
    private string _protocol = "http";

    public VaultFacade Build()
    {
        if (ValidateIfBuilderIsConfigured() is false)
        {
            throw new InvalidOperationException("Builder hasn't been configured correctly");
        }

        IAuthMethodInfo authMethod = new TokenAuthMethodInfo(_token);
        var vaultClientSettings = new VaultClientSettings($"{_protocol}://{_host}:{_port}", authMethod);
        var vaultClient = new VaultClient(vaultClientSettings);

        return new VaultFacade(vaultClient);
    }

    public VaultFacadeBuilder SetToken(string? token)
    {
        if (token is null)
        {
            throw new ArgumentNullException($"{nameof(token)}");
        }

        _token = token;

        return this;
    }

    public VaultFacadeBuilder SetHost(string? host)
    {
        if (host is null)
        {
            throw new ArgumentNullException($"{nameof(host)}");
        }

        _host = host;

        return this;
    }

    public VaultFacadeBuilder SetPort(string? port)
    {
        if (port is null)
        {
            throw new ArgumentNullException($"{nameof(port)}");
        }

        if (int.TryParse(port, out var portAsInt) is false)
        {
            throw new InvalidOperationException("Given port cannot be converted to integer");
        }

        return SetPort(portAsInt);
    }

    public VaultFacadeBuilder SetPort(int port)
    {
        if (port is 0)
        {
            throw new ArgumentException($"{nameof(port)} cannot be 0");
        }

        _port = port;

        return this;
    }

    public VaultFacadeBuilder SetSSL(bool ssl)
    {
        _protocol = ssl ? "https" : "http"; 

        return this;
    }

    private bool ValidateIfBuilderIsConfigured()
    {
        if (_token is null)
        {
            return false;
        }

        if (_host is null)
        {
            return false;
        }

        if (_port is null)
        {
            return false;
        }

        return true;
    }
}
