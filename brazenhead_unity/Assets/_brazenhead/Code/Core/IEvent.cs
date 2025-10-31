namespace brazenhead.Core
{
    public interface IEvent
    {
        /// <summary>
        /// Will only ever fire once, and will instantly invoke handlers on listeners added after the event has been fired
        /// </summary>
        public bool IsInitEvent { get; }
    }
}
