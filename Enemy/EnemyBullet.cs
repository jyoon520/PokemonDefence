using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    // 스크립트
    TowerCtrl _towerCtrl;

    Transform _target; // 성 위치
    float _speed = 20f; // 속도

    private void Start()
    {
        // 스크립트
        _towerCtrl = FindObjectOfType<TowerCtrl>();

        // 이름으로 타겟(성 위치) 찾기
        _target = GameObject.Find("TowerPosition").transform;
    }

    void Update()
    {
        if (_target != null)
        {
            // 성문 공격
            Vector3 direction = (_target.position - transform.position).normalized;
            transform.position += direction * _speed * Time.deltaTime;

            // 성문 도착 시 제거
            if (Vector3.Distance(transform.position, _target.position) < 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }

    // 타워에 총알 충돌 시
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Define.Tower))
        {
            int bulletDamage = 20; // 불렛 데미지 20
            _towerCtrl.GetDamage(bulletDamage);
        }
    }
}
