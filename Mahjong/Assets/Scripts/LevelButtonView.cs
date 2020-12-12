using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LevelInfo))]
public class LevelButtonView : MonoBehaviour
{
    [SerializeField] private Text buttonText;
    [SerializeField] private LevelInfo levelInfo;

    [SerializeField] private List<GameObject> stars = new List<GameObject>();

    private void Start()
    {
        levelInfo = GetComponent<LevelInfo>();
        buttonText = GetComponentInChildren<Text>();

        GenerateView();
    }

    private void GenerateView()
    {
        if (buttonText != null)
        {
            buttonText.text = levelInfo.LevelName;
        }

        if (levelInfo.IsPlayed)
        {
            foreach (var item in stars)
            {
                item.SetActive(true);
            }
        }
        else
        {
            foreach (var item in stars)
            {
                item.SetActive(false);
            }
        }
    }
}
