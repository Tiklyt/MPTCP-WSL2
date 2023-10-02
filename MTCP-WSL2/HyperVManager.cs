using System.Diagnostics;

namespace MTCP_WSL2;

public class HyperVManager
{



    private bool IsNetworkInterfaceSwitched(string interfaceName)
    {
        return RunPowerShell("Get-VMSwitch").Contains(interfaceName);
    }

    public void CreateHyperVSwitch(string interfaceName)
    {
        Task.Run(() =>
        {
            if (!IsNetworkInterfaceSwitched(interfaceName))
            {
                //IT SUCCESSFULLY CREATE AN HYPER-V SWITCHES
                var id = "mtcp_123"; //TO DO 
                var escapedInterfaceName = "Ethernet"; //TO DO
                var command = $"New-VMSwitch -Name '{id}' -NetAdapterName '{escapedInterfaceName}' -AllowManagementOS $true";

                Console.WriteLine(command);
                Console.WriteLine(RunPowerShell(command));
            }
        });
    }

    private string RunPowerShell(string command)
    {
        try
        {
            using var powerShellProcess = new Process();
            powerShellProcess.StartInfo.FileName = "powershell";
            powerShellProcess.StartInfo.Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command {command}";
            powerShellProcess.StartInfo.RedirectStandardOutput = true;
            powerShellProcess.StartInfo.RedirectStandardError = true;
            powerShellProcess.StartInfo.UseShellExecute = false;
            powerShellProcess.StartInfo.CreateNoWindow = true;
            powerShellProcess.Start();

            var output = powerShellProcess.StandardOutput.ReadToEnd();
            var errors = powerShellProcess.StandardError.ReadToEnd();
            powerShellProcess.WaitForExit();
            return output.Length != 0 ? output : errors;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        return "";
    }
    
}