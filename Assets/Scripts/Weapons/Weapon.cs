using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
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

    public GameObject projectile;
    public Sprite sprite;
    #endregion

    // -------------------------------- Setters --------------------------------
    

    // -------------------------------- Getters --------------------------------
    public float GetFireRate()
    {
        return fireRate;
    }

    public GameObject GetProjectile()
    {
        return projectile;
    }

    public float GetDamage()
    {
        return damage;
    }

    // -------------------------------- Checkers --------------------------------


    // -------------------------------- Functions --------------------------------
    public virtual IEnumerator Shoot()
    {
        Debug.Log("gggg");

        yield return null;
    }
}
