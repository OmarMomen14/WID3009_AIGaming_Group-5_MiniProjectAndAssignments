using System.Collections;
using algorithms;
using UnityEngine;

public class Maze : MonoBehaviour
{
    public Vec2d size;
    public MazeCell mazeCellPrefab;
    public MazeWall mazeWallPrefab;
    public float buildDelay;

    private MazeCell[,] _cells;

    public string GetAlgorithmName(int algoIndex)
    {
        string algoName;
        switch (algoIndex)
        {
            case 1:
                algoName = "Recursive Backtracking";
                break;
            case 2:
                algoName = "Prim's Algorithm";
                break;
            case 3:
                algoName = "Sidewinder Algorithm";
                break;
            default:
                algoName = "None";
                break;
        }

        return algoName;
    }

    public IEnumerator Generate(int algoIndex)
    {
        _cells = new MazeCell[size.x, size.z];

        var delay = new WaitForSeconds(buildDelay);
        MazeAlgorithm mazeGenerator;
        switch (algoIndex)
        {
            case 1:
                mazeGenerator = new Backtracking(size);
                break;
            case 2:
                mazeGenerator = new Prim(size);
                break;
            case 3:
                mazeGenerator = new Sidewinder(size);
                break;
            default:
                goto end;
        }

        mazeGenerator.AddOnCellAddedListener(CreateCell);
        mazeGenerator.AddOnCellWallsAddedListener(CreateWalls);

        while (mazeGenerator.GenerateNext())
        {
            yield return delay;
        }

        end: ;
    }

    private void CreateWalls(Vec2d pos, MazeDirection[] directions)
    {
        foreach (var direction in directions)
        {
            CreateWall(pos, direction);
        }
    }

    private void CreateWall(Vec2d pos, MazeDirection direction)
    {
        if (this != null)
        {
            var directionVec = direction.ToVec2d();
            var newWall = Instantiate(mazeWallPrefab, transform, true);
            _cells[pos.x, pos.z].walls[(int) direction] = newWall;

            newWall.GetComponentInChildren<Renderer>().material.color = Color.red;
            newWall.name = "Maze Wall " + pos.x + ", " + pos.z + ", " + direction;
            newWall.transform.rotation = direction.Rotation();
            var position = new Vector3(pos.x - size.x * 0.5f + 0.5f, 0.475f, pos.z - size.z * 0.5f + 0.5f);
            position += new Vector3(directionVec.x * 0.5f, 0, directionVec.z * 0.5f);
            newWall.transform.localPosition = position;
        }
    }

    private void CreateCell(Vec2d pos)
    {
        if (this != null)
        {
            var newCell = Instantiate(mazeCellPrefab, transform, true);
            _cells[pos.x, pos.z] = newCell;
            newCell.GetComponentInChildren<Renderer>().material.color = Color.blue;
            newCell.name = "Maze Cell " + pos.x + ", " + pos.z;
            newCell.transform.localPosition =
                new Vector3(pos.x - size.x * 0.5f + 0.5f, 0f, pos.z - size.z * 0.5f + 0.5f);
        }
    }
}