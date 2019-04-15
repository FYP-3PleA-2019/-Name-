using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponController : MonoBehaviour
{
    #region General Variables
    [Header("General Variables")]
    public Weapon weapon;

    private bool facingLeft;

    private Transform _weaponHolder;
    private Transform _weapon;
    private Transform _shootPoint;
    #endregion

    private void Awake()
    {
        _weaponHolder = GetComponentsInChildren<Transform>()[1];
        _weapon = GetComponentsInChildren<Transform>()[2];
        _shootPoint = GetComponentsInChildren<Transform>()[3];
    }

    private void Start()
    {
        _weapon.GetComponent<SpriteRenderer>().sprite = weapon.GetSprite();
    }

    private void Update()
    {
        Rotate();
    }

    public void Reset()
    {
        SetFacingLeft(false);
        weapon.Reset();
    }

    public void SetFacingLeft(bool facingLeft)
    {
        this.facingLeft = facingLeft;
        Flip(facingLeft);
    }

    public void Rotate()
    {
        Vector3 playerPos = GameManager.Instance.player.transform.position;

        Vector3 dir = (playerPos - _weaponHolder.position).normalized;

        float rotationZ = 0f;
        
        if (facingLeft) rotationZ = Mathf.Atan2(dir.y, -dir.x) * Mathf.Rad2Deg;
        else rotationZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        _weaponHolder.eulerAngles = new Vector3(0f, _weaponHolder.eulerAngles.y, rotationZ);
    }

    public void Shoot()
    {
        StartCoroutine(weapon.Shoot(_shootPoint));
    }

    private void Flip(bool facingLeft)
    {
        float rotationY = 0f;

        if (facingLeft) rotationY = 180f;
        else if (!facingLeft) rotationY = 0f;

        _weaponHolder.eulerAngles = new Vector3(0f, rotationY, _weaponHolder.eulerAngles.z);
    }
}
