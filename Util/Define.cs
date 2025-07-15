using UnityEngine;

public class Define
{
    #region Tag
    public const string Enemy = "Enemy";
    public const string Bullet = "Bullet";
    public const string Tower = "Tower";
    public const string IceBullet = "IceBullet";
    public const string IceTile = "IceTile";
    public const string FireBullet = "FireBullet";
    #endregion

    #region Animation
    // Enemy
    public readonly static int attack = Animator.StringToHash("attack");
    public readonly static int death = Animator.StringToHash("death");
    public readonly static int getHit = Animator.StringToHash("getHit");
    #endregion

    #region Scene
    public const string Lobby = "Lobby";
    public const string Stage_01 = "Stage_01";
    public const string Stage_02 = "Stage_02";
    public const string Stage_03 = "Stage_03";
    public const string BonusStage = "BonusStage";
    #endregion
}
