using UnityEngine;

public class WaterBall : MonoBehaviour
{
    public GameObject waterEffect;

    public float moveSpeed = 10f;
    public float rotateSpeed = 5f;
    public string targetTag = "Enemy";

    private Transform target;

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
            // 타겟이 사라졌다면 그냥 직진
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            return;
        }

        // 방향 계산
        Vector3 direction = (target.position - transform.position).normalized;

        // 회전 보간
        Quaternion toRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotateSpeed * Time.deltaTime);

        // 전진
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // 충돌 지점 계산
            Vector3 hitPoint = other.ClosestPoint(transform.position);

            // 이펙트 생성
            Instantiate(waterEffect, hitPoint, Quaternion.identity);

            // 자기 자신 삭제
            Destroy(gameObject);
        }
    }
}
