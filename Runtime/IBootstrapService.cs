namespace UExtension.Bootstrap
{
    public interface IBootstrapService
    {
        void Bootstrap();
        int Priority { get; }
    }
}