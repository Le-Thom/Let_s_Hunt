using Akarisu;
using System.Threading.Tasks;

public static class SaveService
{

    private static readonly ISaveClient Client = new CloudSaveClient();

    public static async void SaveSettings(GameSettingsParameters param)
    {
        await Client.Save("Parameters", param);
    }
    public static async Task<GameSettingsParameters> LoadSettings()
    {
        GameSettingsParameters _param = await Client.Load<GameSettingsParameters>("Parameters");
        return _param;
    }
}
