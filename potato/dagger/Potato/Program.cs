namespace Potato;

public static class Program
{
    public static async Task Main(string[] args)
    {
        await Entrypoint.Invoke(args);
    } 
}