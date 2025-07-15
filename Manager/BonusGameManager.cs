using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BonusGameManager : MonoBehaviour
{
    // 스크립트
    BonusEnemy _bonusEnemy;
    LobbyManager _lobbyManager;

    // UI
    // 속도
    public Text timeText; // 게임 속도 텍스트
    public Button timeButton; // 게임 속도 버튼
    // 골드
    public Text goldText; // 현재 로비 골드
    public TMP_Text finishedGoldText; // 획득 골드
    Vector3 _originGoldIconSize;
    Vector3 _originGoldTextSize;
    int _finishedGold;
    int _beforeGold;
    int _afterGold;
    // 게임오버
    public GameObject gameOverUI;
    public Button gameoverRestartButton; // 재시작 버튼
    public Button gameoverMainButton; // 처음으로 버튼
    // 환경설정
    public Button settingButton; // 환경설정 버튼
    public Button exitButton; // 환경설정 나가기 버튼
    public Button mainButton; // 처음으로 버튼
    public Button restartButton; // 다시하기 버튼
    public GameObject settingUI; // 환경설정 창

    // 보너스에너미 프리팹
    public GameObject bonusEnemyPrefab; // bonusEnemy
    public Transform spawnPoint; // 스폰 위치

    void Start()
    {
        // 일시정지 해제 (씬 재시작할 때)
        Time.timeScale = 1;


        // 에너미 스폰
        Instantiate(bonusEnemyPrefab, spawnPoint.position, Quaternion.identity);

        // 스크립트
        _bonusEnemy = FindObjectOfType<BonusEnemy>();
        _lobbyManager = FindObjectOfType<LobbyManager>();

        // 골드
        _beforeGold = _lobbyManager.gold;

        // 게임오버 후 다시 시작/메인 버튼
        gameoverRestartButton.onClick.AddListener(OnGameoverRestartButtonClick);
        gameoverMainButton.onClick.AddListener(OnGameoverMainButtonClick);

        // 게임 속도 버튼
        timeButton.onClick.AddListener(OntimeButtonClick);
        // 환경설정 버튼
        settingButton.onClick.AddListener(OnSettingButtonClick);
        // 환경설정창 백버튼
        exitButton.onClick.AddListener(OnExitButtonClick);
        // 처음으로 버튼
        mainButton.onClick.AddListener(OnMainButtonClick);
        // 다시하기 버튼
        restartButton.onClick.AddListener(OnRestartButtonClick);

        // 골드 텍스트 스케일 저장
        _originGoldTextSize = goldText.transform.localScale;
    }

    void Update()
    {
        // 보너스에너미 사망 시 게임오버
        if (!_bonusEnemy.isLive)
        {
            _afterGold = _lobbyManager.gold;
            // 골드 저장
            PlayerPrefs.SetInt("Gold", _afterGold);
            gameOverUI.SetActive(true);
            PrintGoldFinished();
        }

        // 골드 출력 (로비 골드)
        PrintGold();
    }

    // 스테이지 클리어 후 획득 골드 출력
    void PrintGoldFinished()
    {
        _finishedGold = _afterGold - _beforeGold;

        // 천자리마다 점찍어서 출력하기 위함
        string _printGold = string.Format("{0:n0}", _finishedGold);
        finishedGoldText.text = $"{_printGold}";
    }

    // 골드텍스트 줌인아웃
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

    // 게임 속도 조절
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

    // 환경설정 버튼 클릭
    void OnSettingButtonClick()
    {
        // 일시정지
        Time.timeScale = 0;
        // 환경설정 창 활성화
        settingUI.SetActive(true);
    }

    // 환경설정창 백버튼 클릭
    void OnExitButtonClick()
    {
        // 일시정지 해제
        Time.timeScale = 1;
        // 환경설정 창 비활성화
        settingUI.SetActive(false);
    }

    // [UI] 골드 출력
    void PrintGold()
    {
        // 천자리마다 점찍어서 출력하기 위함
        string _gold = string.Format("{0:n0}", _lobbyManager.gold);
        goldText.text = $"{_gold}";
    }

    // 게임오버 후 재시작
    public void OnGameoverRestartButtonClick()
    {
        // 현재 씬 재로드
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // 게임오버 후 메인으로
    public void OnGameoverMainButtonClick()
    {
        SceneManager.LoadScene(Define.Lobby);
    }

    // 처음으로 버튼 클릭
    void OnMainButtonClick()
    {
        // 일시정지 해제
        Time.timeScale = 1;

        SceneManager.LoadScene(Define.Lobby);
    }

    // 다시시작 버튼 클릭
    void OnRestartButtonClick()
    {
        // 현재 씬 재로드
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
