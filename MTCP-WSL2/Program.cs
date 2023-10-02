using System.Net.NetworkInformation;

namespace MTCP_WSL2;



internal class Program
{
    public static int REFRESH_DELAY = 5000;
    private static int Main()
    {
        HyperVManager hyperVManager = new HyperVManager();
        hyperVManager.CreateHyperVSwitch("Realtek PCIe GbE Family Controller"); //FOR TEST PURPOSE
        var networkStateManager = new NetworkStateManager(REFRESH_DELAY);
        networkStateManager.OnUpdate += OnNetworkUpdate;
        while (true) Thread.Sleep(REFRESH_DELAY);
    }

    private static void OnNetworkUpdate(object? sender, CollectionUpdateEvent e)
    {
        Console.WriteLine(e.Type.ToString());
        Console.WriteLine(e.InterfaceName);
    }
}