using UnityEngine;

internal class ObjectLayer
{
    public static int Default;

    public static int Player;

    public static int Enemy;

    public static int EnemyBox;

    public static int IgnoreEnemy;

    public static int IgnorePlayer;

    public static int IgnoreRaycast;

    public static int IgnoreBullets;

    public static int Pumpkin;


    public static void Initialize()
    {
        Default = LayerMask.NameToLayer("Default");
        Player = LayerMask.NameToLayer("Player");
        Enemy = LayerMask.NameToLayer("Enemy");
        EnemyBox = LayerMask.NameToLayer("EnemyBox");
        IgnoreEnemy = LayerMask.NameToLayer("Ignore Enemy");
        IgnorePlayer = LayerMask.NameToLayer("Ignore Player");
        IgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
        IgnoreBullets = LayerMask.NameToLayer("Ignore Bullets");
        Pumpkin = LayerMask.NameToLayer("Pumpkin");
    }
public void Awake()
    {
        ObjectLayer.Initialize();
    }
}
