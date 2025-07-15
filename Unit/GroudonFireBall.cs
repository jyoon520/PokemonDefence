using UnityEngine;

public class GroudonFireBall : MonoBehaviour
{
    public GameObject explodeEffect;
    public GameObject fireFloorPrefab;
    public float fireFloorDuration = 3f;    

    public float moveSpeed = 10f;
    public float rotateSpeed = 5f;
    public int basedamage = 50;
    public string targetTag = "Enemy";

    private Transform target;
    public GroudonState owner;

    public void SetOwner(GroudonState groudon)
    {
        owner = groudon;
    }

    void Start()
    {
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


                    //  바닥 생성
                    if (fireFloorPrefab != null)
                    {
                        GameObject floor = Instantiate(fireFloorPrefab, hitPoint, Quaternion.identity);
                        Destroy(floor, fireFloorDuration);
                    }

                    Destroy(gameObject);
                }
            }
        }
    }
}
