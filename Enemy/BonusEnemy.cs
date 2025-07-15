using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BonusEnemy : MonoBehaviour
{
    // ������Ʈ
    Animator _animator;
    // �� ��ȭ
    public Material redMaterial; // ������ ��Ƽ����
    Material[] originalMaterials; // ���� ��Ƽ����
    Renderer objectRenderer;

    // ��ũ��Ʈ
    LobbyManager _lobbyManager;
    BonusGameManager _bonusGameManager;

    // �̵�
    Vector3[] _waypoints;
    int _currentWaypointIndex = 0;
    float _moveSpeed;

    // ���
    public bool isLive = true;

    // UI
    // ���
    public GameObject goldPrefab;
    public GameObject goldTextPrefab;
    // ����ũ
    public GameObject smokePrefab;

    void Start()
    {
        // ��ũ��Ʈ
        _lobbyManager = FindObjectOfType<LobbyManager>();
        _bonusGameManager = FindObjectOfType<BonusGameManager>();

        // ������Ʈ
        _animator = GetComponent<Animator>();
        objectRenderer = GetComponentInChildren<Renderer>();
        originalMaterials = objectRenderer.materials;

        // �ʱ갪
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

            // Ÿ�� �� ���� (������ ����Ʈ)
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
        // ���� ���� (�Ϲ� ����)
        if (other.gameObject.CompareTag(Define.Bullet) && isLive)
        {
            // ��� �ؽ�Ʈ ���ξƿ� ȿ��
            _bonusGameManager.GoldTextZoomInOut();

            // �ִϸ��̼� ���
            _animator.SetTrigger(Define.getHit);

            // �¾��� �� ������ ����
            ChangeRed();

            // �̵��ӵ� ����
            _moveSpeed += 0.1f;

            // ����ũ ���
            Vector3 smokePosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            GameObject bonusEnemySmokePrefab = Instantiate(smokePrefab, smokePosition, Quaternion.identity);
            Destroy(bonusEnemySmokePrefab, 1);

            // ��� ����
            int goldAmount = 500;
            _lobbyManager.gold += goldAmount;

            // ��� �ݾ� ���
            Vector3 printPosition = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
            GameObject goldText = Instantiate(goldTextPrefab, printPosition, Quaternion.Euler(90, 0, 0));

            BonusGoldUI bonusGoldUI = goldText.GetComponent<BonusGoldUI>();

            if (bonusGoldUI != null)
            {
                bonusGoldUI.AddGold(goldAmount);
            }

            // ��� ������ ���
            for (int i = 0; i < 5; i++)
            {
                Instantiate(goldPrefab, transform.position, Quaternion.identity);
            }
        }
    }

    // �¾��� �� ������ ����
    void ChangeRed()
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
