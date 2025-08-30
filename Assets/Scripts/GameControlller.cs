using System;
using UnityEngine;

public enum GameLocalization
{
    SWAMPS,
    DUNGEON,
    CASTLE,
    CITY,
    TOWER
}

public enum SoulWeakness
{
    BOW = 0,
    SWORD = 1 
}

public class GameControlller : MonoBehaviour
{
    #region Singleton

    private static GameControlller _instance;

    public static GameControlller Instance
    {
        get
        {
            if (_instance == null) _instance = FindFirstObjectByType<GameControlller>();
            return _instance;
        }
        set => _instance = value;
    }

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    [SerializeField] private GameLocalization currentGameLocalization;

    public GameLocalization CurrentGameLocalization
    {
        get => currentGameLocalization;

        set => currentGameLocalization = value;
    }

    public event Action<bool> OnPaused;
    private bool _isPaused;

    public bool IsPaused
    {

        get => _isPaused;
        set
        {
            _isPaused = value;
            Time.timeScale = _isPaused ? 0f : 1f;
            OnPaused?.Invoke(_isPaused);
        }
    }

    public bool IsCurrentLocalization(GameLocalization localization)
    {
        return CurrentGameLocalization == localization;
    }
}