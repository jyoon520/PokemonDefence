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
        // ���� ���ʹ��� ��� ���� ���ŵ� �� �����ϵ��� ��
        if (other.CompareTag("Enemy"))
        {
            EnemyCtrl enemy = other.GetComponent<EnemyCtrl>();

            if (enemy != null)
            {
                if (enemy.bDestroyShield || !enemy.bShieldEnemy)
                {
                    Vector3 hitPoint = other.ClosestPoint(transform.position);

                    int damage = basedamage;

                    // owner�� �����Ǿ� ������ ���� ��� �߰� ������
                    if (owner != null)
                    {
                        damage += (owner.level - 1) * 3;
                        enemy.ChangeRed();

                        // ���� ���� ����
                        enemy.ApplyDotDamage(damage, owner.isBuffActive);
                    }
                    else
                    {
                        // fallback ������
                        enemy.ApplyDotDamage(damage, false);
                    }

                    Destroy(gameObject, 1f);
                }
            }
        }
    }
}
