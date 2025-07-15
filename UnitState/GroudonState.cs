using UnityEngine;

public class GroudonState : MonoBehaviour
{
    public int id;
    public int level;
    public int baseATK;
    public int atk;

    // ���� ó��
    public bool isBuffActive = false; // �ߺ� ���� üũ

    GameObject _unitBuffEffectInstance; // ������ ����Ʈ �ν��Ͻ� ����
    public GameObject unitBuffEffectPrefab; // ���� ����Ʈ ������


    void OnEnable()
    {
        level = UnitLevelManager.Instance.GetLevel(id);
        Debug.Log($"[CharmanderState] ID {id} ����: {level}");
        atk = baseATK + (level - 1) * 3;
    }

    private void OnTriggerEnter(Collider other)
    {
        // �ߺ� ���� �÷��� üũ
        if (other.gameObject.CompareTag("Buff") && !isBuffActive)
        {
            ApplyBuff();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // �ߺ� ���� �÷��� üũ
        if (other.gameObject.CompareTag("Buff") && !isBuffActive)
        {
            ApplyBuff();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Buff"))
        {
            RemoveBuff();
        }
    }

    void ApplyBuff()
    {
        level += 10;
        atk = baseATK + (level - 1) * 3;
        isBuffActive = true;

        // ����Ʈ ����
        if (unitBuffEffectPrefab != null && _unitBuffEffectInstance == null)
        {
            Vector3 effectPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.2f);

            _unitBuffEffectInstance = Instantiate(unitBuffEffectPrefab, effectPosition, Quaternion.identity);
        }
    }


    void RemoveBuff()
    {
        level -= 10;
        atk = baseATK + (level - 1) * 3;
        isBuffActive = false;

        // ����Ʈ ����
        if (_unitBuffEffectInstance != null)
        {
            Destroy(_unitBuffEffectInstance);
            _unitBuffEffectInstance = null;
        }
    }
}