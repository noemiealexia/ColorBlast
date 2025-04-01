using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorBlast
{
    public class MoveManager
    {
        public int MaxMoves { get; private set; }
        public int CurrentMoves { get; private set; }

        public void Initialize(int maxMoves)
        {
            MaxMoves = maxMoves;
            CurrentMoves = 0;
        }

        public void RegisterMove()
        {
            CurrentMoves++;
        }

        public bool HasMovesLeft()
        {
            return CurrentMoves < MaxMoves;
        }

        public bool OutOfMoves()
        {
            return CurrentMoves >= MaxMoves;
        }
    }
}
