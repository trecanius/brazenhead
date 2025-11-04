namespace brazenhead
{
    public interface IListener { }

    public interface IListener<T> : IListener where T : IEvent
    {
        void OnEvent(in T param);
    }
}
