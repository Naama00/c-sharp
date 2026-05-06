
namespace BL.BlApi;

public static class Factory
{
    // public static IBl Get { get; } = new BlImplementation.Bl();
    private static readonly IBl _instance = new BlImplementation.Bl();
    public static IBl Get() => _instance;
}
