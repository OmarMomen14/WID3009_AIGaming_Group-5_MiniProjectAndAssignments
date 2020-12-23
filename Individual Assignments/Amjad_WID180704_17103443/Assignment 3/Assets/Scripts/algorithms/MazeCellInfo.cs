using System.Linq;

namespace algorithms
{
    public class MazeCellInfo
    {
        private readonly bool[] _directions = {true, true, true, true};

        public MazeDirection[] GetWallsDirections()
        {
            var len = _directions.Count(direction => direction);
            var result = new MazeDirection[len];

            var j = 0;

            for (var i = 0; i < _directions.Length; i++)
            {
                if (_directions[i])
                {
                    result[j] = (MazeDirection) i;
                    j++;
                }
            }

            return result;
        }

        public void RemoveWall(MazeDirection direction)
        {
            _directions[(int) direction] = false;
        }
    }
}