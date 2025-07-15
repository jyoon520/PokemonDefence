using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class ArticunoSkill1 : MonoBehaviour
{
    public ArticunoState owner;

    public int basedamage = 25;

    public void SetOwner(ArticunoState articuno)
    {
        owner = articuno;
    }
    void OnTriggerEnter(Collider other)
    {
        // 쉴드 에너미의 경우 쉴드 제거된 후 공격하도록 함
        if (other.CompareTag("Enemy"))
        {
            EnemyCtrl enemy = other.GetComponent<EnemyCtrl>();

            if (enemy != null)
            {
                if (enemy.bDestroyShield || !enemy.bShieldEnemy)
                {
                    Vector3 hitPoint = other.ClosestPoint(transform.position);

                    int damage = basedamage;

                    // owner가 설정되어 있으면 레벨 기반 추가 데미지
                    if (owner != null)
                    {
                        damage += (owner.level - 1) * 3;
                        enemy.ChangeRed();

                        // 버프 색상 여부
                        enemy.ApplyDotDamage(damage, owner.isBuffActive);
                    }
                    else
                    {
                        // fallback 데미지
                        enemy.ApplyDotDamage(damage, false);
                    }

                    Destroy(gameObject, 1f);
                }
            }
        }
    }
}
