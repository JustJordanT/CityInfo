namespace CityInfo.API.Services;

public class CloudMailService : IMailService
{
    private readonly string _mailTo = string.Empty;
    private readonly string _mailFrom = string.Empty;

    public CloudMailService(IConfiguration configuration)
    {
        _mailTo = configuration["mailSettings:mailToAddress"];
        _mailFrom = configuration["mailSettings:mailFromAddress"];
        if (_mailFrom == null || _mailTo == null)
        {
            throw new ArgumentNullException("misconfiguration:mailAddresses");
        }
    }
    
    public void SendMail(string subject, string message)
    {
        // SendMail - outbound to console
        Console.WriteLine($"Mail from {_mailFrom} to {_mailTo} with {nameof(CloudMailService)}");
        Console.WriteLine($"Subject: {subject}");
        Console.WriteLine($"Message: {message}");
        Console.WriteLine();
    }
}