using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ColorBlast
{
    /// <summary>
    /// Helper class for Board mechanics and calculations
    /// </summary>
    public class BoardHelper 
    {
        private IPoolService mPoolService;
        private Slot[,] mBoardMap;

        private TileColorQueue colorQueue = new TileColorQueue();

        private int mWidth;
        private int mHeight;

        public BoardHelper(Slot[,] boardMap, IPoolService poolService) 
        {
            mPoolService = poolService;
            mBoardMap    = boardMap;

            mHeight = mBoardMap.GetLength(0);
            mWidth  = mBoardMap.GetLength(1);
        }

        public Tile GetRandomTile(float specialChance)
        {
            bool isSpecialTile = UnityEngine.Random.Range(0.0f, 1.0f) < specialChance;

            Tile tile = null;

            if (isSpecialTile)
            {
                tile = GetRandomSpecialTile();
            }
            else
            {
                // Use color queue to get the next tile color
                TileType nextColor = colorQueue.GetNextColor();
                tile = GetTileByType(nextColor);
            }

            return tile;
        }

        private Tile GetRandomSpecialTile() 
        {
            // Since we only have 1 special tile, just return it. 
            var poolType = PoolType.TileBomb;
            Tile tile = mPoolService.Get<Tile>(poolType);
            tile.Init(poolType);

            return tile;
        }

        private Tile GetTileByType(TileType type)
        {
            PoolType poolType = type switch
            {
                TileType.Red => PoolType.TileRed,
                TileType.Green => PoolType.TileGreen,
                TileType.Blue => PoolType.TileBlue,
                TileType.Yellow => PoolType.TileYellow,
                TileType.Bomb => PoolType.TileBomb,
                _ => PoolType.TileRed
            };

            Tile tile = mPoolService.Get<Tile>(poolType);
            tile.Init(poolType);
            return tile;
        }


        /// <summary>
        /// This makes tiles fall down if they have an empty space (Slot) underneath them.
        /// </summary>
        public bool GravitySolver()
        {
            bool solved = true;

            // Apply gravitational wave from bottom to top for faster resolution
            for (int y = mHeight - 1; y >= 0; y--)
            {
                for (int x = 0; x < mWidth; x++)
                {
                    // Check if current slot is not empty and down below slot is empty
                    var currentSlot = mBoardMap[y, x];

                    if (!currentSlot.IsEmpty)
                    {
                        int nextY = y + 1;

                        if (nextY < mHeight && mBoardMap[nextY, x].IsEmpty)
                        {
                            // Then move current tile to the slot down below
                            // Tile positions are saved to be moved later
                            var belowSlot = mBoardMap[nextY, x];
                            belowSlot.SetTile(currentSlot.TheTile, false);
                            currentSlot.Clear();
                            belowSlot.TheTile.RecordMoveTo(belowSlot.Position);
                            solved = false;
                        }
                    }
                }
            }

            return solved;
        }

        /// <summary>
        /// Get list of empty slots
        /// </summary>
        public List<Slot> GetEmptySlots() 
        {
            var emptySlots = new List<Slot>();

            for(int y = 0; y < mHeight; y++) 
            {
                for (int x = 0; x < mWidth; x++) 
                {
                    var slot = mBoardMap[y, x];
                    
                    if (slot.IsEmpty) 
                    {
                        emptySlots.Add(slot);
                    }
                }
            }

            return emptySlots;
        }

        
      
        public bool IsThereAnyValidMove()
        {
            for (int y = 0; y < mHeight - 1; y++)
            {
                for (int x = 0; x < mWidth - 1; x++)
                {
                    if (IsMatchAt(x, y))
                        return true;
                }
            }
            return false;
        }

        private bool IsMatchAt(int x, int y)
        {
            var current = mBoardMap[y, x];
            var right = mBoardMap[y, x + 1];
            var below = mBoardMap[y + 1, x];

            return current.TheTile.TType == right.TheTile.TType ||
                   current.TheTile.TType == below.TheTile.TType ||
                   current.TheTile.SpecialTile ||
                   right.TheTile.SpecialTile ||
                   below.TheTile.SpecialTile;
        }

        /// <summary>
        /// Tiles record move to position on themselves.
        /// They start to move when move command is applied.
        /// </summary>
        public void ApplyTileMove()
        {
            for (int y = 0; y < mHeight; y++)
            {
                for (int x = 0; x < mWidth; x++)
                {
                    var slot = mBoardMap[y, x];
                    
                    if (!slot.IsEmpty)
                    {
                        slot.TheTile.ApplyMove();
                    }
                }
            }
        }

        /// <summary>
        /// Drop new tiles from heaven
        /// </summary>
        public void DropNewTiles(Transform parent, float fallAboveHeight, float specialChance)
        {
            var fallPosY   = mBoardMap[0, 0].Position.y + fallAboveHeight;
            var emptySlots = GetEmptySlots();

            // Create a new random tile for each empty slot
            foreach (var slot in emptySlots)
            {
                var randomTile = GetRandomTile(specialChance);
                randomTile.transform.SetParent(parent);

                // Set tile's initial position somewhere above the board.
                // Put the tile into the slot and record move position to animate later
                var tileStartPos = slot.Position + new Vector3(0.0f, fallPosY, 0.0f);
                randomTile.SetPosition(tileStartPos);
                slot.SetTile(randomTile, false);
                randomTile.RecordMoveTo(slot.Position);
                randomTile.ApplyMove();
            }
        }

        public void ClearTheBoard() 
        {
            for (int y = 0; y < mHeight; y++)
            {
                for (int x = 0; x < mWidth; x++)
                {
                    var slot = mBoardMap[y, x];

                    if (!slot.IsEmpty) 
                    {
                        slot.TheTile.Pop();
                        slot.Clear();
                    }
                }
            }
        }

        public static int CoordToIndex(int x, int y, int width)
        {
            return y * width + x;
        }
    }
}
