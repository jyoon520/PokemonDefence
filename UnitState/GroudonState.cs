using UnityEngine;

public class GroudonState : MonoBehaviour
{
    public int id;
    public int level;
    public int baseATK;
    public int atk;

    // 버프 처리
    public bool isBuffActive = false; // 중복 버프 체크

    GameObject _unitBuffEffectInstance; // 생성된 이펙트 인스턴스 참조
    public GameObject unitBuffEffectPrefab; // 버프 이펙트 프리팹


    void OnEnable()
    {
        level = UnitLevelManager.Instance.GetLevel(id);
        Debug.Log($"[CharmanderState] ID {id} 레벨: {level}");
        atk = baseATK + (level - 1) * 3;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 중복 버프 플래그 체크
        if (other.gameObject.CompareTag("Buff") && !isBuffActive)
        {
            ApplyBuff();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // 중복 버프 플래그 체크
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

        // 이펙트 생성
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

        // 이펙트 제거
        if (_unitBuffEffectInstance != null)
        {
            Destroy(_unitBuffEffectInstance);
            _unitBuffEffectInstance = null;
        }
    }
}