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

    public List<bool> WeaponState
    {
        get { return _weaponState; }
        set { _weaponState = value; }
    }
    private List<bool> _weaponState = new List<bool>(new bool[3]);

    public GameData(GameManager manager)
    {
        _highScore = manager.HighScore;
        _coins = manager.Coins;
        _weaponState = manager.WeaponState;
    }
}
