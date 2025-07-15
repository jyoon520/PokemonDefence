using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

public class LobbyManager : MonoBehaviour
{
    [Header("오브젝트")]
    public GameObject mainUI;
    public GameObject lottoUI;
    public GameObject lottoResultUI;
    public GameObject unitDictionaryUI;
    public GameObject unitUpgradeUI;
    public GameObject ShopUI;
    public GameObject cardSlotPrefab; // 카드 UI 프리팹 (이미지+이름 등)
    public GameObject settingUI; // 정인 // 환경설정
    public GameObject paymentFailedUI; // 정인 // 구매 실패 팝업

    [Header("위치")]
    public Transform resultPanelParent;

    [Header("참조")]
    public RandomCardSelect randomCardSelect; // 카드 선택 스크립트
    public UnitUpGrade unitUpGrade;
    public CardDexManager cardDexManager;

    [Header("버튼")]
    public Button gameStartButton;
    public Button shopButton;
    public Button cardButton;
    public Button lottoButton;
    public Button backToLobbyButton;
    public Button unityDictionaryButton;
    public Button questButton;
    public Button unitUpgradeButton;
    public Button settingButton;
    public Button exitButton; // 정인 // 환경설정 뒤로가기 버튼
    public Button[] buyButton; // 정인 // 상점 구매 버튼
    public Button paymentFailedButton; // 정인 // 구매 실패 확인 버튼
    public Button yesExitButton; // 주연 // 설정 네 버튼
    public Button noExitButton; // 주연 // 설정 아니요 버튼

    [Header("가챠버튼")]
    public Button articunoPanelButton;
    public Button groundonPanelButton;
    public Button mewtwoPanelButton;

    [Header("가챠UI")]
    public GameObject articunoUI;
    public GameObject groundonUI;
    public GameObject mewtwoUI;
    public Button backButton; // 정인 // 가챠 뒤로가기 버튼

    [Header("텍스트")]
    public Text crystalText;
    public Text questText;
    public Text goldText;

    [Header("초기값")]
    public int crystal = 100; // 크리스탈 초기 값
    public int stage = 1;
    public int gold = 50000; // 골드 초기 값

    void Awake()
    {
        LoadProgress();
        // 정인
        Time.timeScale = 1f;

        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == Define.Stage_01) return;
        if (sceneName == Define.Stage_02) return;
        if (sceneName == Define.Stage_03) return;
        if (sceneName == Define.BonusStage) return;

        // 퀘스트 버튼 기본 비활성화
        questButton.interactable = false;

        // 가챠 뒤로가기
        backButton.onClick.AddListener(OnBackButtonClick);
        // 환경설정
        settingButton.onClick.AddListener(OnSettingButtonClick);
        // 환경설정 뒤로가기
        exitButton.onClick.AddListener(OnExitButtonClick);
        // 상점 구매
        for (int i = 0; i < buyButton.Length; i++)
        {
            buyButton[i].onClick.AddListener(OnBuyButtonClick);
        }
        // 구매 실패 확인
        paymentFailedButton.onClick.AddListener(OnPaymentFailedButtonClick);
        //

        gameStartButton.onClick.AddListener(OnGameStartButtonClick);
        shopButton.onClick.AddListener(OnShopButtonClick);
        lottoButton.onClick.AddListener(OnLottoButtonClick);
        backToLobbyButton.onClick.AddListener(OnBackToLobbyButtonClick);
        unityDictionaryButton.onClick.AddListener(OnUnityDictionaryButtonClick);
        questButton.onClick.AddListener(OnQuestButtonClick);
        unitUpgradeButton.onClick.AddListener(OnUnitUpgradeButtonClick);
        articunoPanelButton.onClick.AddListener(OnArticunoPanelButtonClick);
        groundonPanelButton.onClick.AddListener(OnGroundonPanelButtonClick);
        mewtwoPanelButton.onClick.AddListener(OnMewtwoPanelButtonClick);
        yesExitButton.onClick.AddListener(OnYesExitButtonClick);
        noExitButton.onClick.AddListener(OnNoExitButtonClick);

