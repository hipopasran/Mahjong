using System;
using UnityEngine.SceneManagement;

public class GameSessionManager : Singleton<GameSessionManager>
{
    public event Action OnCloseLevel;
    private DataForBoard currentLevelData;
    public string LevelName { get; private set; }
    public DataForBoard CurrentLevelData => currentLevelData;
     
    public void OpenLevel(DataForBoard data, string levelName)
    {
        this.currentLevelData = data;
        this.LevelName = levelName;
        SceneManager.LoadScene("Level");
    }

    public void CloseLevel()
    {
        OnCloseLevel?.Invoke();
        SceneManager.LoadScene("Title");
    }
}
