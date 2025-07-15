using UnityEngine;

public class ArticunoSkill : MonoBehaviour
{
    Animator _animator;
    public GameObject icePillarPrefab;      // 얼음 기둥 프리팹
    public GameObject iceFloorPrefab;       // 바닥 얼음 프리팹
    public float icePillarDuration = 1.0f;  // 기둥 지속 시간
    public float iceFloorDuration = 2.5f;   // 바닥 얼음 지속 시간
    public float attackRange = 10f;
    public string enemyTag = "Enemy";
    public float attackCooldown = 1.5f;

    private float lastAttackTime;
    private LineRenderer rangeLineRenderer;
    private bool isClicked = false;

    void Start()
    {
        _animator = GetComponent<Animator>();

        rangeLineRenderer = GetComponent<LineRenderer>();
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

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform)
                {
                    isClicked = true;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isClicked = false;
        }

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Transform target = FindClosestEnemyInRange();
            if (target != null)
            {
                AttackTarget(target);
                lastAttackTime = Time.time;
            }
        }

        rangeLineRenderer.enabled = isClicked;
        if (isClicked)
        {
            DrawRangeCircle();
        }
    }

    void DrawRangeCircle()
    {
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

    void AttackTarget(Transform target)
    {
        if (target == null) return;

        _animator.SetTrigger("Attack");

        Vector3 spawnPos = target.position;
        Quaternion targetRotation = target.rotation; // 회전값 저장

        Vector3 direction = (target.position - transform.position).normalized;
        Vector3 lookPos = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.LookAt(lookPos);
        Quaternion rotation = Quaternion.LookRotation(direction);

        // 얼음 기둥 생성 (적 회전 적용)
        GameObject icePillar = Instantiate(icePillarPrefab, spawnPos, targetRotation);

        ArticunoState state = GetComponent<ArticunoState>();

        ArticunoSkill1 skill = icePillar.GetComponent<ArticunoSkill1>();

        if (state != null && skill != null)
        {
            skill.SetOwner(state);
        }

        Destroy(icePillar, icePillarDuration);

        // 바닥 얼음 생성 (회전도 동일하게 적용)
        GameObject iceFloor = Instantiate(iceFloorPrefab, spawnPos, targetRotation);
        Destroy(iceFloor, iceFloorDuration);
    }


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
}