        if (stage > 3)
        {
            questText.text = $"퀘스트가 없습니다.";
            questButton.interactable = false;
        }
        else
        {
            questText.text = $"스테이지 {stage} 클리어";
        }

        UpdateCrystalText();
        UpdateGoldText();

        // 보상 아직 안 받은 클리어 스테이지가 있을 경우에만 퀘스트 버튼 활성화
        if ((PlayerPrefs.GetInt("Stage1Clear", 0) == 1 && PlayerPrefs.GetInt("Stage1Rewarded", 0) == 0) ||
            (PlayerPrefs.GetInt("Stage2Clear", 0) == 1 && PlayerPrefs.GetInt("Stage2Rewarded", 0) == 0) ||
            (PlayerPrefs.GetInt("Stage3Clear", 0) == 1 && PlayerPrefs.GetInt("Stage3Rewarded", 0) == 0))
        {
            questButton.interactable = true;
        }
        else
        {
            questButton.interactable = false;
        }
    }

    private void Update()
    {
        if (questText != null)
        {
            if (stage > 3)
            {
                questText.text = $"퀘스트가 없습니다.";
                questButton.interactable = false;
            }
            else
            {
                questText.text = $"스테이지 {stage} 클리어";
            }
        }
    }

    void OnGameStartButtonClick()
    {
        randomCardSelect.SaveUnitData(); // 저장
        SaveProgress();                  // 골드/크리스탈 저장
        SceneManager.LoadScene("StageSelect");
    }

    void OnShopButtonClick()
    {
        ShopUI.SetActive(true);
        lottoResultUI.SetActive(false);
        unitDictionaryUI.SetActive(false);
        unitUpgradeUI.SetActive(false);
    }
    void OnLottoButtonClick()
    {
        mainUI.SetActive(false);
        lottoUI.SetActive(true);
        articunoUI.SetActive(true);
    }

    public void OnBackToLobbyButtonClick()
    {
        mainUI.SetActive(true);
        lottoResultUI.SetActive(false);
        unitDictionaryUI.SetActive(false);
        ShopUI.SetActive(false);
        unitUpgradeUI.SetActive(false);
    }

    public void OnQuestButtonClick()
    {
        // 현재 몇 스테이지 클리어인지 확인
        if (PlayerPrefs.GetInt("Stage1Clear", 0) == 1 && PlayerPrefs.GetInt("Stage1Rewarded", 0) == 0)
        {
            PlayerPrefs.SetInt("Stage1Rewarded", 1);
        }
        else if (PlayerPrefs.GetInt("Stage2Clear", 0) == 1 && PlayerPrefs.GetInt("Stage2Rewarded", 0) == 0)
        {
            PlayerPrefs.SetInt("Stage2Rewarded", 1);
        }
        else if (PlayerPrefs.GetInt("Stage3Clear", 0) == 1 && PlayerPrefs.GetInt("Stage3Rewarded", 0) == 0)
        {
            PlayerPrefs.SetInt("Stage3Rewarded", 1);
        }

        stage++;
        crystal += 5;

        questText.text = $"스테이지 {stage} 클리어";
        UpdateCrystalText();
        SaveProgress();

        // 보상 받고 나면 버튼 비활성화
        questButton.interactable = false;
    }

    public void UpdateCrystalText()
    {
        crystalText.text = $"{crystal}";
    }

    public void UpdateGoldText()
    {
        // 천자리마다 점찍어서 출력하기 위함
        string _gold = string.Format("{0:n0}", gold);
        goldText.text = $"{_gold}";
    }

    public void OnUnitUpgradeButtonClick()
    {
        unitUpGrade.CountDuplicationCards();
        unitUpGrade.UpdateUnitLevel();
        unitUpgradeUI.SetActive(true);
        lottoResultUI.SetActive(false);
        unitDictionaryUI.SetActive(false);
        ShopUI.SetActive(false);
    }

    public void OnUnityDictionaryButtonClick()
    {
        // 기존 UI 클리어
        foreach (Transform child in resultPanelParent)
        {
            Destroy(child.gameObject);
        }

        // 등급 오름차순 → 같은 등급 내에서는 ID 오름차순 정렬
        var sortedCards = randomCardSelect.result
            .OrderBy(card => (int)card.cardGrade)  // 등급 순 (SSS=1, AA=2, B=3 등)
            .ThenBy(card => card.id)               // 같은 등급 내 ID순 정렬
            .ToList();

        foreach (var card in sortedCards)  // 정렬된 리스트 사용!
        {
            GameObject slot = Instantiate(cardSlotPrefab, resultPanelParent);
            CardSlotUI slotUI = slot.GetComponent<CardSlotUI>();
            slotUI.Setup(card, true);
        }

        randomCardSelect.SaveUnitData();
        // UI 상태 전환
        mainUI.SetActive(false);
        unitDictionaryUI.SetActive(true);
        lottoUI.SetActive(false);
        ShopUI.SetActive(false);
    }

    // 가챠 UI
    void OnArticunoPanelButtonClick()
    {
        StartCoroutine(ScaleButton(articunoPanelButton));
        articunoUI.SetActive(true);

        groundonUI.SetActive(false);
        mewtwoUI.SetActive(false);
        ShopUI.SetActive(false);
        lottoResultUI.SetActive(false);
        unitDictionaryUI.SetActive(false);
        unitUpgradeUI.SetActive(false);
        mainUI.SetActive(false);
    }

    void OnGroundonPanelButtonClick()
    {
        StartCoroutine(ScaleButton(groundonPanelButton));
        groundonUI.SetActive(true);

        articunoUI.SetActive(false);
        mewtwoUI.SetActive(false);
        ShopUI.SetActive(false);
        lottoResultUI.SetActive(false);
        unitDictionaryUI.SetActive(false);
        unitUpgradeUI.SetActive(false);
        mainUI.SetActive(false);
    }

    void OnMewtwoPanelButtonClick()
    {
        StartCoroutine(ScaleButton(mewtwoPanelButton));
        mewtwoUI.SetActive(true);

        groundonUI.SetActive(false);
        articunoUI.SetActive(false);
        ShopUI.SetActive(false);
        lottoResultUI.SetActive(false);
        unitDictionaryUI.SetActive(false);
        unitUpgradeUI.SetActive(false);
        mainUI.SetActive(false);
    }


    // 버튼 커졌다가 다시 줄어드는 반복문
    IEnumerator ScaleButton(Button button)
    {
        Transform target = button.transform;
        Vector3 originalScale = Vector3.one;
        Vector3 targetScale = originalScale * 1.1f;

        float duration = 0.1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            target.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        target.localScale = targetScale;

        elapsed = 0f;
        while (elapsed < duration)
        {
            target.localScale = Vector3.Lerp(targetScale, originalScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        target.localScale = originalScale;
    }

    // 정인
    // 가챠 뒤로가기 버튼
    void OnBackButtonClick()
    {
        lottoUI.SetActive(false);
        mainUI.SetActive(true);
    }

    // 정인
    // 환경설정 버튼
    void OnSettingButtonClick()
    {
        settingUI.SetActive(true);
    }

    // 정인
    // 환경설정 뒤로가기 버튼
    void OnExitButtonClick()
    {
        settingUI.SetActive(false);
    }

    // 정인
    // 구매 버튼
    void OnBuyButtonClick()
    {
        paymentFailedUI.SetActive(true);
    }

    // 정인
    // 구매 실패 확인 버튼
    void OnPaymentFailedButtonClick()
    {
        paymentFailedUI.SetActive(false);
    }

    public void SaveProgress()
    {
        PlayerPrefs.SetInt("Gold", gold);
        PlayerPrefs.SetInt("Crystal", crystal);
        PlayerPrefs.SetInt("Stage", stage);
    }

    private void LoadProgress()
    {
        gold = PlayerPrefs.GetInt("Gold", 100000);
        crystal = PlayerPrefs.GetInt("Crystal", 100);
        stage = PlayerPrefs.GetInt("Stage", stage);

    }

    public RandomCard GetCardById(int id)
    {
        return randomCardSelect.deck.Find(card => card.id == id);
    }

    void OnYesExitButtonClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit(); // 어플리케이션 종료
#endif
    }

    void OnNoExitButtonClick()
    {
        settingUI.SetActive(false);
    }
}
