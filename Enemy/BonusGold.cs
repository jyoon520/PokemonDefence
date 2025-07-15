using System.Collections;
using UnityEngine;

public class BonusGold : MonoBehaviour
{
    Rigidbody _rigidbody;

    // 회전
    float rotationDirection;
    float rotationSpeed;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        // 회전값 설정
        rotationDirection = Random.Range(0, 2) == 0 ? -1 : 1; 
        rotationSpeed = Random.Range(50f, 100f);

        // 튀어오르도록
        float posX = Random.Range(-1f, 1f);
        float posY = Random.Range(10f, 15f);
        float posZ = Random.Range(-1f, 1f);
        _rigidbody.AddForce(new Vector3(posX, posY, posZ), ForceMode.Impulse);

        Destroy(gameObject, 1);

    }

    void Update()
    {
        // 회전
        transform.Rotate(0, rotationDirection * rotationSpeed * Time.deltaTime, 0);

        // 튀어오르는 높이 (-6.5 ~ 무한대로 튀어오를 수 있음)
        Vector3 goldRange = transform.position;
        goldRange.y = Mathf.Clamp(goldRange.y, -6.5f, Mathf.Infinity);

        transform.position = goldRange;
    }
}
