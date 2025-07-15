using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    // ��ũ��Ʈ
    EnemyCtrl _enemyCtrl;
    GameManager _gameManager;
    LobbyManager _lobbyManager;
    RepairCradDex _repairCardDex;

    // ���ʹ�
    // ���ʹ� ������
    public GameObject enemyPrefab_01; // Enemy_01
    public GameObject enemyPrefab_02; // Enemy_02
    public GameObject shieldEnemyPrefab_01; // ShieldEnemy_01
    public GameObject shieldEnemyPrefab_02; // ShieldEnemy_02
    public GameObject bossEnemyPrefab_01; // BossEnemy_01
    public GameObject bossEnemyPrefab_02; // BossEnemy_02
    public GameObject bossEnemyPrefab_03; // BossEnemy_03
    public Transform spawnPoint; // ���ʹ� ���� ��ġ
    bool _bossWillSpawn = false; // ���� üũ

    // ���̺�
    List<EnemyCtrl> _currentEnemies = new List<EnemyCtrl>(); // ���� ���̺� ���ʹ� ����Ʈ
    float _spawnInterval = 1f; // ���ʹ� �Ÿ� ����
    public int currentWave = 0; // ���� ���̺� ��
    int _maxWave = 10; // �� ���̺� ��
    bool _isWaveInProgress = false; // ���̺� ���� ���� Ȯ��
    public bool isFinished = false; // ���� ����
    public bool _bReward = false;

    // ����
    public GameObject repairUI; // ���� ���� UI
    public bool waitingForRepair = false; // ���� ���� Ȯ��

    // UI
    public Text waveText; // ���� ���̺� ī��Ʈ
    public GameObject gameOverUI; // Ŭ���� UI
    public Button repairButton; // ���� �Ϸ� ��ư

    // �������� Ŭ���� ����
    public int rewardGold;

    public int stageClear = 0;

    private void Start()
    {
        // ��ũ��Ʈ
        _enemyCtrl = FindObjectOfType<EnemyCtrl>();
        _gameManager = FindObjectOfType<GameManager>();
        _lobbyManager = FindObjectOfType<LobbyManager>();
        _repairCardDex = FindObjectOfType<RepairCradDex>();

        // ���� �Ϸ� ��ư Ŭ��
        repairButton.onClick.AddListener(OnRepairFinished);
    }

    void Update()
    {
        // ���̺� ����
        if (!_isWaveInProgress && !waitingForRepair)
        {
            StartCoroutine(CoSpawnWave());
        }

        // ���̺� ī��Ʈ ���
        PrintWaveCount();
    }

    // ���̺� ����
    IEnumerator CoSpawnWave()
    {
        _isWaveInProgress = true;
        _currentEnemies.Clear();
        currentWave++;

        int spawnBatchCount = 3; // �� ���̺�� �� �� ������ ��������
        int enemiesPerBatch = 5; // �� �� ������ ���ʹ� ��
        string sceneName = SceneManager.GetActiveScene().name; // �� �̸�

        // ���̺� �ε��� �� �������̼�
        // �������� 1
        if (sceneName == Define.Stage_01)
        {
            // 1-3, 1-4
            if (currentWave == 3 || currentWave == 4)
            {
                // �� �� ���ʹ� �� ����
                enemiesPerBatch = 10;
                yield return StartCoroutine(CoSpawnEnemies(enemyPrefab_01, spawnBatchCount, enemiesPerBatch));
            }
            // 1-7, 1-8, 1-9
            else if (currentWave == 7 || currentWave == 8 || currentWave == 9)
            {
                // �� �� ���ʹ� �� ����
                enemiesPerBatch = 15;
                // ���ʹ� �̵��ӵ� ���� -> EnemyCtrl Start �Լ����� ����
                yield return StartCoroutine(CoSpawnEnemies(enemyPrefab_01, spawnBatchCount, enemiesPerBatch));

            }
            // 1-10
            else if (currentWave == 10)
            {
                yield return StartCoroutine(CoSpawnEnemies(enemyPrefab_01, spawnBatchCount, enemiesPerBatch));
                _bossWillSpawn = true;
                yield return new WaitForSeconds(5f); // 5�� �� �������ʹ� ����
                SpawnBoss(bossEnemyPrefab_01);
                _bossWillSpawn = false;
            }
            // 1-1, 1-2, 1-5, 1-6
            else
            {
                yield return StartCoroutine(CoSpawnEnemies(enemyPrefab_01, spawnBatchCount, enemiesPerBatch));
            }
        }

        // �������� 2
        if (sceneName == Define.Stage_02)
        {
            // 2-2
            if (currentWave == 2)
            {
                // �� �� ���ʹ� �� ����
                enemiesPerBatch = 10;
                yield return StartCoroutine(CoSpawnEnemies(enemyPrefab_01, spawnBatchCount, enemiesPerBatch));
            }
            // 2-4, 2-5
            if (currentWave == 4 || currentWave == 5)
            {
                // �� �� ���ʹ� �� ����
                enemiesPerBatch = 15;
                // ���ʹ� �̵��ӵ� ���� -> EnemyCtrl Start �Լ����� ����
                yield return StartCoroutine(CoSpawnEnemies(enemyPrefab_01, spawnBatchCount, enemiesPerBatch));
            }
            // 2-6, 2-7
            if (currentWave == 6 || currentWave == 7)
            {
                yield return StartCoroutine(CoSpawnEnemies(enemyPrefab_02, spawnBatchCount, enemiesPerBatch));
            }
            // 2-8
            else if (currentWave == 8)
            {
                yield return StartCoroutine(CoSpawnEnemies(shieldEnemyPrefab_01, spawnBatchCount, enemiesPerBatch));
            }
            // 2-9
            else if (currentWave == 9)
            {
                yield return StartCoroutine(CoSpawnEnemies(shieldEnemyPrefab_02, spawnBatchCount, enemiesPerBatch));
            }
            // 2-10
            else if (currentWave == 10)
            {
                yield return StartCoroutine(CoSpawnEnemies(enemyPrefab_01, spawnBatchCount, enemiesPerBatch));
                yield return StartCoroutine(CoSpawnEnemies(enemyPrefab_02, spawnBatchCount, enemiesPerBatch));
                _bossWillSpawn = true;
                yield return new WaitForSeconds(5f); // 5�� �� �������ʹ� ����
                SpawnBoss(bossEnemyPrefab_02);
                _bossWillSpawn = false;
            }
            // 2-1
            else
            {
                yield return StartCoroutine(CoSpawnEnemies(enemyPrefab_01, spawnBatchCount, enemiesPerBatch));
            }
        }

        // �������� 3
        if (sceneName == Define.Stage_03)
        {
            // 3-2
            if (currentWave == 2)
            {
                // �� �� ���ʹ� �� ����
                enemiesPerBatch = 10;
                yield return StartCoroutine(CoSpawnEnemies(enemyPrefab_01, spawnBatchCount, enemiesPerBatch));
            }
            // 3-4
            else if (currentWave == 4)
            {
                // �� �� ���ʹ� �� ����
                enemiesPerBatch = 15;
                // ���ʹ� �̵��ӵ� ���� -> EnemyCtrl Start �Լ����� ����
                yield return StartCoroutine(CoSpawnEnemies(enemyPrefab_01, spawnBatchCount, enemiesPerBatch));
            }
            // 3-5
            else if (currentWave == 5)
            {
                yield return StartCoroutine(CoSpawnEnemies(enemyPrefab_02, spawnBatchCount, enemiesPerBatch));
            }
            // 3-6, 3-8
            else if (currentWave == 6 || currentWave == 8)
            {
                yield return StartCoroutine(CoSpawnEnemies(shieldEnemyPrefab_01, spawnBatchCount, enemiesPerBatch));
            }
            // 3-7, 3-9
            else if (currentWave == 7 || currentWave == 9)
            {
                yield return StartCoroutine(CoSpawnEnemies(shieldEnemyPrefab_02, spawnBatchCount, enemiesPerBatch));
            }
            // 3-10
            else if (currentWave == 10)
            {
                yield return StartCoroutine(CoSpawnEnemies(enemyPrefab_01, spawnBatchCount, enemiesPerBatch));
                yield return StartCoroutine(CoSpawnEnemies(enemyPrefab_02, spawnBatchCount, enemiesPerBatch));
                _bossWillSpawn = true;
                yield return new WaitForSeconds(5f); // 5�� �� �������ʹ� ����
                SpawnBoss(bossEnemyPrefab_03);
                _bossWillSpawn = false;
            }
            // 3-1
            else
            {
                yield return StartCoroutine(CoSpawnEnemies(enemyPrefab_01, spawnBatchCount, enemiesPerBatch));
            }
        }
    }

    // ���ʹ� ����
    IEnumerator CoSpawnEnemies(GameObject prefab, int batchCount, int enemiesPerBatch)
    {
        for (int batch = 0; batch < batchCount; batch++)
        {
            for (int i = 0; i < enemiesPerBatch; i++)
            {
                GameObject enemyObj = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
                EnemyCtrl enemy = enemyObj.GetComponent<EnemyCtrl>();
                enemy.OnEnemyDied += OnEnemyDied; // �̺�Ʈ ����
                _currentEnemies.Add(enemy);

                yield return new WaitForSeconds(_spawnInterval);
            }
            yield return new WaitForSeconds(2f);
        }
    }

    // �������ʹ� ����
    void SpawnBoss(GameObject bossPrefab)
    {
        GameObject bossObj = Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);
        EnemyCtrl bossEnemy = bossObj.GetComponent<EnemyCtrl>();
        bossEnemy.OnEnemyDied += OnEnemyDied; // �̺�Ʈ ����
        _currentEnemies.Add(bossEnemy);
    }

    // ���̺� ���ʹ̰� ��� ����ϸ� ���� �ð� ����
    void OnEnemyDied(EnemyCtrl deadEnemy)
    {
        _currentEnemies.Remove(deadEnemy);
        string sceneName = SceneManager.GetActiveScene().name;// �ֿ�

        if (_currentEnemies.Count == 0)
        {
            // ���� üũ
            if (_bossWillSpawn) return;

            // ������ ���̺���
            if (currentWave == _maxWave)
            {
                if (!_bReward)
                {
                    // ���̺� ���� üũ
                    _isWaveInProgress = false;
                    waitingForRepair = true;
                    isFinished = true;
                    _bReward = true;

                    // ���� ���
                    rewardGold = 10000;
                    _lobbyManager.gold += rewardGold;

                    // ��� ����
                    PlayerPrefs.SetInt("Gold", _lobbyManager.gold);

                    if (isFinished)
                    {
                        // Ŭ���� â Ȱ��ȭ
                        gameOverUI.SetActive(true);

                        // -- �ֿ�
                        int stageClear = PlayerPrefs.GetInt("StageClear", 0); // ����� �� �ҷ�����

                        if (sceneName == Define.Stage_01 && PlayerPrefs.GetInt("Stage1Clear", 0) == 0 && stageClear == 0)
                        {
                            PlayerPrefs.SetInt("Stage1Clear", 1);
                            PlayerPrefs.SetInt("StageClear", 1);
                            PlayerPrefs.Save();
                        }
                        else if (sceneName == Define.Stage_02 && PlayerPrefs.GetInt("Stage2Clear", 0) == 0 && stageClear == 1)
                        {
                            PlayerPrefs.SetInt("Stage2Clear", 1);
                            PlayerPrefs.SetInt("StageClear", 2);
                            PlayerPrefs.Save();
                        }
                        else if (sceneName == Define.Stage_03 && PlayerPrefs.GetInt("Stage3Clear", 0) == 0 && stageClear == 2)
                        {
                            PlayerPrefs.SetInt("Stage3Clear", 1);
                            PlayerPrefs.SetInt("StageClear", 3);
                            PlayerPrefs.Save();
                        }
                        // -- �ֿ� ����Ʈ �۾�
                    }
                }
            }
            else
            {
                _isWaveInProgress = false;
                waitingForRepair = true;

                repairUI.SetActive(true);

                // ���� ������ ����
                if (_repairCardDex != null)
                    _repairCardDex.UpdateRanDomCardUI();
            }
        }
    }

    IEnumerator CoDelayRepair()
    {
        yield return new WaitForSeconds(1f);
    }

    // ���� ���� ��ư Ŭ�� �� ���̺� �����
    public void OnRepairFinished()
    {
        waitingForRepair = false;
        _repairCardDex.buyText.text = "";
        repairUI.SetActive(false);
    }

    // ���̺� �� ���
    void PrintWaveCount()
    {
        waveText.text = $"Wave {currentWave}";
    }
}
