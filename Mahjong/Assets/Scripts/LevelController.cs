using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    private string levelName;
    private DataForBoard currentLevelData;

    [Header("Managers")]
    [SerializeField] private Board board;
    [SerializeField] private GamePlayManager gamePlayManager;
    [SerializeField] private UIManager uiManager;

    public void LoadMainMenu()
    {
        GameSessionManager.Instance.CloseLevel();
    }

    private void Start()
    {
        InitData();
        InitLevelManagers();
        CreateBoard();
    }

    private void InitLevelManagers()
    {
        gamePlayManager.Setup(board, levelName);
        uiManager.Setup(gamePlayManager);
    }

    private void InitData()
    {
        currentLevelData = GameSessionManager.Instance.CurrentLevelData;
        levelName = GameSessionManager.Instance.LevelName;
    }

    private void CreateBoard()
    {
        board.Generate(currentLevelData);
    }
}
