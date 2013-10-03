namespace SteamLauncher.Domain
{
    // Note: this may need to support other in/out conversion types in the future
    public interface IIdConverter
    {
        int Convert(string source);
    }
}