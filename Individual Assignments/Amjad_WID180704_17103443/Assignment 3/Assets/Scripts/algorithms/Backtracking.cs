using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace algorithms
{
    public class Backtracking : MazeAlgorithm
    {
        private readonly List<Vec2d> _stack;
        private bool _started;

        public Backtracking(Vec2d size) : base(size)
        {
            _stack = new List<Vec2d>(size.x * size.z);
            _started = false;
        }

        public override bool GenerateNext()
        {
            if (!_started)
            {
                var cell = new Vec2d(0, 0);
                _stack.Add(cell);
                CreateCell(cell);
                _started = true;
                return true;
            }

            if (_stack.Count <= 0) return false;

            var index = _stack.Count - 1;
            var current = _stack[index];

            var (neighborsPresent, mazeDirection) = _getRandomEmptyNeighbor(current);

            if (neighborsPresent)
            {
                Cells[current.x, current.z].RemoveWall(mazeDirection);

                Vec2d newPos = mazeDirection.ToVec2d() + current;

                CreateCell(newPos);
                _stack.Add(newPos);

                Cells[newPos.x, newPos.z].RemoveWall(mazeDirection.Opposite());
            }
            else
            {
                OnCellWallsAddedHandler(current, Cells[current.x, current.z].GetWallsDirections());
                _stack.RemoveAt(index);
            }

            return true;
        }


        private (bool, MazeDirection) _getRandomEmptyNeighbor(Vec2d pos)
        {
            List<MazeDirection> neighbors = new List<MazeDirection>(4);

            foreach (MazeDirection direction in Enum.GetValues(typeof(MazeDirection)))
            {
                Vec2d newPos = pos + direction.ToVec2d();
                if (ContainsCoordinates(newPos) && Cells[newPos.x, newPos.z] == null)
                {
                    neighbors.Add(direction);
                }
            }

            return neighbors.Count == 0
                ? (false, MazeDirection.North)
                : (true, neighbors[Random.Range(0, neighbors.Count)]);
        }
    }
}