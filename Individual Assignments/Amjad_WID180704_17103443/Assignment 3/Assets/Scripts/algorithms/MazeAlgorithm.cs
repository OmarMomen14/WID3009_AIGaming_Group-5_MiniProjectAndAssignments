using System;
using System.Collections.Generic;

namespace algorithms
{
    public abstract class MazeAlgorithm
    {
        protected Vec2d Size;

        protected readonly MazeCellInfo[,] Cells;
        protected Action<Vec2d> OnCellAddedHandler = a => { };
        protected Action<Vec2d, MazeDirection[]> OnCellWallsAddedHandler = (a, b) => { };

        public MazeAlgorithm(Vec2d size)
        {
            this.Size = size;
            Cells = new MazeCellInfo[size.x, size.z];
        }

        /// <summary>
        /// generate a single cell, returns true if this function should be called again,
        /// false otherwise
        /// </summary>
        /// <returns>`false` when the function is finished and not more cells will be made</returns>
        public abstract bool GenerateNext();

        public void AddOnCellAddedListener(Action<Vec2d> handler)
        {
            OnCellAddedHandler += handler;
        }

        public void AddOnCellWallsAddedListener(Action<Vec2d, MazeDirection[]> handler)
        {
            OnCellWallsAddedHandler += handler;
        }

        protected bool ContainsCoordinates(Vec2d coordinate)
        {
            return coordinate.x >= 0 && coordinate.x < Size.x && coordinate.z >= 0 && coordinate.z < Size.z;
        }

        protected void CreateCell(Vec2d pos)
        {
            Cells[pos.x, pos.z] = new MazeCellInfo();
            OnCellAddedHandler(pos);
        }

        protected List<Vec2d> GetEmptyNeighbors(Vec2d pos)
        {
            return GetNeighbors(pos, newPos => ContainsCoordinates(newPos) && Cells[newPos.x, newPos.z] == null);
        }

        protected List<Vec2d> GetFilledNeighbors(Vec2d pos)
        {
            return GetNeighbors(pos, newPos => ContainsCoordinates(newPos) && Cells[newPos.x, newPos.z] != null);
        }

        private List<Vec2d> GetNeighbors(Vec2d pos, Func<Vec2d, bool> condition)
        {
            var neighbors = new List<Vec2d>(4);

            foreach (MazeDirection direction in Enum.GetValues(typeof(MazeDirection)))
            {
                var newPos = pos + direction.ToVec2d();
                if (condition(newPos))
                {
                    neighbors.Add(newPos);
                }
            }

            return neighbors;
        }
    }
}