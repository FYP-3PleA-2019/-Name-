using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int HighScore
    {
        get { return _highScore; }
        set
        {
            if (value >= 0)
                _highScore = value;
        }
    }
    private int _highScore;

    public int Coins
    {
        get { return _coins; }
        set
        {
            _coins = value;
        }
    }
    private int _coins;

    public GameData(GameManager manager)
    {
        HighScore = manager.HighScore;
        Coins = manager.Coins;
    }
}
