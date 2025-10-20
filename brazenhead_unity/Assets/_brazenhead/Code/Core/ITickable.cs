namespace brazenhead.Core
{
    public interface ITickable<T>
    {
        void OnTick(in float deltaTime, in float alpha);
    }
}
