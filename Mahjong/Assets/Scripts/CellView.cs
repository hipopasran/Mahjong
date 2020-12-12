using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellView : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image cellImage;
    [SerializeField] private Image cellBackground;
    [SerializeField] private Animator animator;

    [SerializeField] private Color clickColor;


    public void CreateView(Transform mainCanvas, Sprite cellImageSprite = null)
    {
        transform.SetParent(mainCanvas.transform);
        if (cellImageSprite != null)
        {
            cellImage.sprite = cellImageSprite;
        }
        else
        {
            cellBackground.enabled = false;
            cellImage.enabled = false;
        }
        rectTransform.localScale = new Vector3(1f, 1f, 1f);
        rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y, 0f);
    }

    public void ResetView()
    {
        cellBackground.enabled = false;
        cellImage.enabled = false;

        animator.Play("Idle");
    }

    public void OnClick()
    {
        cellBackground.color = clickColor;
    }

    public void OnUncklick()
    {
        cellBackground.color = Color.white;
    }

    public void ShowHint()
    {
        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("CellHint"))
        {
            animator.Play("CellHint");
        }
    }

    public void HideHint()
    {
        animator.Play("Idle");
    }
}
