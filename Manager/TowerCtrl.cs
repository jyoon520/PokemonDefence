using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TowerCtrl : MonoBehaviour
{
    // ü��
    int _maxHp;
    int _currentHp;

    // UI
    // ü�¹�
    public Slider hpBar;
    public Text hpText;
    // ���ӿ���
    public GameObject gameOverUI;

    void Start()
    {
        // �ʱ갪
        // ü��
        _maxHp = 100000;
        _currentHp = _maxHp;
    }

    void Update()
    {
        // HpBar ������Ʈ
        UpdateHp();

        // _currentHp�� 0 ���϶�� ���
        if (_currentHp <= 0)
        {
            GameOver();
        }
    }

    // HpBar ������Ʈ
    void UpdateHp()
    {
        hpBar.value = _currentHp;
        hpBar.maxValue = _maxHp;

        hpText.text = $"{_currentHp} / {_maxHp}";
    }

    // Enemy, EnemyBullet���κ��� ������ �Դ� �Լ�
    public void GetDamage(int enemyDamage)
    {
        _currentHp -= enemyDamage;
    }

    // ���ӿ��� ó��
    void GameOver()
    {
        // ���� �Ͻ�����
        Time.timeScale = 0;
        gameOverUI.SetActive(true);
    }

    // ����� ��ư Ŭ�� �� ���� �����
    // RestartBtn OnClick�� ����
    public void OnRestartBtnClick()
    {
        gameOverUI.SetActive(false);

        // ���� �� ��ε�
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
