using System.Collections;
using UnityEngine;

public class FireDamage : MonoBehaviour
{
    // 스크립트
    EnemyCtrl _enemyCtrl;
    GroudonState _groudonState;

    // 데미지
    float _timer;
    float _damageInterval = 1f; // 데미지 주는 간격
    int _dotDamage; // 도트 데미지 양
    bool _bDestroy = false;

    // 불 위치
    Vector3 _offSet = new Vector3(0, 1, 0);

    void Start()
    {
        _enemyCtrl = GetComponentInParent<EnemyCtrl>();
        _groudonState = FindObjectOfType<GroudonState>();

        _timer = 0f;
    }

    void Update()
    {
        StartCoroutine(CoDestroyFire());

        if (!_enemyCtrl.isLive || _bDestroy)
        {
            Destroy(gameObject);
            return;
        }

        // 에너미 따라가기
        transform.position = _enemyCtrl.transform.position + _offSet;

        // 1초마다 한 번씩 데미지 주기
        _dotDamage = Random.Range(5, 9); // 도트 데미지
        _timer += Time.deltaTime;

        if (_timer >= _damageInterval)
        {
            // 버프 상태일 경우 데미지 텍스트색상 변경
            if (_groudonState.isBuffActive)
            {
                _enemyCtrl.ApplyDotDamage(_dotDamage, true);
            }
            else
            {
                _enemyCtrl.ApplyDotDamage(_dotDamage, false);
            }

            _timer = 0f;
        }
    }

    IEnumerator CoDestroyFire()
    {
        // 5초동안 1초에 한 번씩 데미지 주기
        yield return new WaitForSeconds(5f);

        _bDestroy = true;
        _enemyCtrl.isOnFire = false;
    }
}
