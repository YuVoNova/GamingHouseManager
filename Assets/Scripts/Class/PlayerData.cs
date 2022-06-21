public class PlayerData
{
    public const int BoothCount = 4;
    public const int BoothLevelCount = 4;
    public const int MainHubLevelCount = 2;

    public bool IsTutorial;

    public int Money;
    public int TournamentsWon;

    public int[] BoothLevels = new int[BoothCount];
    public int[][] BoothPrices = new int[BoothCount][];

    public int MainHubLevel;
    public int[] MainHubPrices = new int[MainHubLevelCount];

    public PlayerData()
    {
        IsTutorial = true;

        Money = 500;
        TournamentsWon = 0;

        for (int i = 0; i < BoothCount; i++)
        {
            BoothLevels[i] = 0;
        }

        //BoothLevels[0] = 1;

        BoothPrices[0] = new int[BoothLevelCount] { 500, 1000, 1500, 3000 };
        BoothPrices[1] = new int[BoothLevelCount] { 500, 1000, 1500, 3000 };
        BoothPrices[2] = new int[BoothLevelCount] { 500, 1000, 1500, 3000 };
        BoothPrices[3] = new int[BoothLevelCount] { 500, 1000, 1500, 3000 };

        MainHubLevel = 0;
        MainHubPrices = new int[MainHubLevelCount] { 2000, 3000 };
    }
}
