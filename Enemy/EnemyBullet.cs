using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    // ��ũ��Ʈ
    TowerCtrl _towerCtrl;

    Transform _target; // �� ��ġ
    float _speed = 20f; // �ӵ�

    private void Start()
    {
        // ��ũ��Ʈ
        _towerCtrl = FindObjectOfType<TowerCtrl>();

        // �̸����� Ÿ��(�� ��ġ) ã��
        _target = GameObject.Find("TowerPosition").transform;
    }

    void Update()
    {
        if (_target != null)
        {
            // ���� ����
            Vector3 direction = (_target.position - transform.position).normalized;
            transform.position += direction * _speed * Time.deltaTime;

            // ���� ���� �� ����
            if (Vector3.Distance(transform.position, _target.position) < 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }

    // Ÿ���� �Ѿ� �浹 ��
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Define.Tower))
        {
            int bulletDamage = 20; // �ҷ� ������ 20
            _towerCtrl.GetDamage(bulletDamage);
        }
    }
}
