using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    #region General Variables
    [Header("General")]
    protected float moveSpeed;
    protected float damage;
    protected float range;

    protected Vector3 shootDir;
    #endregion

    // Start is called before the first frame update
    public virtual void Start()
    {

    }

    // Update is called once per frame
    public virtual void Update()
    {
        transform.Translate(Vector2.right * Time.deltaTime * moveSpeed);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetFireRange(float range)
    {
        this.range = range;
    }
}
