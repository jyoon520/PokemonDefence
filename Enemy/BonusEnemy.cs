using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BonusEnemy : MonoBehaviour
{
    // 컴포넌트
    Animator _animator;
    // 색 변화
    public Material redMaterial; // 빨간색 머티리얼
    Material[] originalMaterials; // 기존 머티리얼
    Renderer objectRenderer;

    // 스크립트
    LobbyManager _lobbyManager;
    BonusGameManager _bonusGameManager;

    // 이동
    Vector3[] _waypoints;
    int _currentWaypointIndex = 0;
    float _moveSpeed;

    // 사망
    public bool isLive = true;

    // UI
    // 골드
    public GameObject goldPrefab;
    public GameObject goldTextPrefab;
    // 스모크
    public GameObject smokePrefab;

    void Start()
    {
        // 스크립트
        _lobbyManager = FindObjectOfType<LobbyManager>();
        _bonusGameManager = FindObjectOfType<BonusGameManager>();

        // 컴포넌트
        _animator = GetComponent<Animator>();
        objectRenderer = GetComponentInChildren<Renderer>();
        originalMaterials = objectRenderer.materials;

        // 초깃값
        _waypoints = GetWaypoints();
        _moveSpeed = 1f;
    }

    Vector3[] GetWaypoints()
    {
        return new Vector3[]
        {
            new Vector3(-4.74f, -6.9f, -11.65f),
            new Vector3(-4.74f, -6.9f, -9.23f),
            new Vector3(7.28f, -6.9f, -9.23f),
            new Vector3(7.28f, -6.9f, 5.43f),
            new Vector3(-4.74f, -6.9f, 5.43f),
            new Vector3(-4.74f, -6.9f, 2.6f),
            new Vector3(4.66f, -6.9f, 2.6f),
            new Vector3(4.66f, -6.9f, -6.63f),
            new Vector3(-4.78f, -6.9f, -6.63f),
            new Vector3(-4.78f, -6.9f, -0.1f),
            new Vector3(1.24f, -6.9f, -0.1f),
        };
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (_waypoints.Length == 0 || _currentWaypointIndex >= _waypoints.Length)
            return;

        Vector3 target = _waypoints[_currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, target, _moveSpeed * Time.deltaTime);

        // 이동 방향에 맞추어 회전

        Vector3 direction = (target - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 10f * Time.deltaTime);
        }

        // 목적지에 도달하면 다음 포인트로
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            _currentWaypointIndex++;

            // 타워 앞 도착 (마지막 포인트)
            if (_currentWaypointIndex >= _waypoints.Length)
            {
                isLive = false;
                _animator.SetTrigger(Define.death);
                Destroy(gameObject, 3f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 유닛 공격 (일반 유닛)
        if (other.gameObject.CompareTag(Define.Bullet) && isLive)
        {
            // 골드 텍스트 줌인아웃 효과
            _bonusGameManager.GoldTextZoomInOut();

            // 애니메이션 출력
            _animator.SetTrigger(Define.getHit);

            // 맞았을 때 빨갛게 변경
            ChangeRed();

            // 이동속도 증가
            _moveSpeed += 0.1f;

            // 스모크 출력
            Vector3 smokePosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            GameObject bonusEnemySmokePrefab = Instantiate(smokePrefab, smokePosition, Quaternion.identity);
            Destroy(bonusEnemySmokePrefab, 1);

            // 골드 지급
            int goldAmount = 500;
            _lobbyManager.gold += goldAmount;

            // 골드 금액 출력
            Vector3 printPosition = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
            GameObject goldText = Instantiate(goldTextPrefab, printPosition, Quaternion.Euler(90, 0, 0));

            BonusGoldUI bonusGoldUI = goldText.GetComponent<BonusGoldUI>();

            if (bonusGoldUI != null)
            {
                bonusGoldUI.AddGold(goldAmount);
            }

            // 골드 프리팹 출력
            for (int i = 0; i < 5; i++)
            {
                Instantiate(goldPrefab, transform.position, Quaternion.identity);
            }
        }
    }

    // 맞았을 때 빨갛게 변경
    void ChangeRed()
    {
        StartCoroutine(CoChangeRed());
    }

    IEnumerator CoChangeRed()
    {
        // 빨간색 머티리얼로 변경
        Material[] newMaterials = new Material[originalMaterials.Length];

        for (int i = 0; i < originalMaterials.Length; i++)
        {
            newMaterials[i] = redMaterial;
        }

        objectRenderer.materials = newMaterials;

        // 0.3초 동안 빨갛게 유지
        yield return new WaitForSeconds(0.3f);

        // 다시 원래 머티리얼로 변경
        objectRenderer.materials = originalMaterials;
    }
}
