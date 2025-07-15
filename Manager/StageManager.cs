using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public Button stage1EnterButton;
    public Button stage2EnterButton;
    public Button stage3EnterButton;
    public Button bonusStageEnterButton;
    public Button backToLobbyButton;
    void Start()
    {
        stage1EnterButton.onClick.AddListener(OnStage1EnterButtonClick);
        stage2EnterButton.onClick.AddListener(OnStage2EnterButtonClick);
        stage3EnterButton.onClick.AddListener(OnStage3EnterButtonClick);
        bonusStageEnterButton.onClick.AddListener(OnBonusStageEnterButtonClick);
        backToLobbyButton.onClick.AddListener(OnBackToLobbyButtonClick);
    }

    void OnStage1EnterButtonClick()
    {
        SceneManager.LoadScene("Stage_01");
    }
    void OnStage2EnterButtonClick()
    {
        SceneManager.LoadScene("Stage_02");
    }
    void OnStage3EnterButtonClick()
    {
        SceneManager.LoadScene("Stage_03");
    }

    void OnBonusStageEnterButtonClick()
    {
        SceneManager.LoadScene("BonusStage");
    }

    void OnBackToLobbyButtonClick()
    {
        SceneManager.LoadScene("Lobby");
    }
}
