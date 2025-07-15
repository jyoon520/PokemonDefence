using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShieldEnemy_02 : EnemyCtrl
{
    public GameObject enemyBullet_01; // 원거리 공격 총알
    public GameObject shield; // 쉴드
    int _shieldHitCount = 0; // 쉴드 타격 카운트
    
    protected override void InitEnemy()
    {
        _maxHp =300;
        _currentHp = _maxHp;
        _moveSpeed = 1f;
        _originMoveSpeed = _moveSpeed;

        string sceneName = SceneManager.GetActiveScene().name; // 씬 이름

        // 스테이지 3
        if (sceneName == Define.Stage_03)
        {
            // 3-9
            if (_spawnManager.currentWave == 9)
            {
                if (Random.Range(0f, 1f) <= 0.1f) // 10% 확률로 원거리 공격
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

    // 쉴드 카운트 처리 후 데미지 처리
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Define.Bullet))
        {
            _shieldHitCount++;

            // 10번 이상 쉴드를 맞으면
            if (_shieldHitCount >= 10)
            {
                // 쉴드 제거
                DestroyShield();
                bDestroyShield = true;
            }
        }

        // 불 유닛 공격
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
