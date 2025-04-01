using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorBlast
{
    public class TileColorQueue
    {
        private LinkedList<TileType> tileQueue = new();
        private System.Random rng = new();

        private static readonly List<TileType> basicTileTypes = new()
        {
            TileType.Red,
            TileType.Green,
            TileType.Blue,
            TileType.Yellow
        };

        public TileColorQueue()
        {
            Refill();
        }

        public void Refill()
        {
            tileQueue.Clear();

            for (int i = 0; i < 20; i++)
            {
                TileType type = basicTileTypes[rng.Next(basicTileTypes.Count)];
                tileQueue.AddLast(type);
            }
        }

        public TileType GetNextColor()
        {
            if (tileQueue.Count == 0)
                Refill();

            TileType next = tileQueue.First.Value;
            tileQueue.RemoveFirst();

            Debug.Log("Next color: " + next);

            return next;


        }
    }
}
