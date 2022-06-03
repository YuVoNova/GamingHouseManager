using UnityEngine;

public class PlayerData
{
    public const int BoothCount = 4;
    public const int BoothLevelCount = 4;
    public const int MainHubLevelCount = 2;

    public int Money;
    public int TournamentsWon;

    public int[] BoothLevels = new int[BoothCount];
    public int[][] BoothPrices = new int[BoothCount][];

    public int MainHubLevel;
    public int[] MainHubPrices = new int[MainHubLevelCount];

    public PlayerData()
    {
        Money = 0;
        TournamentsWon = 0;

        for (int i = 0; i < BoothCount; i++)
        {
            BoothLevels[i] = 0;
        }

        BoothPrices[0] = new int[BoothLevelCount] { 100, 200, 300, 400};
        BoothPrices[1] = new int[BoothLevelCount] { 250, 500, 750, 1000};
        BoothPrices[2] = new int[BoothLevelCount] { 500, 1000, 1500, 2000};
        BoothPrices[3] = new int[BoothLevelCount] { 1000, 2000, 3000, 4000};

        MainHubLevel = 0;
        MainHubPrices = new int[MainHubLevelCount] { 500, 1000 };
    }
}
