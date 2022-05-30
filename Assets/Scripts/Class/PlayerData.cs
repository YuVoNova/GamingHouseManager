using UnityEngine;

public class PlayerData
{
    public const int BoothCount = 4;

    public int Money;
    public int TournamentsWon;

    public int[] BoothLevels = new int[BoothCount];
    public int[] BoothPrices = new int[BoothCount];

    public PlayerData()
    {
        Money = 0;
        TournamentsWon = 0;

        for (int i = 0; i < BoothCount; i++)
        {
            BoothLevels[i] = 0;
        }

        BoothPrices[0] = 100;
        BoothPrices[1] = 250;
        BoothPrices[2] = 500;
        BoothPrices[3] = 1000;

        
    }
}
