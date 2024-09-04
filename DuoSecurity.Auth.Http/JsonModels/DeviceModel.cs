using System.Collections.Generic;

namespace DuoSecurity.Auth.Http.JsonModels;

internal class DeviceModel
{
    private ICollection<string> _capabilities = new List<string>();

    public ICollection<string> Capabilities
    {
        get => _capabilities;
        set => _capabilities = value ?? new List<string>();
    }

    public string Device { get; set; }

    public string Display_Name { get; set; }

    public string Name { get; set; }

    public string Sms_Nextcode { get; set; }

    public string Number { get; set; }

    public string Type { get; set; }
}