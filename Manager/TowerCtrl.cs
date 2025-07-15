using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TowerCtrl : MonoBehaviour
{
    // 체력
    int _maxHp;
    int _currentHp;

    // UI
    // 체력바
    public Slider hpBar;
    public Text hpText;
    // 게임오버
    public GameObject gameOverUI;

    void Start()
    {
        // 초깃값
        // 체력
        _maxHp = 100000;
        _currentHp = _maxHp;
    }

    void Update()
    {
        // HpBar 업데이트
        UpdateHp();

        // _currentHp가 0 이하라면 사망
        if (_currentHp <= 0)
        {
            GameOver();
        }
    }

    // HpBar 업데이트
    void UpdateHp()
    {
        hpBar.value = _currentHp;
        hpBar.maxValue = _maxHp;

        hpText.text = $"{_currentHp} / {_maxHp}";
    }

    // Enemy, EnemyBullet으로부터 데미지 입는 함수
    public void GetDamage(int enemyDamage)
    {
        _currentHp -= enemyDamage;
    }

    // 게임오버 처리
    void GameOver()
    {
        // 게임 일시정지
        Time.timeScale = 0;
        gameOverUI.SetActive(true);
    }

    // 재시작 버튼 클릭 시 게임 재시작
    // RestartBtn OnClick에 연결
    public void OnRestartBtnClick()
    {
        gameOverUI.SetActive(false);

        // 현재 씬 재로드
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
