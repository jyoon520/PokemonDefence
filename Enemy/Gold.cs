using UnityEngine;

public class Gold : MonoBehaviour
{
    float _speed = 10f;
    Vector3 _targetPos = new Vector3(2.05f, 4.85f, 1.9f);

    void Update()
    {
        // 회전
        transform.Rotate(0, 100 * Time.deltaTime, 0);

        // 이동
        transform.position = Vector3.MoveTowards(transform.position, _targetPos, Time.deltaTime * _speed);

        // 도착 체크
        if (Vector3.Distance(transform.position, _targetPos) < 0.01f)
        {
            Destroy(gameObject);
        }
    }
}
