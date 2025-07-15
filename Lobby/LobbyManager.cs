using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

public class LobbyManager : MonoBehaviour
{
    [Header("������Ʈ")]
    public GameObject mainUI;
    public GameObject lottoUI;
    public GameObject lottoResultUI;
    public GameObject unitDictionaryUI;
    public GameObject unitUpgradeUI;
    public GameObject ShopUI;
    public GameObject cardSlotPrefab; // ī�� UI ������ (�̹���+�̸� ��)
    public GameObject settingUI; // ���� // ȯ�漳��
    public GameObject paymentFailedUI; // ���� // ���� ���� �˾�

    [Header("��ġ")]
    public Transform resultPanelParent;

    [Header("����")]
    public RandomCardSelect randomCardSelect; // ī�� ���� ��ũ��Ʈ
    public UnitUpGrade unitUpGrade;
    public CardDexManager cardDexManager;

    [Header("��ư")]
    public Button gameStartButton;
    public Button shopButton;
    public Button cardButton;
    public Button lottoButton;
    public Button backToLobbyButton;
    public Button unityDictionaryButton;
    public Button questButton;
    public Button unitUpgradeButton;
    public Button settingButton;
    public Button exitButton; // ���� // ȯ�漳�� �ڷΰ��� ��ư
    public Button[] buyButton; // ���� // ���� ���� ��ư
    public Button paymentFailedButton; // ���� // ���� ���� Ȯ�� ��ư
    public Button yesExitButton; // �ֿ� // ���� �� ��ư
    public Button noExitButton; // �ֿ� // ���� �ƴϿ� ��ư

    [Header("��í��ư")]
    public Button articunoPanelButton;
    public Button groundonPanelButton;
    public Button mewtwoPanelButton;

    [Header("��íUI")]
    public GameObject articunoUI;
    public GameObject groundonUI;
    public GameObject mewtwoUI;
    public Button backButton; // ���� // ��í �ڷΰ��� ��ư

    [Header("�ؽ�Ʈ")]
    public Text crystalText;
    public Text questText;
    public Text goldText;

    [Header("�ʱⰪ")]
    public int crystal = 100; // ũ����Ż �ʱ� ��
    public int stage = 1;
    public int gold = 50000; // ��� �ʱ� ��

    void Awake()
    {
        LoadProgress();
        // ����
        Time.timeScale = 1f;

        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == Define.Stage_01) return;
        if (sceneName == Define.Stage_02) return;
        if (sceneName == Define.Stage_03) return;
        if (sceneName == Define.BonusStage) return;

        // ����Ʈ ��ư �⺻ ��Ȱ��ȭ
        questButton.interactable = false;

        // ��í �ڷΰ���
        backButton.onClick.AddListener(OnBackButtonClick);
        // ȯ�漳��
        settingButton.onClick.AddListener(OnSettingButtonClick);
        // ȯ�漳�� �ڷΰ���
        exitButton.onClick.AddListener(OnExitButtonClick);
        // ���� ����
        for (int i = 0; i < buyButton.Length; i++)
        {
            buyButton[i].onClick.AddListener(OnBuyButtonClick);
        }
        // ���� ���� Ȯ��
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
            questText.text = $"����Ʈ�� �����ϴ�.";
            questButton.interactable = false;
        }
        else
        {
            questText.text = $"�������� {stage} Ŭ����";
        }

        UpdateCrystalText();
        UpdateGoldText();

        // ���� ���� �� ���� Ŭ���� ���������� ���� ��쿡�� ����Ʈ ��ư Ȱ��ȭ
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
                questText.text = $"����Ʈ�� �����ϴ�.";
                questButton.interactable = false;
            }
            else
            {
                questText.text = $"�������� {stage} Ŭ����";
            }
        }
    }

    void OnGameStartButtonClick()
    {
        randomCardSelect.SaveUnitData(); // ����
        SaveProgress();                  // ���/ũ����Ż ����
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
        // ���� �� �������� Ŭ�������� Ȯ��
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

        questText.text = $"�������� {stage} Ŭ����";
        UpdateCrystalText();
        SaveProgress();

        // ���� �ް� ���� ��ư ��Ȱ��ȭ
        questButton.interactable = false;
    }

    public void UpdateCrystalText()
    {
        crystalText.text = $"{crystal}";
    }

    public void UpdateGoldText()
    {
        // õ�ڸ����� ���� ����ϱ� ����
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
        // ���� UI Ŭ����
        foreach (Transform child in resultPanelParent)
        {
            Destroy(child.gameObject);
        }

        // ��� �������� �� ���� ��� �������� ID �������� ����
        var sortedCards = randomCardSelect.result
            .OrderBy(card => (int)card.cardGrade)  // ��� �� (SSS=1, AA=2, B=3 ��)
            .ThenBy(card => card.id)               // ���� ��� �� ID�� ����
            .ToList();

        foreach (var card in sortedCards)  // ���ĵ� ����Ʈ ���!
        {
            GameObject slot = Instantiate(cardSlotPrefab, resultPanelParent);
            CardSlotUI slotUI = slot.GetComponent<CardSlotUI>();
            slotUI.Setup(card, true);
        }

        randomCardSelect.SaveUnitData();
        // UI ���� ��ȯ
        mainUI.SetActive(false);
        unitDictionaryUI.SetActive(true);
        lottoUI.SetActive(false);
        ShopUI.SetActive(false);
    }

    // ��í UI
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


    // ��ư Ŀ���ٰ� �ٽ� �پ��� �ݺ���
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

    // ����
    // ��í �ڷΰ��� ��ư
    void OnBackButtonClick()
    {
        lottoUI.SetActive(false);
        mainUI.SetActive(true);
    }

    // ����
    // ȯ�漳�� ��ư
    void OnSettingButtonClick()
    {
        settingUI.SetActive(true);
    }

    // ����
    // ȯ�漳�� �ڷΰ��� ��ư
    void OnExitButtonClick()
    {
        settingUI.SetActive(false);
    }

    // ����
    // ���� ��ư
    void OnBuyButtonClick()
    {
        paymentFailedUI.SetActive(true);
    }

    // ����
    // ���� ���� Ȯ�� ��ư
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
            Application.Quit(); // ���ø����̼� ����
#endif
    }

    void OnNoExitButtonClick()
    {
        settingUI.SetActive(false);
    }
}
