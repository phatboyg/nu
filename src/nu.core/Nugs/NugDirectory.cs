namespace nu.core.Nugs
{
    using FileSystem;

    public interface NugDirectory :
        Directory
    {
        string Version { get; }
        string NugName { get; }
    }
}