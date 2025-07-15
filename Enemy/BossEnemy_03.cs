using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossEnemy_03 : EnemyCtrl
{
    public GameObject enemyBullet_02; // ���Ÿ� ���� �Ѿ�
    public GameObject shield; // ����
    int _shieldHitCount = 0; // ���� Ÿ�� ī��Ʈ

    protected override void InitEnemy()
    {
        _maxHp = 1500;
        _currentHp = _maxHp;
        _moveSpeed = 0.5f;
        _originMoveSpeed = _moveSpeed;

        string sceneName = SceneManager.GetActiveScene().name; // �� �̸�

        // �������� 3
        if (sceneName == Define.Stage_03)
        {
            // 3-10
            if (_spawnManager.currentWave == 10)
            {
                LongRangeAttack();
            }
        }
    }

    void LongRangeAttack()
    {
        StartCoroutine(CoRepeatAttack());
    }

    IEnumerator CoRepeatAttack()
    {
        while (isLive)
        {
            yield return new WaitForSeconds(7f); // 7�ʿ� �� ���� ���Ÿ� ����

            Instantiate(enemyBullet_02, transform.position, Quaternion.identity);
        }
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
