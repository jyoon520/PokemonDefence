using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int gold;
    public float currentSpeed; // 게임 현재 속도
    public Text goldText;
    public Text timeText; // 게임 속도 텍스트
    public Text timer;
    public Button timeButton; // 게임 속도 버튼

    // 정인
    // 환경설정
    public Button settingButton; // 환경설정 버튼
    public Button exitButton; // 뒤로가기 버튼
    public Button mainButton; // 처음으로 버튼
    public Button restartButton; // 다시시작 버튼
    public Button homeButton; // 게임오버 홈 버튼
    public GameObject settingCanvas; // 환경설정 캔버스
    // 클리어 창
    public TMP_Text clearTimeText; // 최종스코어 (클리어 시간)
    public TMP_Text getGoldText; // 보상 골드 텍스트
    public Button clearMainButton; // 클리어 후 메인 버튼
    // 스크립트
    SpawnManager _spawnManager;
    //

    private float time = 0f;
    private float min = 0f;
    private float sec = 0f;
    public string scoreTime;
    void Start()
    {
        #region 정인
        // 환경설정
        settingButton.onClick.AddListener(OnSettingButtonClick);
        exitButton.onClick.AddListener(OnExitButtonClick);
        mainButton.onClick.AddListener(OnMainButtonClick);
        restartButton.onClick.AddListener(OnRestartButtonClick);

        // 클리어 창
        clearMainButton.onClick.AddListener(OnClearMainButtonClick);

        // 스크립트
        _spawnManager = FindObjectOfType<SpawnManager>();
        #endregion

        _spawnManager.waitingForRepair = true;

        timeButton.onClick.AddListener(OntimeButtonClick);
        homeButton.onClick.AddListener(OnHomeButtonClick);

        // 게임 일시정지 해제 (TowerCtrl GameOver 처리 고려)
        Time.timeScale = 1;

        // 초기 골드
        gold = 10;
    }

    void Update()
    {
        PrintGold();
        // 시간 체크
        CheckTime();

        // 게임 종료되면 최종 결과 출력
        if (_spawnManager.isFinished)
        {
            // 스코어 (클리어 시간)
            clearTimeText.text = scoreTime;
            // 보상 골드
            // 천자리마다 점 찍어서 출력하기 위함
            string rewardGold = string.Format("{0:n0}", _spawnManager.rewardGold);
            getGoldText.text = $"{rewardGold}";
        }
    }

    void CheckTime()
    {
        // 정비 중이 아닐 때만 시간 체크
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

    // 에너미 사망 시 50% 확률로 골드 지급
    public void AddGold(int goldAmount)
    {
        gold += goldAmount;
    }

    // [UI] 골드 출력
    void PrintGold()
    {
        // 천자리마다 점찍어서 출력하기 위함
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

    #region 정인
    // 환경설정
    // 환경설정 버튼 클릭
    void OnSettingButtonClick()
    {
        currentSpeed = Time.timeScale; // 주연 // 현재 속도 저장
        // 일시정지
        Time.timeScale = 0;

        settingCanvas.SetActive(true);
    }

    // 뒤로가기 버튼 클릭
    void OnExitButtonClick()
    {
        // 일시정지 해제
        Time.timeScale = currentSpeed; // 주연 // 원래 속도로 시작

        settingCanvas.SetActive(false);
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

    // 클리어 창
    void OnClearMainButtonClick()
    {
        // 로비씬 로드
        SceneManager.LoadScene(Define.Lobby);
    }
    #endregion

    // 게임오버 창
    void OnHomeButtonClick()
    {
        SceneManager.LoadScene("Lobby");
    }
}
