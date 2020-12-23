using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace algorithms
{
    public class Sidewinder : MazeAlgorithm
    {
        private bool _started;
        private readonly HashSet<Vec2d> _run;
        private Vec2d _currentIndex;

        /// <summary>
        /// probability of carving right
        /// </summary>
        private int weight = 2;

        public Sidewinder(Vec2d size) : base(size)
        {
            _started = false;
            _run = new HashSet<Vec2d>();
            _currentIndex = new Vec2d(0, Size.z - 1);
        }

        public override bool GenerateNext()
        {
            if (!_started)
            {
                CreateCell(_currentIndex);
                _started = true;
                return true;
            }

            if (!ContainsCoordinates(_currentIndex)) return false;

            _run.Add(_currentIndex);

            var oldIndex = _currentIndex.Copy();
            _currentIndex += new Vec2d(1, 0);
            if (!ContainsCoordinates(_currentIndex))
            {
                CarvePathRunSet();
                _currentIndex += new Vec2d(-Size.x, -1);

                if (!ContainsCoordinates(_currentIndex))
                {
                    DisplayLastRow();
                    return false;
                }

                CreateCell(_currentIndex);

                return true;
            }

            CreateCell(_currentIndex);
            var shouldCarveWest = _currentIndex.z == Size.z - 1 || Random.Range(0, weight) != 0;

            if (shouldCarveWest)
            {
                Carve(oldIndex, MazeDirection.East);
            }
            else
            {
                CarvePathRunSet();
            }

            return true;
        }

        private void Carve(Vec2d pos, MazeDirection direction)
        {
            Cells[pos.x, pos.z].RemoveWall(direction);
            var other = pos + direction.ToVec2d();
            Cells[other.x, other.z].RemoveWall(direction.Opposite());
        }

        private void CarvePathRunSet()
        {
            var index = Random.Range(0, _run.Count);
            var toCarve = _run.ElementAt(index);

            if (toCarve.z >= 0 && toCarve.z < Size.z - 1)
            {
                Carve(toCarve, MazeDirection.North);
            }

            var z = _currentIndex.z + 1;
            if (z >= 0 && z < Size.z)
            {
                foreach (var cell in _run)
                {
                    DisplayCell(cell + new Vec2d(0, 1));
                }
            }

            _run.Clear();
        }

        private void DisplayCell(Vec2d pos)
        {
            OnCellWallsAddedHandler(pos, Cells[pos.x, pos.z].GetWallsDirections());
        }

        /// <summary>
        /// As we are displaying each line when the next line is computing carving,
        /// so the last line will not be drawn in this case, that's why we draw it in one go
        /// at the end
        /// </summary>
        private void DisplayLastRow()
        {
            var z = 0;
            for (int x = 0; x < Size.x; x++)
            {
                OnCellWallsAddedHandler(new Vec2d(x, z), Cells[x, z].GetWallsDirections());
            }
        }
    }
}