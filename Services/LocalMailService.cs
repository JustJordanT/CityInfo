namespace CityInfo.API.Services;

public class LocalMailService : IMailService
{
    private readonly string _mailTo = string.Empty;
    private readonly string _mailFrom = string.Empty;

    public LocalMailService(IConfiguration configuration)
    {
        _mailTo = configuration["mailSettings:mailToAddress"];
        _mailFrom = configuration["mailSettings:mailFromAddress"];
    }

    public void SendMail(string subject, string message)
    {
        // SendMail - outbound to console
        Console.WriteLine($"Mail from {_mailFrom} to {_mailTo} with {nameof(LocalMailService)}");
        Console.WriteLine($"Subject: {subject}");
        Console.WriteLine($"Message: {message}");
        Console.WriteLine();
    } 
}