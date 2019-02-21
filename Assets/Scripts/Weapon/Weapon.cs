using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    #region General Variables
    [Header("General")]
    public bool pierce;

    public float damage;
    public float fireRange;
    public float fireRate;

    public int fireCost;
    public int areaOfEffect;
    public int cost;

    public string weaponName;

    public Sprite sprite;
    #endregion

    private void Start()
    {

    }

    // -------------------------------- Functions --------------------------------


}
