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
        // ���� ����� Enemy ã��
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
            // Ÿ���� ������ٸ� �׳� ����
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            return;
        }

        // ���� ���
        Vector3 direction = (target.position - transform.position).normalized;

        // ȸ�� ����
        Quaternion toRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotateSpeed * Time.deltaTime);

        // ����
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // �浹 ���� ���
            Vector3 hitPoint = other.ClosestPoint(transform.position);

            // ����Ʈ ����
            Instantiate(waterEffect, hitPoint, Quaternion.identity);

            // �ڱ� �ڽ� ����
            Destroy(gameObject);
        }
    }
}
