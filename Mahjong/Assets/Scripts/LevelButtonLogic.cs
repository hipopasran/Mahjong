using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LevelInfo), typeof(Button))]
public class LevelButtonLogic : MonoBehaviour
{
    private LevelInfo levelInfo;
    private Button levelButton;

    private void Awake()
    {
        levelInfo = GetComponent<LevelInfo>();
        levelButton = GetComponent<Button>();
    }

    private void Start()
    {
        levelButton.onClick.AddListener(DoAfterButtonClick);
    }

    private void DoAfterButtonClick()
    {
        GameSessionManager.Instance.OpenLevel(levelInfo.Data, levelInfo.LevelName);
    }
}
