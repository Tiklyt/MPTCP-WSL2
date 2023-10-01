using System.Management;

namespace MTCP_WSL2;

public class NetworkStateManager
{
    private readonly HashSet<string> _interfacesName = new();

    private long Delay { get; set; }

    public NetworkStateManager(long refreshDelay)
    {
        Delay = refreshDelay;
        Loop();
    }

    public event EventHandler<CollectionUpdateEvent> OnUpdate = null!;


    private void Loop()
    {
        Task.Run(async () =>
        {
            while (true)
            {
                UpdateNetworkInterfaces();
                await Task.Delay(5000);
            }
        });
    }


    private void UpdateNetworkInterfaces()
    {
        var updatedInterfacesName = GetAllNetworkInterfaces();
        foreach (var interfaceName in updatedInterfacesName.Where(interfaceName => !_interfacesName.Contains(interfaceName)))
        {
            _interfacesName.Add(interfaceName);
            OnUpdate?.Invoke(this, new CollectionUpdateEvent(ModificationType.Add, interfaceName));
        }
        foreach (var interfaceName in _interfacesName.Where(interfaceName => !updatedInterfacesName.Contains(interfaceName)))
        {
            _interfacesName.Remove(interfaceName);
            OnUpdate?.Invoke(this, new CollectionUpdateEvent(ModificationType.Del, interfaceName));
        }
    }
    
    private List<string> GetAllNetworkInterfaces()
    {
        var interfaces = new List<string>();
        var query = new ObjectQuery("SELECT * FROM Win32_NetworkAdapter " +
                                    "WHERE (PNPDeviceID LIKE 'PCI%' " +
                                    "OR PNPDeviceID LIKE 'USB%' or PNPDeviceID LIKE 'PCMCIA%') " +
                                    "AND NetConnectionID IS NOT NULL");
        var searcher = new ManagementObjectSearcher(query);
        foreach (ManagementObject adapter in searcher.Get()) interfaces.Add(adapter["Name"].ToString());
        return interfaces;
    }
}