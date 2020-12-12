using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using Random = UnityEngine.Random;
using System;

public class Board : MonoBehaviour
{
    public event Action OnBoardCreated;
    public event Action<Cell> OnSelectedCell;

    private Cell[,] cells;
    private List<Cell> avaliableCells = new List<Cell>();

    [Header("Board Settings")]
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Transform boardRoot;
    [Header("Sprites For Chips")]
    [SerializeField] private List<Sprite> cellImages = new List<Sprite>();

    public IEnumerable<Cell> AvaliableCells => avaliableCells;
    public IEnumerable<string> AvaliableTypes
    {
        get
        {
            List<string> types = new List<string>();
            foreach(var item in AvaliableCells)
            {
                types.Add(item.CellType);
            }

            return types.Distinct().ToList();
        }
    }

    public void Generate(DataForBoard data)
    {
        ResetTempData(data.y, data.x);
        SetBoardGridLayout(data.x);
        var cellTypes = SelectCellTypesForBoard(data.layoutString);
        GenerateBoardWithEmptyCells(data.y, data.x);
        SetCells(data, cellTypes);

        OnBoardCreated?.Invoke();
    }

    public void RemoveAvaliableCell(Cell first, Cell second)
    {
        first.ResetCell();
        second.ResetCell();

        avaliableCells.Remove(first);
        avaliableCells.Remove(second);
    }

    private void SetBoardGridLayout(int columnCount)
    {
        var boardRootGridLayout = boardRoot.GetComponent<GridLayoutGroup>();
        boardRootGridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        boardRootGridLayout.constraintCount = columnCount;
    }

    private List<Sprite> SelectCellTypesForBoard(string layoutString)
    {
        var types = new List<Sprite>();
        var typesCount = layoutString.Count(x => x.Equals('X'));
        if (typesCount % 2 > 0)
        {
            typesCount = typesCount / 2 + 1;
            Debug.LogError("This template contains an odd number of play chips");
        }
        else
        {
            typesCount /= 2;
        }

        for (int i = 0; i < typesCount; i++)
        {
            var selectedType = cellImages[Random.Range(0, cellImages.Count)];
            types.Add(selectedType);
            types.Add(selectedType);
        }

        return types;
    }

    private void GenerateBoardWithEmptyCells(int yMax, int xMax)
    {
        for (int y = 0; y < yMax; y++)
        {
            for (int x = 0; x < xMax; x++)
            {
                var cellInstance = Instantiate(cellPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
                var cellController = cellInstance.GetComponent<Cell>();
                cells[y, x] = cellController;
                cellController.OnClick += SelectCell;
            }
        }
    }

    private void SetCells(DataForBoard data, List<Sprite> typesCell)
    {
        for (int y = 0; y < data.y; y++)
        {
            for (int x = 0; x < data.x; x++)
            {
                var cellView = cells[y, x].View;
                var cellController = cells[y, x];

                if (data.layoutString[y * (data.x) + x].Equals('X'))
                {
                    var cellSprtie = typesCell[Random.Range(0, typesCell.Count)];
                    typesCell.Remove(cellSprtie);
                    avaliableCells.Add(cellController);

                    if (cellView != null)
                    {
                        cellView.CreateView(boardRoot, cellSprtie);
                    }
                    if (cellController != null)
                    {
                        cellController.SetType(cellSprtie.name);
                    }
                }
                else
                {
                    if (cellView != null)
                    {
                        cellView.CreateView(boardRoot);
                    }
                    if (cellController != null)
                    {
                        cellController.SetType(string.Empty);
                    }
                }

                CellNeighborsLinking(cellController, x, y, data.x, data.y);
            }
        }
    }

    private void CellNeighborsLinking(Cell cell, int x, int y, int xMax, int yMax)
    {
        var leftCell = x != 0 ? cells[y, x -1] : null;
        var upCell = y != 0 ? cells[y - 1 , x] : null;
        var rightCell = x < xMax-1 ? cells[y, x + 1] : null;
        var downCell = y < yMax-1 ? cells[y + 1, x] : null;

        cell.SetNeighborsLink(leftCell, upCell, rightCell, downCell);
    }

    private void ResetTempData(int y, int x)
    {
        cells = new Cell[y, x];
        avaliableCells.Clear();
    }

    private void SelectCell(Cell selectedCell)
    {
        OnSelectedCell?.Invoke(selectedCell);
    }
}
