namespace brazenhead
{
    public interface ITickable<T>
    {
        void OnTick(in float deltaTime);
    }
}
