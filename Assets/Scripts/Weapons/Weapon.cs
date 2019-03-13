using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
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

    protected bool canShoot;
    #endregion

    // -------------------------------- Setters --------------------------------


    // -------------------------------- Getters --------------------------------

    public float GetDamage()
    {
        return damage;
    }

    public float GetFireRange()
    {
        return fireRange;
    }

    public float GetFireRate()
    {
        return fireRate;
    }

    public GameObject GetProjectile()
    {
        return projectile;
    }

    // -------------------------------- Functions --------------------------------

    public void Reset()
    {
        canShoot = true;
    }

    public abstract IEnumerator Shoot(Transform shootPoint);
}
