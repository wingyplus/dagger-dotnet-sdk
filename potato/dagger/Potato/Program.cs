public class Program
{
    public static async Task Main(string[] args)
    {
        var dag = Dagger.SDK.Dagger.Connect();
        // await Dagger.SDK.Mod.Entrypoint.Invoke<Potato.Potato>(dag);
    }
}