using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public abstract class EnemyCtrl : MonoBehaviour
{
    // ������Ʈ
    protected Animator _animator;
    // �� ��ȭ
    public Material redMaterial; // ������ ��Ƽ����
    Material[] originalMaterials; // ���� ��Ƽ����
    Renderer objectRenderer;

    // ��ũ��Ʈ
    protected GameManager _gameManager;
    protected TowerCtrl _towerCtrl;
    protected SpawnManager _spawnManager;

    // �̵�
    protected Vector3[] _waypoints;
    protected int _currentWaypointIndex = 0;
    protected float _moveSpeed; // �̵� �ӵ�
    protected float _originMoveSpeed; // �̵� �ӵ� ���� (���� �� ���ǵ� ���� ����)

    // ����
    protected bool _isAttacking = false;
    protected float _attackInterval = 2f; // ���� �ð� ����

    // ü��
    protected int _maxHp;
    protected int _currentHp;
    public int bulletDamage;
    public bool bDestroyShield = false; // ���� ���� ���� (�� �ı� ����)
    public bool bShieldEnemy = true; // ������ ó�� �� ���ʹ� üũ ����
    public GameObject fireDamage; // ���̾� ��Ʈ������ ������
    public bool isOnFire = false;

    // ���
    public bool isLive = true;
    public System.Action<EnemyCtrl> OnEnemyDied; // ��� �� �˸� �̺�Ʈ (��������Ʈ)

    // UI
    // ü�¹�
    public GameObject hpBarUI;
    public UnityEngine.UI.Slider hpBar;
    // ������ ���
    public GameObject damagePrefab;
    // ���
    public GameObject goldPrefab;

    // �� ���ʹ̺� �ʱ갪
    protected abstract void InitEnemy();

    protected virtual void Start()
    {
        // ��ũ��Ʈ
        _gameManager = FindObjectOfType<GameManager>();
        _towerCtrl = FindObjectOfType<TowerCtrl>();
        _spawnManager = FindObjectOfType<SpawnManager>();

        // ������Ʈ
        _animator = GetComponent<Animator>();
        objectRenderer = GetComponentInChildren<Renderer>();
        originalMaterials = objectRenderer.materials;

        // �ʱ갪
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
        // ���� ���� �ƴ϶�� �̵�
        if (!_isAttacking && isLive)
        {
            Move();
        }

        // HpBar ������Ʈ
        UpdateHp();

        // _currentHp�� 0 ���϶�� ���
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

        // �̵� ���⿡ ���߾� ȸ��
        Vector3 direction = (target - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 10f * Time.deltaTime);
        }

        // �������� �����ϸ� ���� ����Ʈ��
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            _currentWaypointIndex++;

            // ���� �� ���� (������ ����Ʈ)
            if (_currentWaypointIndex >= _waypoints.Length)
            {
                // �ٰŸ� ����
                ShortRangeAttack();
            }
        }
    }

    // �ٰŸ� ���� (���� �� ����)
    protected virtual void ShortRangeAttack()
    {
        _isAttacking = true;

        // ������ ����Ʈ���� ���� �ٶ󺸵��� �������� ȸ��
        Vector3 lookDirection = -transform.right;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = lookRotation;

        // ���� ���� �ִϸ��̼�
        _animator.SetBool(Define.attack, true);

        // �� ������ ������
        StartCoroutine(CoSetDamage());
    }

    protected virtual IEnumerator CoSetDamage()
    {
        // ����ִ� ���� �ݺ������� ����
        while (isLive)
        {
            yield return new WaitForSeconds(_attackInterval);

            int enemyDamage = 100;
            _towerCtrl.GetDamage(enemyDamage);
        }
    }

    // �浹 ��
    private void OnTriggerEnter(Collider other)
    {
        // �Ϲ� ���� ����
        if (other.gameObject.CompareTag(Define.Bullet))
        {
            ChangeRed();
            //GetHit();
        }

        // ���� ���� ����
        if (other.gameObject.CompareTag(Define.IceBullet))
        {
            ChangeRed();
            //GetHit();
        }

        // �� ���� ����
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
        // �����ٴ� �浹 ���� ��
        if (other.gameObject.CompareTag(Define.IceTile))
        {
            // �̵��ӵ� ������
            _moveSpeed = 0.5f;
        }
        // �����ٴ� ����� ���� �ӵ���
        else
        {
            _moveSpeed = _originMoveSpeed;
        }
    }

    // ������ ó�� �� ��ų���� ��
    // �Ѿ˿� ���� �� ������ ó��
    //public void GetHit()
    //{
    //    // �Ѿ��� ������ �Ͻ������� HpBar�� ���̸� ü���� ����
    //    //hpBarUI.SetActive(true);
    //    //StartCoroutine(CoActiveHpBar());

    //    //IEnumerator CoActiveHpBar()
    //    //{
    //    //    yield return new WaitForSeconds(2);
    //    //    hpBarUI.SetActive(false);
    //    //}

    //    // Hp ����
    //    //bulletDamage = Random.Range(10, 50);
    //    //_currentHp -= bulletDamage;

    //    // ������ ���
    //    //Vector3 printPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
    //    //GameObject damageText = Instantiate(damagePrefab, printPosition, Quaternion.Euler(90, 0, 0));

    //    //DamageUI damageUI = damageText.GetComponent<DamageUI>();

    //    //if (damageUI != null)
    //    //{
    //    //    damageUI.SetDamage(bulletDamage); // ������ ���� ����
    //    //}
    //}

    // ���ʹ� ��� ó��
    protected virtual void Die()
    {
        // �̵� ����
        isLive = false;

        // death �ִϸ��̼� ���
        _animator.SetTrigger(Define.death);
        hpBarUI.SetActive(false);

        OnEnemyDied?.Invoke(this);

        // ���ʹ� ����
        StartCoroutine(CoDestroyEnemy());
    }

    protected virtual IEnumerator CoDestroyEnemy()
    {
        yield return new WaitForSeconds(1.2f);

        RandomGold();
        Destroy(gameObject);
    }

    // HpBar ������Ʈ
    protected virtual void UpdateHp()
    {
        hpBar.value = _currentHp;
        hpBar.maxValue = _maxHp;
    }

    // ��� �� 50% Ȯ���� �÷��̾�� ��� ����
    protected virtual void RandomGold()
    {
        if (Random.Range(0f, 1f) <= 0.5f)
        {
            // ��� ����
            Vector3 goldPosition = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
            Instantiate(goldPrefab, goldPosition, Quaternion.identity);

            // ��� ����
            int goldAmount = 1;
            _gameManager.AddGold(goldAmount);
        }
    }

    // �� ��Ʈ �������� �Լ�������, �� ��ų���� ������ ó���� �ϸ� ������ ó�� �Լ��� �� (�̿� ���� GetHit �Լ� �ּ� ó����)
    public void ApplyDotDamage(int dotDamage, bool isBuffed)
    {
        if (!isLive) return;

        _currentHp -= dotDamage;

        // �Ѿ��� ������ �Ͻ������� HpBar�� ���̸� ü���� ����
        hpBarUI.SetActive(true);
        StartCoroutine(CoActiveHpBar());

        IEnumerator CoActiveHpBar()
        {
            yield return new WaitForSeconds(2);
            hpBarUI.SetActive(false);
        }

        // ������ ���
        Vector3 printPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        GameObject damageText = Instantiate(damagePrefab, printPosition, Quaternion.Euler(90, 0, 0));

        DamageUI damageUI = damageText.GetComponent<DamageUI>();

        if (damageUI != null)
        {
            damageUI.SetDamage(dotDamage, isBuffed);
        }
    }

    // �¾��� �� ������ ����
    public void ChangeRed()
    {
        StartCoroutine(CoChangeRed());
    }

    IEnumerator CoChangeRed()
    {
        // ������ ��Ƽ����� ����
        Material[] newMaterials = new Material[originalMaterials.Length];

        for (int i = 0; i < originalMaterials.Length; i++)
        {
            newMaterials[i] = redMaterial;
        }

        objectRenderer.materials = newMaterials;

        // 0.3�� ���� ������ ����
        yield return new WaitForSeconds(0.3f);

        // �ٽ� ���� ��Ƽ����� ����
        objectRenderer.materials = originalMaterials;
    }
}
