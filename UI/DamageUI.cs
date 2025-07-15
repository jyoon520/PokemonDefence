using TMPro;
using UnityEngine;

public class DamageUI : MonoBehaviour
{
    public TMP_Text damageText;

    void Start()
    {
        // 아웃라인 설정
        damageText.outlineWidth = 0.5f;
        damageText.outlineColor = Color.black;

        // 2초 후 제거
        Destroy(gameObject, 2f);
    }

    void Update()
    {
        transform.position += new Vector3(1 * Time.deltaTime, 1 * Time.deltaTime, 1 * Time.deltaTime);
    }

    public void SetDamage(int damage, bool isBuffed)
    {
        // 도트 데미지는 노란색으로 표시 (도트 데미지 : 5 ~ 9)
        if (damage <= 9)
        {
            damageText.text = $"{damage}";
            damageText.color = Color.yellow;
            damageText.fontSize = isBuffed ? 0.7f : 0.5f;
        }
        else
        { 
            damageText.text = $"{damage}";
            damageText.color = isBuffed ? Color.red : Color.white;
            damageText.fontSize = isBuffed ? 0.7f : 0.5f;
        }
    }
}
