namespace brazenhead.Core
{
    public interface IListener<T> where T : IEvent
    {
        void OnEvent(in T param);
    }
}
