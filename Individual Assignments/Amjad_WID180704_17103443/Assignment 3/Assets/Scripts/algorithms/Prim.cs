using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace algorithms
{
    public class Prim : MazeAlgorithm
    {
        private bool _started;
        private readonly HashSet<Vec2d> _frontier;

        public Prim(Vec2d size) : base(size)
        {
            _frontier = new HashSet<Vec2d>();
            _started = false;
        }

        public override bool GenerateNext()
        {
            if (!_started)
            {
                // random starting cell
                AddCellAndMarkNeighbours(new Vec2d(Random.Range(0, Size.x), Random.Range(0, Size.z)));
                _started = true;
                return true;
            }

            if (_frontier.Count == 0) return false;

            var index = Random.Range(0, _frontier.Count);
            var nextFrontier = _frontier.ElementAt(index);
            _frontier.Remove(nextFrontier);

            var filledNeighbors = GetFilledNeighbors(nextFrontier);
            if (filledNeighbors.Count == 0)
                throw new Exception("Frontier does not have filled neighbors when it should have");

            index = Random.Range(0, filledNeighbors.Count);
            var cellToConnect = filledNeighbors[index];

            AddCellAndMarkNeighbours(nextFrontier);
            ConnectTwoCells(nextFrontier, cellToConnect);

            // display walls if the cell does not have any empty neighboring cells
            foreach (var neighbor in filledNeighbors.Where(neighbor => GetEmptyNeighbors(neighbor).Count == 0))
            {
                OnCellWallsAddedHandler(neighbor, Cells[neighbor.x, neighbor.z].GetWallsDirections());
            }

            // display walls if the cell does not have any empty neighboring cells
            if (GetEmptyNeighbors(nextFrontier).Count == 0)
            {
                OnCellWallsAddedHandler(nextFrontier, Cells[nextFrontier.x, nextFrontier.z].GetWallsDirections());
            }

            return true;
        }

        private void AddCellAndMarkNeighbours(Vec2d pos)
        {
            CreateCell(pos);
            _frontier.UnionWith(GetEmptyNeighbors(pos));
        }

        private void ConnectTwoCells(Vec2d cell1, Vec2d cell2)
        {
            var diff = cell2 - cell1;

            var direction = MazeDirections.FromVec2d(diff);
            if (direction.HasValue)
            {
                Assert.AreEqual(direction.Value.ToVec2d() + cell1, cell2);

                Cells[cell1.x, cell1.z].RemoveWall(direction.Value);
                Cells[cell2.x, cell2.z].RemoveWall(direction.Value.Opposite());
            }
            else
            {
                throw new NullReferenceException("direction is null");
            }
        }
    }
}