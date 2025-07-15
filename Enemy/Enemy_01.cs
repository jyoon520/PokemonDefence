using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy_01 : EnemyCtrl
{
    protected override void InitEnemy()
    {
        _maxHp = 20;
        _currentHp = _maxHp;
        bShieldEnemy = false;

        string sceneName = SceneManager.GetActiveScene().name; // 씬 이름

        // 스테이지 1
        if (sceneName == Define.Stage_01)
        {
            //1-5, 1-6, 1-7, 1-8, 1-9
            if (_spawnManager.currentWave == 5 || _spawnManager.currentWave == 6 || _spawnManager.currentWave == 7 || _spawnManager.currentWave == 8 || _spawnManager.currentWave == 9)
            {
                _moveSpeed = 2.5f;
                _originMoveSpeed = _moveSpeed;
            }
            // 기본 이동속도
            else
            {
                _moveSpeed = 1f;
                _originMoveSpeed = _moveSpeed;
            }
        }

        // 스테이지 2
        if (sceneName == Define.Stage_02)
        {
            // 2-3, 2-4, 2-5
            if (_spawnManager.currentWave == 3 || _spawnManager.currentWave == 4 || _spawnManager.currentWave == 5)
            {
                _moveSpeed = 2.5f;
                _originMoveSpeed = _moveSpeed;
            }
            else
            {
                _moveSpeed = 1f;
                _originMoveSpeed = _moveSpeed;
            }
        }

        // 스테이지 3
        if (sceneName == Define.Stage_03)
        {
            // 3-3, 3-4
            if (_spawnManager.currentWave == 3 || _spawnManager.currentWave == 4)
            {
                _moveSpeed = 2.5f;
                _originMoveSpeed = _moveSpeed;
            }
            else
            {
                _moveSpeed = 1f;
                _originMoveSpeed = _moveSpeed;
            }
        }
    }
}

