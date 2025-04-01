
using System;

namespace ColorBlast
{
    public interface IGameService : IService
    {
        event Action GameInited;
        event Action SessionStarted;
        event Action SessionEnded;

        MoveManager MoveManager { get; }
        void StartSession();
        void EndSession();
    }
}
