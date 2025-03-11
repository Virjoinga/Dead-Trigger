internal static class ObjectLayerMask
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

	static ObjectLayerMask()
	{
		Default = 1 << ObjectLayer.Default;
		Player = 1 << ObjectLayer.Player;
		Enemy = 1 << ObjectLayer.Enemy;
		EnemyBox = 1 << ObjectLayer.EnemyBox;
		IgnoreEnemy = 1 << ObjectLayer.IgnoreEnemy;
		IgnorePlayer = 1 << ObjectLayer.IgnorePlayer;
		IgnoreRaycast = 1 << ObjectLayer.IgnoreRaycast;
		IgnoreBullets = 1 << ObjectLayer.IgnoreBullets;
		Pumpkin = 1 << ObjectLayer.Pumpkin;
	}
}
