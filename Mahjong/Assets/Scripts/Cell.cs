using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

[RequireComponent(typeof(CellView))]
public class Cell : MonoBehaviour, IPointerDownHandler
{
    public event Action<Cell> OnClick;

    private CellView cellView;
    [SerializeField] private string cellType;

    [SerializeField] private Cell leftCell;
    [SerializeField] private Cell upCell;
    [SerializeField] private Cell rightCell;
    [SerializeField] private Cell downCell;

    public string  CellType => cellType;
    public CellView View => cellView;

    public void SetNeighborsLink(Cell leftCell, Cell upCell, Cell rightCell, Cell downCell)
    {
        this.leftCell = leftCell;
        this.upCell = upCell;
        this.rightCell = rightCell;
        this.downCell = downCell;
    }

    public void SetType(string cellType)
    {
        this.cellType = cellType;
    }

    public void ResetCell()
    {
        cellType = string.Empty;
        cellView.ResetView();
    }

    public void Deselect()
    {
        cellView.OnUncklick();
    }

    public void ShowHint()
    {
        cellView.ShowHint();
    }

    public void HideHint()
    {
        cellView.HideHint();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(cellType == string.Empty)
        {
            return;
        }

        cellView.OnClick();
        OnClick?.Invoke(this);
    }

    public bool TryFindPath(Cell target, int changeDirectionTime, Cell incomingCell)
    {
        bool resultLeft = false;
        bool resultUp = false;
        bool resultRight = false;
        bool resultDown = false;

        if(this == target)
        {
            return true;
        }
        else
        {
            if(leftCell != null && leftCell != incomingCell && (leftCell.cellType == string.Empty || leftCell == target))
            {
                var tempChangeDirectionTimes = changeDirectionTime;
                if(incomingCell == upCell || incomingCell == downCell)
                {
                    tempChangeDirectionTimes += 1;
                }

                if(tempChangeDirectionTimes < 3)
                {
                    resultLeft =  leftCell.TryFindPath(target, tempChangeDirectionTimes, this);
                }

            }
            if(upCell != null && upCell != incomingCell && (upCell.cellType == string.Empty || upCell == target))
            {
                var tempChangeDirectionTimes = changeDirectionTime;
                if(incomingCell == rightCell || incomingCell == leftCell)
                {
                    tempChangeDirectionTimes += 1;
                }

                if(tempChangeDirectionTimes < 3)
                {
                    resultUp = upCell.TryFindPath(target, tempChangeDirectionTimes, this);
                }
            }
            if(rightCell != null && rightCell != incomingCell && (rightCell.cellType == string.Empty || rightCell == target))
            {
                var tempChangeDirectionTimes = changeDirectionTime;
                if(incomingCell == downCell || incomingCell == upCell)
                {
                    tempChangeDirectionTimes += 1;
                }

                if(tempChangeDirectionTimes < 3)
                {
                    resultRight = rightCell.TryFindPath(target, tempChangeDirectionTimes, this);
                }
            }
            if(downCell != null && downCell != incomingCell && (downCell.cellType == string.Empty || downCell == target))
            {
                var tempChangeDirectionTimes = changeDirectionTime;
                if(incomingCell == rightCell || incomingCell == leftCell)
                {
                    tempChangeDirectionTimes += 1;
                }

                if(tempChangeDirectionTimes < 3)
                {
                    resultDown = downCell.TryFindPath(target, tempChangeDirectionTimes, this);
                }
            }

            if (resultLeft || resultUp || resultRight || resultDown)
            {
                return true;
            }
        }
        return false;
    }

    private void Awake()
    {
        cellView = GetComponent<CellView>();
    }
}
