using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GamePlayManager : MonoBehaviour
{
    public event Action<bool, int, int> OnLevelEnd;
    public event Action<int> OnScoreUpdate;

    private bool canMove;
    private int score;
    private string levelName;
    private string scoreString;
    private Cell firstElement;
    private Cell secondElement;
    private Board board;
    private List<Cell> cellForHint = new List<Cell>();

    public void Setup(Board board,string levelName)
    {
        this.board = board;
        this.board.OnSelectedCell += CellClick;
        this.board.OnBoardCreated += DoAfterBoardLoad;
        this.levelName = levelName;
        this.scoreString = levelName + "_score";
    }

    public void ShowHint()
    {
        foreach(var cell in cellForHint)
        {
            cell.ShowHint();
        }
    }

    private void OnDestroy()
    {
        board.OnSelectedCell -= CellClick;
        board.OnBoardCreated -= DoAfterBoardLoad;
    }

    private void LevelEnd(bool result)
    {
        if(result)
        {
            PlayerPrefs.SetInt(levelName, 1);
        }
        var highScore = UpdateHighScore();
        OnLevelEnd?.Invoke(result, score, highScore);
    }

    private bool CheckAvaliableMove(IEnumerable<string> types)
    {
        if(types.Count() == 0)
        {
            LevelEnd(true);
            return true;
        }
        foreach (var type in types)
        {
            var listOfCells = board.AvaliableCells.Where(x => x.CellType == type).ToList();
            for(int i = 0; i < listOfCells.Count() - 1; i++)
            {
                var result = listOfCells[i].TryFindPath(listOfCells[i + 1], 0, listOfCells[i]);
                if(result)
                {
                    cellForHint.Clear();
                    cellForHint.Add(listOfCells[i]);
                    cellForHint.Add(listOfCells[i + 1]);
                    return true;
                }
            }
        }
        return false;
    }

    private void ScoreUpdate(int value)
    {
        if (score + value >= 0)
        {
            score += value;
        }
        else
        {
            score = 0;
        }
            OnScoreUpdate?.Invoke(score);
    }

    private int UpdateHighScore()
    {
        if (PlayerPrefs.HasKey(scoreString))
        {
            if (score > PlayerPrefs.GetInt(scoreString))
            {
                PlayerPrefs.SetInt(scoreString, score);
            }
            else
            {
                return PlayerPrefs.GetInt(scoreString);
            }
        }
        else
        {
            PlayerPrefs.SetInt(scoreString, score);
        }
        return score;
    }

    private void DoAfterBoardLoad()
    {
        ScoreUpdate(0);
        CheckBoard();
    }

    private void CheckBoard()
    {
        var types = board.AvaliableTypes;
        if (types.Count() == 0)
        {
            LevelEnd(true);
            return;
        }
        canMove = CheckAvaliableMove(types);
        if (!canMove)
        {
            LevelEnd(false);
        }
    }

    private void ResetHints()
    {
        foreach(var item in cellForHint)
        {
            item.HideHint();
        }

        cellForHint.Clear();
    }

    private void CellClick(Cell cell)
    {
        if(firstElement == null)
        {
            firstElement = cell;
            return;
        }
        if(firstElement != null)
        {
            secondElement = cell;
            if (firstElement != secondElement && firstElement.CellType == secondElement.CellType)
            {
                var result = firstElement.TryFindPath(secondElement, 0, firstElement);
                if(result)
                {
                    ResetHints();
                    ScoreUpdate(15);

                    board.RemoveAvaliableCell(firstElement, secondElement);

                    CheckBoard();
                }
                else
                {
                    WrongChips();
                }
            }
            else
            {
                WrongChips();
            }

            firstElement = null;
            secondElement = null;
        }
    }

    private void WrongChips()
    {
        ScoreUpdate(-10);
        firstElement.Deselect();
        secondElement.Deselect();
    }
}
