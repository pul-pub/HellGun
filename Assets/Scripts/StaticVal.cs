using UnityEngine;

public static class StaticVal
{
    //PLAYER SETTINGS
    public static string language = "ru";
    public static float volMusic = 100f;
    public static float sens = 800f;
    public static int money = 0;

    //GAME
    public static int ammo = 300;
    public static int kills = 0;
    public static int scoreWithPoint = 0;
    public static int moneyForBattle = 0;
    public static int numEnemy = 11;
    public static bool hiting;

    public static int[] inv = { 0, -1 };
    public static int[] shoped = { 0, -1, -1, -1, -1 };
    public static int[] dmEnemy = { 7, 12, 19, 24, 30, 45, 56, 69, 86, 91};
    public static float[] speedEnemy = { 3f, 3.2f, 3.6f, 4f, 4.2f, 4.8f, 5f, 5.8f, 6.2f, 6.5f };

    public static Gun[] gun = {new Gun("AKM",       0, 12f, 30, 0.075f, false, 1f,   30, new Vector3(-0.54f,  -0.8f, 0)),
                               new Gun("VAL",    3200, 25f, 14, 0.05f,  false, 0.5f, 14, new Vector3(-0.54f, -0.73f, 0)),
                               new Gun("AK-108", 2000, 18f, 42, 0.08f,  false, 1.9f, 42, new Vector3(-0.54f, -0.76f, 0)),
                               new Gun("M-16",   4000, 20f, 30, 0.05f,  false, 0.9f, 30, new Vector3(-0.53f,  -0.8f, 0)),
                               new Gun("FAL",    6850, 30f, 24, 0.08f,  false, 1.3f, 24, new Vector3(-0.53f,  -0.8f, 0)) };
}
