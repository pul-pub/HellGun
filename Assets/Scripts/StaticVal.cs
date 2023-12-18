using System.Collections.Generic;

public static class StaticVal
{
    //PLAYER SETTINGS
    public static string language = "ru";
    public static bool onAd = false;
    public static float volMusic = 100f;
    public static float sens = 550f;
    public static int money = 0;

    //GAME
    public static int ammo = 200;
    public static int levlEnemy = 0;

    public static int[] inv = { 0, -1 };
    public static int[] shoped = { 0, -1, -1, -1, -1 };
    public static int[] dmEnemy = { 7, 12, 19, 24, 30, 45, 56, 69, 86, 91};
    public static float[] speedEnemy = { 3f, 3.2f, 3.6f, 4f, 4.2f, 4.8f, 5f, 5.8f, 6.2f, 6.5f };

    public static Gun[] gun = {new Gun("AKM", 0, 12f, 30, 0.075f, false, 1f, 0),
                               new Gun("VAL", 3200, 25f, 14, 0.05f, false, 0.8f, 0),
                               new Gun("AK-108", 2000, 18f, 42, 0.08f, false, 1.3f, 0),
                               new Gun("M-16", 4000, 20f, 30, 0.05f, false, 0.9f, 0),
                               new Gun("VSS-Vintorez", 6850, 30f, 12, 0.08f, true, 1.3f, 0) };
}
