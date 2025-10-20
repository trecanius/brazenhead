namespace brazenhead.Core
{
    public interface IListener<T>
    {
        void OnEvent(in T param);
    }
}
