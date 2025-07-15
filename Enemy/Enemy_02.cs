using UnityEngine;

public class Enemy_02 : EnemyCtrl
{
    protected override void InitEnemy()
    {
        _maxHp = 300;
        _currentHp = _maxHp;
        _moveSpeed = 1f;
        _originMoveSpeed = _moveSpeed;
        bShieldEnemy = false;
    }
}
