using System;

namespace Gram.Rpg.Client.Core.Threading
{
    public interface IThreadMarshaller
    {
        void PollOnSourceThread(IDisposer disposer, Action onMarshall);
    }
}
