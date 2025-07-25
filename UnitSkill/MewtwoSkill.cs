using UnityEngine;

public class MewtwoSkill : MonoBehaviour
{
    public GameObject explodeEffect;

    public float moveSpeed = 10f;
    public float rotateSpeed = 5f;
    public int basedamage = 80;
    public string targetTag = "Enemy";

    private Transform target;
    public MewtwoState owner;

    public void SetOwner(MewtwoState mewtwo)
    {
        owner = mewtwo;
    }
    void Start()
    {
        // 가장 가까운 Enemy 찾기
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(targetTag);
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                target = enemy.transform;
            }
        }
    }

    void Update()
    {
        if (target == null)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion toRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotateSpeed * Time.deltaTime);
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

    }

    void OnTriggerEnter(Collider other)
    {
        // 쉴드 에너미의 경우 쉴드 제거된 후 공격하도록 함
        if (other.CompareTag("Enemy"))
        {
            EnemyCtrl enemy = other.GetComponent<EnemyCtrl>();

            if (enemy != null)
            {
                if (enemy.bDestroyShield || !enemy.bShieldEnemy)
                {
                    Vector3 hitPoint = other.ClosestPoint(transform.position);
                    Instantiate(explodeEffect, hitPoint, Quaternion.identity);

                    int damage = basedamage;

                    // owner가 설정되어 있으면 레벨 기반 추가 데미지
                    if (owner != null)
                    {
                        damage += (owner.level - 1) * 3;
                        enemy.ChangeRed();

                        // 버프 색상 여부
                        enemy.ApplyDotDamage(damage, owner.isBuffActive);
                    }
                    else
                    {
                        // fallback 데미지
                        enemy.ApplyDotDamage(damage, false);
                    }

                    Destroy(gameObject);
                }
            }
        }
    }
}
