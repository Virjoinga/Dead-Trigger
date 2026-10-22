using UnityEngine;

internal static class ObjectLayer
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


    // Unity does not invoke Awake on this utility class. Initialize before scene
    // components cache masks or collect enemy colliders.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void AutoInitialize() { Initialize(); }

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
}
