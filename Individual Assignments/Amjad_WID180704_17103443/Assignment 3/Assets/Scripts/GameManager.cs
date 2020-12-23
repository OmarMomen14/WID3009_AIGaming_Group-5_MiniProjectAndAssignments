using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Maze mazePrefab;
    public Text algorithmText;
    public Text timeText;

    private Maze _mazeInstance;
    private int _currentGenerator;

    private Task _algoTask;
    private float _algoStartTime;

    void Start()
    {
        _currentGenerator = 1;
        BeginGame();
    }

    void Update()
    {
        // Space to restart the current algorithm
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }

        // Numbers 1-9 to choose an algorithm
        for (var i = (int) KeyCode.Alpha1; i < (int) KeyCode.Alpha9; i++)
        {
            if (Input.GetKeyDown((KeyCode) i))
            {
                var gen = i - (int) KeyCode.Alpha0;
                if (gen != _currentGenerator)
                {
                    _currentGenerator = gen;
                    RestartGame();
                }
            }
        }

        // update Time if an algorithm is running
        if (_algoTask != null && _algoTask.Running)
            DisplayElapsedAlgoTime();
    }

    void BeginGame()
    {
        // create Maze object
        _mazeInstance = Instantiate(mazePrefab);
        // update the algorithm name
        algorithmText.text = _mazeInstance.GetAlgorithmName(_currentGenerator);
        // start the generation task
        _algoTask = new Task(_mazeInstance.Generate(_currentGenerator));
        // mark the start time of the algorithm to be used when displaying the timer
        _algoStartTime = Time.time;

        // reset time font style
        timeText.fontStyle = FontStyle.Normal;
        // when the task is finished, set timer font to bold
        _algoTask.Finished += (manual) =>
        {
            if (!manual)
                timeText.fontStyle = FontStyle.Bold;
        };
    }

    void RestartGame()
    {
        _algoTask.Stop();
        Destroy(_mazeInstance.gameObject);
        BeginGame();
    }

    private void DisplayElapsedAlgoTime()
    {
        var timeToDisplay = Time.time - _algoStartTime;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float milliseconds = Mathf.FloorToInt((timeToDisplay - Mathf.FloorToInt(timeToDisplay)) * 100);

        timeText.text = $"{minutes:00}:{seconds:00}:{milliseconds:00}";
    }
}