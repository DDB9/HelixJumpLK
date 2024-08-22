using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject GameEndScreen;
    public BallBehavior BallOne, BallTwo;
    public int Score { get; private set; }
    public int HighScore { get; private set; }
    public List<Powerup> ActivePowerups = new List<Powerup>();

    #region LEVEL GENEERATION VARIABLES
    public int CurrentLevel;
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
        GameEndScreen.SetActive(false);

        // Length of the level is calculated here.
        helixLength = FirstPlatformTransform.localPosition.y - (LastPlatformTransform.localPosition.y + 0.1f);
        HelixStartRotation = Helix.transform.localEulerAngles;
        LoadLevel(CurrentLevel);

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (BallOne.IsFinished && BallTwo.IsFinished)
        {
            NextLevel();

            BallOne.IsFinished = false;
            BallOne.AllowCollision();

            BallTwo.IsFinished = false;
            BallTwo.AllowCollision();
        }
    }

    /// <summary>
    /// Generates a level corresponding to the given level index parameter.
    /// </summary>
    /// <param name="pLevelIndex">Level to load.</param>
    public void LoadLevel(int pLevelIndex)
    {
        Level _level = AllLevels[Mathf.Clamp(pLevelIndex, 0, AllLevels.Count - 1)];
        if (_level == null)
        {
            Debug.LogError("No level " + pLevelIndex + " was found. Please make sure all levels are assigned in " + this.name);
            return;
        }
        CurrentLevel = pLevelIndex;

        try
        {
            Camera.main.backgroundColor = AllLevels[pLevelIndex].LevelBGColor;  // Change camera background color.
        }
        catch (System.Exception)
        {
            GameEndScreen.SetActive(true);
            return;
        }

        // Reset Helix rotation.
        Helix.transform.eulerAngles = HelixStartRotation;

        // If present, destroy any old levels.
        foreach (GameObject obj in Platforms) Destroy(obj);

        // Then create the new platforms
        float _platformDistance = helixLength / _level.Platforms.Count; // calculate the distance between platforms.
        float _spawnPositionY = FirstPlatformTransform.localPosition.y; // define first platform spawn location.

        // Loop through each platform individually.
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
                GameObject _randomSlice = _platform.transform.GetChild(Random.Range(0, _platform.transform.childCount)).gameObject;
                GameObject _randomSliceTwo = _platformTwo.transform.GetChild(Random.Range(0, _platformTwo.transform.childCount)).gameObject;
                _randomSlice.SetActive(false);
                _randomSliceTwo.SetActive(false);
                if (!_inactiveParts.Contains(_randomSlice)) _inactiveParts.Add(_randomSlice);
                if (!_inactiveParts.Contains(_randomSliceTwo)) _inactiveParts.Add(_randomSliceTwo);
            }

            // Color the level according to its level variables.
            List<GameObject> _remainingSlices = new List<GameObject>();
            foreach (Transform t in _platform.transform)
            {
                t.GetComponent<Renderer>().material.color = AllLevels[pLevelIndex].LevelPlatformColor;
                if (t.gameObject.activeInHierarchy) _remainingSlices.Add(t.gameObject);
            }

            foreach (Transform t in _platformTwo.transform)
            {
                t.GetComponent<Renderer>().material.color = AllLevels[pLevelIndex].LevelPlatformColor;
                if (t.gameObject.activeInHierarchy) _remainingSlices.Add(t.gameObject);
            }

            // Spawn random powerups.
            if (_level.Platforms[i].PowerupPresent)
            {
                // choose a random powerup (all children of the slices).
                GameObject _randomSlice = _remainingSlices[Random.Range(0, _remainingSlices.Count - 1)];
                GameObject _powerup = _randomSlice.transform.GetChild(Random.Range(0, _randomSlice.transform.childCount)).gameObject;
                _powerup.SetActive(true);
                if (_remainingSlices.Contains(_randomSlice)) _remainingSlices.Remove(_randomSlice);
            }

            // Then finally place the kill slices randomly between the remaining slices.
            List<GameObject> _killSlices = new List<GameObject>();
            while (_killSlices.Count < _level.Platforms[i].KillSliceCount)
            {
                GameObject _randomPart = _remainingSlices[Random.Range(0, _remainingSlices.Count - 1)];
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

        foreach (Powerup p in ActivePowerups) p.ResetPowerup();
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
