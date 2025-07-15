using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    // 스크립트
    EnemyCtrl _enemyCtrl;
    GameManager _gameManager;
    LobbyManager _lobbyManager;
    RepairCradDex _repairCardDex;

    // 에너미
    // 에너미 프리팹
    public GameObject enemyPrefab_01; // Enemy_01
    public GameObject enemyPrefab_02; // Enemy_02
    public GameObject shieldEnemyPrefab_01; // ShieldEnemy_01
    public GameObject shieldEnemyPrefab_02; // ShieldEnemy_02
    public GameObject bossEnemyPrefab_01; // BossEnemy_01
    public GameObject bossEnemyPrefab_02; // BossEnemy_02
    public GameObject bossEnemyPrefab_03; // BossEnemy_03
    public Transform spawnPoint; // 에너미 스폰 위치
    bool _bossWillSpawn = false; // 보스 체크

    // 웨이브
    List<EnemyCtrl> _currentEnemies = new List<EnemyCtrl>(); // 현재 웨이브 에너미 리스트
    float _spawnInterval = 1f; // 에너미 거리 간격
    public int currentWave = 0; // 현재 웨이브 수
    int _maxWave = 10; // 총 웨이브 수
    bool _isWaveInProgress = false; // 웨이브 진행 여부 확인
    public bool isFinished = false; // 종료 여부
    public bool _bReward = false;

    // 정비
    public GameObject repairUI; // 유닛 정비 UI
    public bool waitingForRepair = false; // 정비 여부 확인

    // UI
    public Text waveText; // 현재 웨이브 카운트
    public GameObject gameOverUI; // 클리어 UI
    public Button repairButton; // 정비 완료 버튼

    // 스테이지 클리어 보상
    public int rewardGold;

    public int stageClear = 0;

    private void Start()
    {
        // 스크립트
        _enemyCtrl = FindObjectOfType<EnemyCtrl>();
        _gameManager = FindObjectOfType<GameManager>();
        _lobbyManager = FindObjectOfType<LobbyManager>();
        _repairCardDex = FindObjectOfType<RepairCradDex>();

        // 정비 완료 버튼 클릭
        repairButton.onClick.AddListener(OnRepairFinished);
    }

    void Update()
    {
        // 웨이브 생성
        if (!_isWaveInProgress && !waitingForRepair)
        {
            StartCoroutine(CoSpawnWave());
        }

        // 웨이브 카운트 출력
        PrintWaveCount();
    }

    // 웨이브 생성
    IEnumerator CoSpawnWave()
    {
        _isWaveInProgress = true;
        _currentEnemies.Clear();
        currentWave++;

        int spawnBatchCount = 3; // 한 웨이브당 몇 번 나눠서 등장할지
        int enemiesPerBatch = 5; // 한 번 등장할 에너미 수
        string sceneName = SceneManager.GetActiveScene().name; // 씬 이름

        // 웨이브 인덱스 별 베리에이션
        // 스테이지 1
        if (sceneName == Define.Stage_01)
        {
            // 1-3, 1-4
            if (currentWave == 3 || currentWave == 4)
            {
                // 한 텀 에너미 수 증가
                enemiesPerBatch = 10;
                yield return StartCoroutine(CoSpawnEnemies(enemyPrefab_01, spawnBatchCount, enemiesPerBatch));
            }
            // 1-7, 1-8, 1-9
            else if (currentWave == 7 || currentWave == 8 || currentWave == 9)
            {
                // 한 텀 에너미 수 증가
                enemiesPerBatch = 15;
                // 에너미 이동속도 증가 -> EnemyCtrl Start 함수에서 설정
                yield return StartCoroutine(CoSpawnEnemies(enemyPrefab_01, spawnBatchCount, enemiesPerBatch));

            }
            // 1-10
            else if (currentWave == 10)
            {
                yield return StartCoroutine(CoSpawnEnemies(enemyPrefab_01, spawnBatchCount, enemiesPerBatch));
                _bossWillSpawn = true;
                yield return new WaitForSeconds(5f); // 5초 후 보스에너미 생성
                SpawnBoss(bossEnemyPrefab_01);
                _bossWillSpawn = false;
            }
            // 1-1, 1-2, 1-5, 1-6
            else
            {
                yield return StartCoroutine(CoSpawnEnemies(enemyPrefab_01, spawnBatchCount, enemiesPerBatch));
            }
        }

        // 스테이지 2
        if (sceneName == Define.Stage_02)
        {
            // 2-2
            if (currentWave == 2)
            {
                // 한 텀 에너미 수 증가
                enemiesPerBatch = 10;
                yield return StartCoroutine(CoSpawnEnemies(enemyPrefab_01, spawnBatchCount, enemiesPerBatch));
            }
            // 2-4, 2-5
            if (currentWave == 4 || currentWave == 5)
            {
                // 한 텀 에너미 수 증가
                enemiesPerBatch = 15;
                // 에너미 이동속도 증가 -> EnemyCtrl Start 함수에서 설정
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
                yield return new WaitForSeconds(5f); // 5초 후 보스에너미 생성
                SpawnBoss(bossEnemyPrefab_02);
                _bossWillSpawn = false;
            }
            // 2-1
            else
            {
                yield return StartCoroutine(CoSpawnEnemies(enemyPrefab_01, spawnBatchCount, enemiesPerBatch));
            }
        }

        // 스테이지 3
        if (sceneName == Define.Stage_03)
        {
            // 3-2
            if (currentWave == 2)
            {
                // 한 텀 에너미 수 증가
                enemiesPerBatch = 10;
                yield return StartCoroutine(CoSpawnEnemies(enemyPrefab_01, spawnBatchCount, enemiesPerBatch));
            }
            // 3-4
            else if (currentWave == 4)
            {
                // 한 텀 에너미 수 증가
                enemiesPerBatch = 15;
                // 에너미 이동속도 증가 -> EnemyCtrl Start 함수에서 설정
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
                yield return new WaitForSeconds(5f); // 5초 후 보스에너미 생성
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

    // 에너미 생성
    IEnumerator CoSpawnEnemies(GameObject prefab, int batchCount, int enemiesPerBatch)
    {
        for (int batch = 0; batch < batchCount; batch++)
        {
            for (int i = 0; i < enemiesPerBatch; i++)
            {
                GameObject enemyObj = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
                EnemyCtrl enemy = enemyObj.GetComponent<EnemyCtrl>();
                enemy.OnEnemyDied += OnEnemyDied; // 이벤트 연결
                _currentEnemies.Add(enemy);

                yield return new WaitForSeconds(_spawnInterval);
            }
            yield return new WaitForSeconds(2f);
        }
    }

    // 보스에너미 생성
    void SpawnBoss(GameObject bossPrefab)
    {
        GameObject bossObj = Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);
        EnemyCtrl bossEnemy = bossObj.GetComponent<EnemyCtrl>();
        bossEnemy.OnEnemyDied += OnEnemyDied; // 이벤트 연결
        _currentEnemies.Add(bossEnemy);
    }

    // 웨이브 에너미가 모두 사망하면 정비 시간 시작
    void OnEnemyDied(EnemyCtrl deadEnemy)
    {
        _currentEnemies.Remove(deadEnemy);
        string sceneName = SceneManager.GetActiveScene().name;// 주연

        if (_currentEnemies.Count == 0)
        {
            // 보스 체크
            if (_bossWillSpawn) return;

            // 마지막 웨이브라면
            if (currentWave == _maxWave)
            {
                if (!_bReward)
                {
                    // 웨이브 상태 체크
                    _isWaveInProgress = false;
                    waitingForRepair = true;
                    isFinished = true;
                    _bReward = true;

                    // 보상 골드
                    rewardGold = 10000;
                    _lobbyManager.gold += rewardGold;

                    // 골드 저장
                    PlayerPrefs.SetInt("Gold", _lobbyManager.gold);

                    if (isFinished)
                    {
                        // 클리어 창 활성화
                        gameOverUI.SetActive(true);

                        // -- 주연
                        int stageClear = PlayerPrefs.GetInt("StageClear", 0); // 저장된 값 불러오기

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
                        // -- 주연 퀘스트 작업
                    }
                }
            }
            else
            {
                _isWaveInProgress = false;
                waitingForRepair = true;

                repairUI.SetActive(true);

                // 유닛 무작위 갱신
                if (_repairCardDex != null)
                    _repairCardDex.UpdateRanDomCardUI();
            }
        }
    }

    IEnumerator CoDelayRepair()
    {
        yield return new WaitForSeconds(1f);
    }

    // 정비 종료 버튼 클릭 시 웨이브 재시작
    public void OnRepairFinished()
    {
        waitingForRepair = false;
        _repairCardDex.buyText.text = "";
        repairUI.SetActive(false);
    }

    // 웨이브 수 출력
    void PrintWaveCount()
    {
        waveText.text = $"Wave {currentWave}";
    }
}
