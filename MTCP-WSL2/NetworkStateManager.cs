using System.Management;
using System.Net.NetworkInformation;

namespace MTCP_WSL2;

public class NetworkStateManager
{
    private List<String> interfacesName = new List<String>();
    

    public NetworkStateManager()
    {
        
    }


    private void UpdateNetworkInterfaces()
    {
        var updatedInterfacesName = GetAllNetworkInterfaces();
        foreach (var interfaceName in updatedInterfacesName)
        {
            //TODO
        }
    }

    private List<String> GetAllNetworkInterfaces()
    {
        List<String> interfaces = new List<string>();
        ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_NetworkAdapter " +
                                            "WHERE (PNPDeviceID LIKE 'PCI%' OR PNPDeviceID LIKE 'USB%' or PNPDeviceID LIKE 'PCMCIA%') " +
                                            "AND NetConnectionID IS NOT NULL");
        ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
        foreach (ManagementObject adapter in searcher.Get())
        {
            interfaces.Add(adapter["Name"].ToString());
        }
        return interfaces;
    }
}