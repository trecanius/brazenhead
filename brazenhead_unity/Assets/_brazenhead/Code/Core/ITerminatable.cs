using UnityEngine;

namespace brazenhead.Core
{
    public interface ITerminatable
    {
        bool Terminate(out Awaitable awaitable);
    }
}
