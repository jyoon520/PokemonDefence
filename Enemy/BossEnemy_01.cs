using UnityEngine;

public class BossEnemy_01 : EnemyCtrl
{
    protected override void InitEnemy()
    {
        _maxHp = 500;
        _currentHp = _maxHp;
        _moveSpeed = 2.5f;
        _originMoveSpeed = _moveSpeed;
        bShieldEnemy = false;
    }
}
