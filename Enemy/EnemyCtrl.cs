using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public abstract class EnemyCtrl : MonoBehaviour
{
    // 컴포넌트
    protected Animator _animator;
    // 색 변화
    public Material redMaterial; // 빨간색 머티리얼
    Material[] originalMaterials; // 기존 머티리얼
    Renderer objectRenderer;

    // 스크립트
    protected GameManager _gameManager;
    protected TowerCtrl _towerCtrl;
    protected SpawnManager _spawnManager;

    // 이동
    protected Vector3[] _waypoints;
    protected int _currentWaypointIndex = 0;
    protected float _moveSpeed; // 이동 속도
    protected float _originMoveSpeed; // 이동 속도 저장 (얼음 위 스피드 조절 위함)

    // 공격
    protected bool _isAttacking = false;
    protected float _attackInterval = 2f; // 공격 시간 간격

    // 체력
    protected int _maxHp;
    protected int _currentHp;
    public int bulletDamage;
    public bool bDestroyShield = false; // 쉴드 제거 여부 (방어막 파괴 여부)
    public bool bShieldEnemy = true; // 데미지 처리 시 에너미 체크 위함
    public GameObject fireDamage; // 파이어 도트데미지 프리팹
    public bool isOnFire = false;

    // 사망
    public bool isLive = true;
    public System.Action<EnemyCtrl> OnEnemyDied; // 사망 시 알림 이벤트 (델리게이트)

    // UI
    // 체력바
    public GameObject hpBarUI;
    public UnityEngine.UI.Slider hpBar;
    // 데미지 출력
    public GameObject damagePrefab;
    // 골드
    public GameObject goldPrefab;

    // 각 에너미별 초깃값
    protected abstract void InitEnemy();

    protected virtual void Start()
    {
        // 스크립트
        _gameManager = FindObjectOfType<GameManager>();
        _towerCtrl = FindObjectOfType<TowerCtrl>();
        _spawnManager = FindObjectOfType<SpawnManager>();

        // 컴포넌트
        _animator = GetComponent<Animator>();
        objectRenderer = GetComponentInChildren<Renderer>();
        originalMaterials = objectRenderer.materials;

        // 초깃값
        InitEnemy();

        _waypoints = GetWaypoints();
    }

    protected virtual Vector3[] GetWaypoints()
    {
        return new Vector3[]
        {
            new Vector3(-1.5f, -6.9f, -5.3f),
            new Vector3(6.1f, -6.9f, -5.2f),
            new Vector3(6.1f, -6.9f, 8.3f),
            new Vector3(-3.5f, -6.9f, 8.3f),
            new Vector3(-3.5f, -6.9f, -3.3f),
            new Vector3(1.1f, -6.9f, -3.3f),
        };
    }

    protected virtual void Update()
    {
        // 공격 중이 아니라면 이동
        if (!_isAttacking && isLive)
        {
            Move();
        }

        // HpBar 업데이트
        UpdateHp();

        // _currentHp가 0 이하라면 사망
        if (_currentHp <= 0)
        {
            Die();
        }
    }

    protected virtual void Move()
    {
        if (_waypoints.Length == 0 || _currentWaypointIndex >= _waypoints.Length) return;

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

            // 성문 앞 도착 (마지막 포인트)
            if (_currentWaypointIndex >= _waypoints.Length)
            {
                // 근거리 공격
                ShortRangeAttack();
            }
        }
    }

    // 근거리 공격 (성문 앞 공격)
    protected virtual void ShortRangeAttack()
    {
        _isAttacking = true;

        // 마지막 포인트에서 성문 바라보도록 왼쪽으로 회전
        Vector3 lookDirection = -transform.right;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = lookRotation;

        // 성문 공격 애니메이션
        _animator.SetBool(Define.attack, true);

        // 성 데미지 입히기
        StartCoroutine(CoSetDamage());
    }

    protected virtual IEnumerator CoSetDamage()
    {
        // 살아있는 동안 반복적으로 공격
        while (isLive)
        {
            yield return new WaitForSeconds(_attackInterval);

            int enemyDamage = 100;
            _towerCtrl.GetDamage(enemyDamage);
        }
    }

    // 충돌 시
    private void OnTriggerEnter(Collider other)
    {
        // 일반 유닛 공격
        if (other.gameObject.CompareTag(Define.Bullet))
        {
            ChangeRed();
            //GetHit();
        }

        // 얼음 유닛 공격
        if (other.gameObject.CompareTag(Define.IceBullet))
        {
            ChangeRed();
            //GetHit();
        }

        // 불 유닛 공격
        if (other.gameObject.CompareTag(Define.FireBullet))
        {
            if (bShieldEnemy) return;

            ChangeRed();

            if (!isOnFire)
            {
                GameObject fire = Instantiate(fireDamage, transform.position, Quaternion.identity, transform);
                isOnFire = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // 얼음바닥 충돌 중일 시
        if (other.gameObject.CompareTag(Define.IceTile))
        {
            // 이동속도 느려짐
            _moveSpeed = 0.5f;
        }
        // 얼음바닥 벗어나면 원래 속도로
        else
        {
            _moveSpeed = _originMoveSpeed;
        }
    }

    // 데미지 처리 각 스킬에서 함
    // 총알에 맞은 후 데미지 처리
    //public void GetHit()
    //{
    //    // 총알을 맞으면 일시적으로 HpBar가 보이며 체력이 깎임
    //    //hpBarUI.SetActive(true);
    //    //StartCoroutine(CoActiveHpBar());

    //    //IEnumerator CoActiveHpBar()
    //    //{
    //    //    yield return new WaitForSeconds(2);
    //    //    hpBarUI.SetActive(false);
    //    //}

    //    // Hp 감소
    //    //bulletDamage = Random.Range(10, 50);
    //    //_currentHp -= bulletDamage;

    //    // 데미지 출력
    //    //Vector3 printPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
    //    //GameObject damageText = Instantiate(damagePrefab, printPosition, Quaternion.Euler(90, 0, 0));

    //    //DamageUI damageUI = damageText.GetComponent<DamageUI>();

    //    //if (damageUI != null)
    //    //{
    //    //    damageUI.SetDamage(bulletDamage); // 데미지 값을 전달
    //    //}
    //}

    // 에너미 사망 처리
    protected virtual void Die()
    {
        // 이동 멈춤
        isLive = false;

        // death 애니메이션 출력
        _animator.SetTrigger(Define.death);
        hpBarUI.SetActive(false);

        OnEnemyDied?.Invoke(this);

        // 에너미 제거
        StartCoroutine(CoDestroyEnemy());
    }

    protected virtual IEnumerator CoDestroyEnemy()
    {
        yield return new WaitForSeconds(1.2f);

        RandomGold();
        Destroy(gameObject);
    }

    // HpBar 업데이트
    protected virtual void UpdateHp()
    {
        hpBar.value = _currentHp;
        hpBar.maxValue = _maxHp;
    }

    // 사망 시 50% 확률로 플레이어에게 골드 지급
    protected virtual void RandomGold()
    {
        if (Random.Range(0f, 1f) <= 0.5f)
        {
            // 골드 생성
            Vector3 goldPosition = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
            Instantiate(goldPrefab, goldPosition, Quaternion.identity);

            // 골드 지급
            int goldAmount = 1;
            _gameManager.AddGold(goldAmount);
        }
    }

    // 불 도트 데미지용 함수였으나, 각 스킬에서 데미지 처리를 하며 데미지 처리 함수가 됨 (이에 따라 GetHit 함수 주석 처리함)
    public void ApplyDotDamage(int dotDamage, bool isBuffed)
    {
        if (!isLive) return;

        _currentHp -= dotDamage;

        // 총알을 맞으면 일시적으로 HpBar가 보이며 체력이 깎임
        hpBarUI.SetActive(true);
        StartCoroutine(CoActiveHpBar());

        IEnumerator CoActiveHpBar()
        {
            yield return new WaitForSeconds(2);
            hpBarUI.SetActive(false);
        }

        // 데미지 출력
        Vector3 printPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        GameObject damageText = Instantiate(damagePrefab, printPosition, Quaternion.Euler(90, 0, 0));

        DamageUI damageUI = damageText.GetComponent<DamageUI>();

        if (damageUI != null)
        {
            damageUI.SetDamage(dotDamage, isBuffed);
        }
    }

    // 맞았을 때 빨갛게 변경
    public void ChangeRed()
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
