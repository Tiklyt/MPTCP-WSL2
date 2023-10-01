namespace MTCP_WSL2;

public class CollectionUpdateEvent : EventArgs
{
    public CollectionUpdateEvent(ModificationType type, string interfaceName)
    {
        Type = type;
        InterfaceName = interfaceName;
    }

    public string InterfaceName { get; }
    public ModificationType Type { get; }
}