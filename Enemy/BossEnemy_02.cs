using UnityEngine;

public class BossEnemy_02 : EnemyCtrl
{
    public GameObject shield; // 쉴드
    int _shieldHitCount = 0; // 쉴드 타격 카운트

    protected override void InitEnemy()
    {
        _maxHp = 1000;
        _currentHp = _maxHp;
        _moveSpeed = 2.5f;
        _originMoveSpeed = _moveSpeed;
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
