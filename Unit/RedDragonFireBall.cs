using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RedDragonFireBall : MonoBehaviour
{
    Animator _animator; // Animator로 변경
    public GameObject fireBallPrefab;
    public Transform fireBallPos;
    public float shotSpeed = 500f;
    public float attackRange = 10f;
    public string enemyTag = "Enemy";

    private float attackCooldown = 1.5f;
    private float lastAttackTime;

    private LineRenderer rangeLineRenderer;
    private bool isClicked = false;

    void Start()
    {
        _animator = GetComponent<Animator>(); // Animator 가져오기

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
                ShootAtTarget(target);
                lastAttackTime = Time.time;
            }
        }

        if (isClicked)
        {
            rangeLineRenderer.enabled = true;
            DrawRangeCircle();
        }
        else
        {
            rangeLineRenderer.enabled = false;
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

    void ShootAtTarget(Transform target)
    {
        Vector3 direction = (target.position - fireBallPos.position).normalized;

        Vector3 lookPos = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.LookAt(lookPos);

        _animator.SetTrigger("Attack"); // 애니메이션 트리거 설정

        Quaternion rotation = Quaternion.LookRotation(direction);
        GameObject fireball = Instantiate(fireBallPrefab, fireBallPos.position, rotation);

        //  본체에 붙은 상태 컴포넌트 가져오기
        CharmanderState state = GetComponent<CharmanderState>();
        CharmeleonState state2 = GetComponent<CharmeleonState>();
        ChespinState state3 = GetComponent<ChespinState>();
        GreninjaState state4 = GetComponent<GreninjaState>();
        LucarioState state5 = GetComponent<LucarioState>();
        MewtwoState state6 = GetComponent<MewtwoState>();   
        SceptileState state7 = GetComponent<SceptileState>();
        TotodileState state8 = GetComponent<TotodileState>();   


        //  발사체에 붙은 스킬 컴포넌트 가져오기
        CharmenderSkill skill = fireball.GetComponent<CharmenderSkill>();
        CharmeleonSkill skill2 = fireball.GetComponent<CharmeleonSkill>();
        ChespinSkill skill3 = fireball.GetComponent<ChespinSkill>();
        GreninjaSkill skill4 = fireball.GetComponent<GreninjaSkill>();
        LucarioSkill skill5 = fireball.GetComponent<LucarioSkill>();
        MewtwoSkill skill6 = fireball.GetComponent<MewtwoSkill>();
        SceptileSkill skill7 = fireball.GetComponent<SceptileSkill>();
        TotodileSkill skill8 = fireball.GetComponent<TotodileSkill>();

        // 발사체의 owner지정
        if (state != null && skill != null)
        {
            skill.SetOwner(state);
        }

        if (state2 != null && skill2 != null)
        {
            skill2.SetOwner(state2);
        }

        if (state3 != null && skill3 != null)
        {
            skill3.SetOwner(state3);
        }

        if (state4 != null && skill4 != null)
        {
            skill4.SetOwner(state4);
        }

        if (state5 != null && skill5 != null)
        {
            skill5.SetOwner(state5);
        }

        if (state6 != null && skill6 != null)
        {
            skill6.SetOwner(state6);
        }

        if (state7 != null && skill7 != null)
        {
            skill7.SetOwner(state7);
        }

        if (state8 != null && skill8 != null)
        {
            skill8.SetOwner(state8);
        }

        Rigidbody rb = fireball.GetComponent<Rigidbody>();
        rb.AddForce(direction * shotSpeed);

        Destroy(fireball, 2.0f);
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
