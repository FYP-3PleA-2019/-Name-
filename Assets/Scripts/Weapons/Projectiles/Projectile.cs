using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    #region General Variables
    [Header("General")]
    protected float moveSpeed;
    protected float damage;
    protected float fireRange;
    protected float fireRate;

    protected Vector3 shootDir;
    #endregion

    // Update is called once per frame
    public virtual void Update()
    {
        transform.Translate(Vector2.right * Time.deltaTime * moveSpeed);
    }

    private void OnBecameInvisible()
    {
        Die();
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetFireRange(float fireRange)
    {
        this.fireRange = fireRange;
    }

    public void SetFireRate(float fireRate)
    {
        this.fireRate = fireRate;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
