using Microsoft.AspNetCore;
using Project_Management;

internal class Program
{
    private static void Main(string[] args)
    {
        BuildWebHost(args).Run();
    }
    public static IWebHost BuildWebHost(string[] args)
    {
        return WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>()
            .UseIISIntegration()
            .Build();
    }
}