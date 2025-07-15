using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int gold;
    public float currentSpeed; // ���� ���� �ӵ�
    public Text goldText;
    public Text timeText; // ���� �ӵ� �ؽ�Ʈ
    public Text timer;
    public Button timeButton; // ���� �ӵ� ��ư

    // ����
    // ȯ�漳��
    public Button settingButton; // ȯ�漳�� ��ư
    public Button exitButton; // �ڷΰ��� ��ư
    public Button mainButton; // ó������ ��ư
    public Button restartButton; // �ٽý��� ��ư
    public Button homeButton; // ���ӿ��� Ȩ ��ư
    public GameObject settingCanvas; // ȯ�漳�� ĵ����
    // Ŭ���� â
    public TMP_Text clearTimeText; // �������ھ� (Ŭ���� �ð�)
    public TMP_Text getGoldText; // ���� ��� �ؽ�Ʈ
    public Button clearMainButton; // Ŭ���� �� ���� ��ư
    // ��ũ��Ʈ
    SpawnManager _spawnManager;
    //

    private float time = 0f;
    private float min = 0f;
    private float sec = 0f;
    public string scoreTime;
    void Start()
    {
        #region ����
        // ȯ�漳��
        settingButton.onClick.AddListener(OnSettingButtonClick);
        exitButton.onClick.AddListener(OnExitButtonClick);
        mainButton.onClick.AddListener(OnMainButtonClick);
        restartButton.onClick.AddListener(OnRestartButtonClick);

        // Ŭ���� â
        clearMainButton.onClick.AddListener(OnClearMainButtonClick);

        // ��ũ��Ʈ
        _spawnManager = FindObjectOfType<SpawnManager>();
        #endregion

        _spawnManager.waitingForRepair = true;

        timeButton.onClick.AddListener(OntimeButtonClick);
        homeButton.onClick.AddListener(OnHomeButtonClick);

        // ���� �Ͻ����� ���� (TowerCtrl GameOver ó�� ���)
        Time.timeScale = 1;

        // �ʱ� ���
        gold = 10;
    }

    void Update()
    {
        PrintGold();
        // �ð� üũ
        CheckTime();

        // ���� ����Ǹ� ���� ��� ���
        if (_spawnManager.isFinished)
        {
            // ���ھ� (Ŭ���� �ð�)
            clearTimeText.text = scoreTime;
            // ���� ���
            // õ�ڸ����� �� �� ����ϱ� ����
            string rewardGold = string.Format("{0:n0}", _spawnManager.rewardGold);
            getGoldText.text = $"{rewardGold}";
        }
    }

    void CheckTime()
    {
        // ���� ���� �ƴ� ���� �ð� üũ
        if (!_spawnManager.waitingForRepair)
        {
            sec += Time.deltaTime;
            if (sec >= 60f)
            {
                min += 1f;
                sec = 0f;
            }

            scoreTime = string.Format("{0:D2}:{1:D2}", (int)min, (int)sec);
            timer.text = scoreTime;
        }
        else
        {
            Time.timeScale = 1;
            timeText.text = "1.0x";
        }
    }

    // ���ʹ� ��� �� 50% Ȯ���� ��� ����
    public void AddGold(int goldAmount)
    {
        gold += goldAmount;
    }

    // [UI] ��� ���
    void PrintGold()
    {
        // õ�ڸ����� ���� ����ϱ� ����
        string _gold = string.Format("{0:n0}", gold);
        goldText.text = $"{_gold}";
    }

    public void OntimeButtonClick()
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
                    timeText.text = "2.5x";
                    Time.timeScale = 2.5f;

                    break;
                }
            case 2.5f:
                {
                    timeText.text = "3.0x";
                    Time.timeScale = 3.0f;

                    break;
                }
            case 3.0f:
                {
                    timeText.text = "1.0x";
                    Time.timeScale = 1.0f;

                    break;
                }
        }
    }

    #region ����
    // ȯ�漳��
    // ȯ�漳�� ��ư Ŭ��
    void OnSettingButtonClick()
    {
        currentSpeed = Time.timeScale; // �ֿ� // ���� �ӵ� ����
        // �Ͻ�����
        Time.timeScale = 0;

        settingCanvas.SetActive(true);
    }

    // �ڷΰ��� ��ư Ŭ��
    void OnExitButtonClick()
    {
        // �Ͻ����� ����
        Time.timeScale = currentSpeed; // �ֿ� // ���� �ӵ��� ����

        settingCanvas.SetActive(false);
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

    // Ŭ���� â
    void OnClearMainButtonClick()
    {
        // �κ�� �ε�
        SceneManager.LoadScene(Define.Lobby);
    }
    #endregion

    // ���ӿ��� â
    void OnHomeButtonClick()
    {
        SceneManager.LoadScene("Lobby");
    }
}
