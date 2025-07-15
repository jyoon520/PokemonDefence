using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BonusGameManager : MonoBehaviour
{
    // ��ũ��Ʈ
    BonusEnemy _bonusEnemy;
    LobbyManager _lobbyManager;

    // UI
    // �ӵ�
    public Text timeText; // ���� �ӵ� �ؽ�Ʈ
    public Button timeButton; // ���� �ӵ� ��ư
    // ���
    public Text goldText; // ���� �κ� ���
    public TMP_Text finishedGoldText; // ȹ�� ���
    Vector3 _originGoldIconSize;
    Vector3 _originGoldTextSize;
    int _finishedGold;
    int _beforeGold;
    int _afterGold;
    // ���ӿ���
    public GameObject gameOverUI;
    public Button gameoverRestartButton; // ����� ��ư
    public Button gameoverMainButton; // ó������ ��ư
    // ȯ�漳��
    public Button settingButton; // ȯ�漳�� ��ư
    public Button exitButton; // ȯ�漳�� ������ ��ư
    public Button mainButton; // ó������ ��ư
    public Button restartButton; // �ٽ��ϱ� ��ư
    public GameObject settingUI; // ȯ�漳�� â

    // ���ʽ����ʹ� ������
    public GameObject bonusEnemyPrefab; // bonusEnemy
    public Transform spawnPoint; // ���� ��ġ

    void Start()
    {
        // �Ͻ����� ���� (�� ������� ��)
        Time.timeScale = 1;


        // ���ʹ� ����
        Instantiate(bonusEnemyPrefab, spawnPoint.position, Quaternion.identity);

        // ��ũ��Ʈ
        _bonusEnemy = FindObjectOfType<BonusEnemy>();
        _lobbyManager = FindObjectOfType<LobbyManager>();

        // ���
        _beforeGold = _lobbyManager.gold;

        // ���ӿ��� �� �ٽ� ����/���� ��ư
        gameoverRestartButton.onClick.AddListener(OnGameoverRestartButtonClick);
        gameoverMainButton.onClick.AddListener(OnGameoverMainButtonClick);

        // ���� �ӵ� ��ư
        timeButton.onClick.AddListener(OntimeButtonClick);
        // ȯ�漳�� ��ư
        settingButton.onClick.AddListener(OnSettingButtonClick);
        // ȯ�漳��â ���ư
        exitButton.onClick.AddListener(OnExitButtonClick);
        // ó������ ��ư
        mainButton.onClick.AddListener(OnMainButtonClick);
        // �ٽ��ϱ� ��ư
        restartButton.onClick.AddListener(OnRestartButtonClick);

        // ��� �ؽ�Ʈ ������ ����
        _originGoldTextSize = goldText.transform.localScale;
    }

    void Update()
    {
        // ���ʽ����ʹ� ��� �� ���ӿ���
        if (!_bonusEnemy.isLive)
        {
            _afterGold = _lobbyManager.gold;
            // ��� ����
            PlayerPrefs.SetInt("Gold", _afterGold);
            gameOverUI.SetActive(true);
            PrintGoldFinished();
        }

        // ��� ��� (�κ� ���)
        PrintGold();
    }

    // �������� Ŭ���� �� ȹ�� ��� ���
    void PrintGoldFinished()
    {
        _finishedGold = _afterGold - _beforeGold;

        // õ�ڸ����� ���� ����ϱ� ����
        string _printGold = string.Format("{0:n0}", _finishedGold);
        finishedGoldText.text = $"{_printGold}";
    }

    // ����ؽ�Ʈ ���ξƿ�
    public void GoldTextZoomInOut()
    {
        goldText.transform.localScale = new Vector3(goldText.transform.localScale.x + 0.3f, goldText.transform.localScale.y + 0.3f, goldText.transform.localScale.z);

        StartCoroutine(CoZoomOut());

        IEnumerator CoZoomOut()
        {
            yield return new WaitForSeconds(0.1f);

            goldText.transform.localScale = _originGoldTextSize;
        }
    }

    // ���� �ӵ� ����
    void OntimeButtonClick()
    {
        float speed = Time.timeScale;

        switch (speed)
        {
            case 1.0f:
                {
                    timeText.text = "1.5x";
                    Time.timeScale = 1.5f;

                    break;
                }
            case 1.5f:
                {
                    timeText.text = "2.0x";
                    Time.timeScale = 2f;

                    break;
                }
            case 2.0f:
                {
                    timeText.text = "4.0x";
                    Time.timeScale = 4.0f;

                    break;
                }
            case 4.0f:
                {
                    timeText.text = "8.0x";
                    Time.timeScale = 8.0f;

                    break;
                }
            case 8.0f:
                {
                    timeText.text = "1.0x";
                    Time.timeScale = 1.0f;

                    break;
                }
        }
    }

    // ȯ�漳�� ��ư Ŭ��
    void OnSettingButtonClick()
    {
        // �Ͻ�����
        Time.timeScale = 0;
        // ȯ�漳�� â Ȱ��ȭ
        settingUI.SetActive(true);
    }

    // ȯ�漳��â ���ư Ŭ��
    void OnExitButtonClick()
    {
        // �Ͻ����� ����
        Time.timeScale = 1;
        // ȯ�漳�� â ��Ȱ��ȭ
        settingUI.SetActive(false);
    }

    // [UI] ��� ���
    void PrintGold()
    {
        // õ�ڸ����� ���� ����ϱ� ����
        string _gold = string.Format("{0:n0}", _lobbyManager.gold);
        goldText.text = $"{_gold}";
    }

    // ���ӿ��� �� �����
    public void OnGameoverRestartButtonClick()
    {
        // ���� �� ��ε�
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // ���ӿ��� �� ��������
    public void OnGameoverMainButtonClick()
    {
        SceneManager.LoadScene(Define.Lobby);
    }

    // ó������ ��ư Ŭ��
    void OnMainButtonClick()
    {
        // �Ͻ����� ����
        Time.timeScale = 1;

        SceneManager.LoadScene(Define.Lobby);
    }

    // �ٽý��� ��ư Ŭ��
    void OnRestartButtonClick()
    {
        // ���� �� ��ε�
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
