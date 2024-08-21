using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public BallBehavior BallOne, BallTwo;
    public int Score { get; private set; }
    public int HighScore { get; private set; }

    #region LEVEL GENEERATION VARIABLES
    public int CurrentLevel { get; private set; }
    public Transform FirstPlatformTransform, LastPlatformTransform;
    public GameObject Helix, HelixTwo, PlatformPrefab;
    public List<Level> AllLevels = new List<Level>();

    private float helixLength;
    private Vector3 HelixStartRotation;
    private List<GameObject> Platforms = new List<GameObject>();
    #endregion

    private void Awake()
    {
        // Set the application's target framerate to 60.
        // Since it's a lightweight game, 60fps should be perfectly doable on most modern day devices.
        Application.targetFrameRate = 60;

        // Simple Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        HighScore = PlayerPrefs.GetInt("High Score");
        CurrentLevel = 0;

        // Length of the level is calculated here.
        helixLength = FirstPlatformTransform.localPosition.y - (LastPlatformTransform.localPosition.y + 0.1f);
        HelixStartRotation = Helix.transform.localEulerAngles;
        LoadLevel(0);

        DontDestroyOnLoad(gameObject);
    }

    // This takes care of the level generation.
    public void LoadLevel(int pLevelIndex)
    {
        Level _level = AllLevels[Mathf.Clamp(pLevelIndex, 0, AllLevels.Count -1)];
        if (_level == null)
        {
            Debug.LogError("No level " + pLevelIndex + " was found. Please make sure all levels are assigned in " + this.name);
            return;
        }

        Camera.main.backgroundColor = AllLevels[pLevelIndex].LevelBGColor;  // Change camera background color.

        // Reset Helix rotation.
        Helix.transform.eulerAngles = HelixStartRotation;

        // If present, destroy any old levels.
        foreach (GameObject obj in Platforms) Destroy(obj);

        // Then create the new platforms
        float _platformDistance = helixLength / _level.Platforms.Count; // calculate the distance between platforms.
        float _spawnPositionY = FirstPlatformTransform.localPosition.y; // define first platform spawn location.
        for (int i = 0; i < _level.Platforms.Count; i++)
        {
            _spawnPositionY -= _platformDistance;
            GameObject _platform = Instantiate(PlatformPrefab, Helix.transform);
            GameObject _platformTwo = Instantiate(PlatformPrefab, HelixTwo.transform);
            _platform.transform.localPosition = new Vector3(0, _spawnPositionY, 0);
            _platformTwo.transform.localPosition = new Vector3(0, _spawnPositionY, 0);

            // Disable the specified amount of slices.
            int _slicesToDisable = 12 - _level.Platforms[i].SliceCount;
            List<GameObject> _inactiveParts = new List<GameObject>();

            // Keep disabling parts until the specified amount per platform has been reached.
            while (_inactiveParts.Count < _slicesToDisable)
            {
                GameObject _randomPart = _platform.transform.GetChild(Random.Range(0, _platform.transform.childCount)).gameObject;
                GameObject _randomPartTwo = _platformTwo.transform.GetChild(Random.Range(0, _platformTwo.transform.childCount)).gameObject;
                _randomPart.SetActive(false);
                _randomPartTwo.SetActive(false);
                if (!_inactiveParts.Contains(_randomPart)) _inactiveParts.Add(_randomPart);
                if (!_inactiveParts.Contains(_randomPartTwo)) _inactiveParts.Add(_randomPartTwo);
            }

           
            // Color the level according to its level variables.
            List<GameObject> _remainingParts = new List<GameObject>();
            foreach (Transform t in _platform.transform)
            {
                if (!t.CompareTag("ScoreCollider"))
                {
                    t.GetComponent<Renderer>().material.color = AllLevels[pLevelIndex].LevelPlatformColor;
                    if (t.gameObject.activeInHierarchy) _remainingParts.Add(t.gameObject);
                }
            }
            foreach (Transform t in _platformTwo.transform)
            {
                if (!t.CompareTag("ScoreCollider"))
                {
                    t.GetComponent<Renderer>().material.color = AllLevels[pLevelIndex].LevelPlatformColor;
                    if (t.gameObject.activeInHierarchy) _remainingParts.Add(t.gameObject);
                }
            }

            // Then finally place the kill slices randomly between the remaining slices.
            List<GameObject> _killSlices = new List<GameObject>();
            while (_killSlices.Count < _level.Platforms[i].KillSliceCount)
            {
                GameObject _randomPart = _remainingParts[(Random.Range(0, _remainingParts.Count))];
                if (!_killSlices.Contains(_randomPart)) _randomPart.gameObject.AddComponent<KillSlice>();
                _killSlices.Add(_randomPart);
            }
            
            Platforms.Add(_platform);
            Platforms.Add(_platformTwo);
        }
    }

    // Reset the ball and load the next stage.
    public void NextLevel()
    {
        CurrentLevel++;
        BallOne.ResetBall();
        BallTwo.ResetBall();
        LoadLevel(CurrentLevel);
    }

    // Reset the score and ball, then reload the current level.
    public void RestartLevel()
    {
        Score = 0;
        BallOne.ResetBall();
        BallTwo.ResetBall();
    }

    // Some basic scoring functionality
    public void AddScore(int pScore)
    {
        Score += pScore;
        if (Score > HighScore)
        {
            HighScore = Score;
            PlayerPrefs.SetInt("High Score", Score);
        }
    }
}
