
namespace BL.BlApi;

public static class Factory
{
    public static IBl Get { get; } = new BlImplementation.Bl();
}
