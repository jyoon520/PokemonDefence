using UnityEngine;

public class BossEnemy_02 : EnemyCtrl
{
    public GameObject shield; // ����
    int _shieldHitCount = 0; // ���� Ÿ�� ī��Ʈ

    protected override void InitEnemy()
    {
        _maxHp = 1000;
        _currentHp = _maxHp;
        _moveSpeed = 2.5f;
        _originMoveSpeed = _moveSpeed;
    }

    // ���� ī��Ʈ ó�� �� ������ ó��
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Define.Bullet))
        {
            _shieldHitCount++;

            // 10�� �̻� ���带 ������
            if (_shieldHitCount >= 10)
            {
                // ���� ����
                DestroyShield();
                bDestroyShield = true;
            }
        }

        // �� ���� ����
        if (other.gameObject.CompareTag(Define.FireBullet) && bDestroyShield)
        {
            ChangeRed();

            if (!isOnFire)
            {
                Vector3 firePosition = new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z);

                GameObject fire = Instantiate(fireDamage, firePosition, Quaternion.identity, transform);
                isOnFire = true;
            }
        }
    }

    void DestroyShield()
    {
        Destroy(shield);
    }
}
