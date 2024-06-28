public class Program
{
    public static async Task Main(string[] args)
    {
        var dag = Dagger.SDK.Dagger.Connect();
        // NOTE: We're in converting from reflection to source generator state. The module is not working
        // at the moment.
        // await Dagger.SDK.Mod.Entrypoint.Invoke<Potato.Potato>(dag);
    }
}