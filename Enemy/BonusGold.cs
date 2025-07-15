using System.Collections;
using UnityEngine;

public class BonusGold : MonoBehaviour
{
    Rigidbody _rigidbody;

    // ȸ��
    float rotationDirection;
    float rotationSpeed;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        // ȸ���� ����
        rotationDirection = Random.Range(0, 2) == 0 ? -1 : 1; 
        rotationSpeed = Random.Range(50f, 100f);

        // Ƣ���������
        float posX = Random.Range(-1f, 1f);
        float posY = Random.Range(10f, 15f);
        float posZ = Random.Range(-1f, 1f);
        _rigidbody.AddForce(new Vector3(posX, posY, posZ), ForceMode.Impulse);

        Destroy(gameObject, 1);

    }

    void Update()
    {
        // ȸ��
        transform.Rotate(0, rotationDirection * rotationSpeed * Time.deltaTime, 0);

        // Ƣ������� ���� (-6.5 ~ ���Ѵ�� Ƣ����� �� ����)
        Vector3 goldRange = transform.position;
        goldRange.y = Mathf.Clamp(goldRange.y, -6.5f, Mathf.Infinity);

        transform.position = goldRange;
    }
}
