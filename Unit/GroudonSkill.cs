using UnityEngine;

public class GroudonSkill : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject fireballPrefab;        // 파이어볼 프리팹

    [Header("Settings")]
    public float fireballSpeed = 10f;
    public float attackRange = 10f;
    public float attackCooldown = 1.5f;
    public string enemyTag = "Enemy";

    private float lastAttackTime;
    private bool isClicked = false;
    private Animator _animator;
    private LineRenderer rangeLineRenderer;

    void Start()
    {
        _animator = GetComponent<Animator>();
        SetupRangeLineRenderer();
    }

    void Update()
    {
        HandleClick();
        TryAttack();
        ToggleRangeCircle();
    }

    // 마우스 클릭 감지
    void HandleClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                if (hit.transform == transform)
                    isClicked = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isClicked = false;
        }
    }

    // 공격 시도
    void TryAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;

        Transform target = FindClosestEnemyInRange();
        if (target != null)
        {
            AttackTarget(target);
            lastAttackTime = Time.time;
        }
    }

    // 공격 실행
    void AttackTarget(Transform target)
    {
        _animator.SetTrigger("Attack");

        Vector3 spawnPos = transform.position + Vector3.up * 1.5f;
        Vector3 direction = (target.position - spawnPos).normalized;

        Vector3 lookPos = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.LookAt(lookPos);
        Quaternion rotation = Quaternion.LookRotation(direction);

        GameObject fireball = Instantiate(fireballPrefab, spawnPos, Quaternion.LookRotation(direction));

        GroudonState state = GetComponent<GroudonState>();

        GroudonFireBall skill = fireball.GetComponent<GroudonFireBall>();

        if (state != null && skill != null)
        {
            skill.SetOwner(state);
        }

        Rigidbody rb = fireball.GetComponent<Rigidbody>();
        if (rb != null)
            rb.linearVelocity = direction * fireballSpeed;
    }

    // 가장 가까운 적 찾기
    Transform FindClosestEnemyInRange()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist <= attackRange + 1 && dist < minDist)
            {
                closest = enemy.transform;
                minDist = dist;
            }
        }

        return closest;
    }

    // 사거리 원형 표시 설정
    void SetupRangeLineRenderer()
    {
        rangeLineRenderer = GetComponent<LineRenderer>();
        if (rangeLineRenderer == null) return;

        rangeLineRenderer.positionCount = 50;
        rangeLineRenderer.loop = true;
        rangeLineRenderer.useWorldSpace = true;
        rangeLineRenderer.startWidth = 0.05f;
        rangeLineRenderer.endWidth = 0.05f;

        Material mat = new Material(Shader.Find("Unlit/Color"));
        mat.color = new Color(0f, 0.5f, 1f, 1f);
        rangeLineRenderer.material = mat;
        rangeLineRenderer.enabled = false;
    }

    // 마우스 클릭 시 사거리 원 표시
    void ToggleRangeCircle()
    {
        if (rangeLineRenderer == null) return;

        rangeLineRenderer.enabled = isClicked;
        if (!isClicked) return;

        int segments = rangeLineRenderer.positionCount;
        float angleStep = 360f / segments;

        for (int i = 0; i < segments; i++)
        {
            float angle = Mathf.Deg2Rad * i * angleStep;
            float x = Mathf.Cos(angle) * attackRange;
            float z = Mathf.Sin(angle) * attackRange;
            Vector3 pos = transform.position + new Vector3(x, 0.1f, z);
            rangeLineRenderer.SetPosition(i, pos);
        }
    }
}
