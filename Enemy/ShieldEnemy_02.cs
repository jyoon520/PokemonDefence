using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShieldEnemy_02 : EnemyCtrl
{
    public GameObject enemyBullet_01; // ���Ÿ� ���� �Ѿ�
    public GameObject shield; // ����
    int _shieldHitCount = 0; // ���� Ÿ�� ī��Ʈ
    
    protected override void InitEnemy()
    {
        _maxHp =300;
        _currentHp = _maxHp;
        _moveSpeed = 1f;
        _originMoveSpeed = _moveSpeed;

        string sceneName = SceneManager.GetActiveScene().name; // �� �̸�

        // �������� 3
        if (sceneName == Define.Stage_03)
        {
            // 3-9
            if (_spawnManager.currentWave == 9)
            {
                if (Random.Range(0f, 1f) <= 0.1f) // 10% Ȯ���� ���Ÿ� ����
                {
                    LongRangeAttack();
                }
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
            yield return new WaitForSeconds(5f);

            Instantiate(enemyBullet_01, transform.position, Quaternion.identity);
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
                GameObject fire = Instantiate(fireDamage, transform.position, Quaternion.identity, transform);
                isOnFire = true;
            }
        }
    }

    void DestroyShield()
    {
        Destroy(shield);
    }
}
