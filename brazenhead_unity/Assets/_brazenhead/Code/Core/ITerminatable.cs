using UnityEngine;

namespace brazenhead
{
    public interface ITerminatable
    {
        bool Terminate(out Awaitable awaitable);
    }
}
