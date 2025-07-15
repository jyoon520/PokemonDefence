using System.Collections;
using UnityEngine;

public class FireDamage : MonoBehaviour
{
    // ��ũ��Ʈ
    EnemyCtrl _enemyCtrl;
    GroudonState _groudonState;

    // ������
    float _timer;
    float _damageInterval = 1f; // ������ �ִ� ����
    int _dotDamage; // ��Ʈ ������ ��
    bool _bDestroy = false;

    // �� ��ġ
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

        // ���ʹ� ���󰡱�
        transform.position = _enemyCtrl.transform.position + _offSet;

        // 1�ʸ��� �� ���� ������ �ֱ�
        _dotDamage = Random.Range(5, 9); // ��Ʈ ������
        _timer += Time.deltaTime;

        if (_timer >= _damageInterval)
        {
            // ���� ������ ��� ������ �ؽ�Ʈ���� ����
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
        // 5�ʵ��� 1�ʿ� �� ���� ������ �ֱ�
        yield return new WaitForSeconds(5f);

        _bDestroy = true;
        _enemyCtrl.isOnFire = false;
    }
}
