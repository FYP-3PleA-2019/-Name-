using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    #region General Variables
    [Header("General Variables")]
    private bool facingLeft;

    private Transform weaponHolder;
    private Transform shootPoint;
    #endregion

    private void Awake()
    {
        weaponHolder = GetComponentsInChildren<Transform>()[1];
        shootPoint = GetComponentsInChildren<Transform>()[3];
    }

    private void Update()
    {
        Rotate();
    }

    public void Flip(bool facingLeft)
    {
        this.facingLeft = facingLeft;

        float rotationY = 0f;

        if (facingLeft) rotationY = 180f;
        else if (!facingLeft) rotationY = 0f;

        weaponHolder.eulerAngles = new Vector3(0f, rotationY, 0f);
    }

    public void Rotate()
    {
        Vector3 playerPos = GameManager.Instance.player.transform.position;

        float rotationZ = 0f;
        
        if (facingLeft) rotationZ = Mathf.Atan2(playerPos.y, -playerPos.x) * Mathf.Rad2Deg;
        else rotationZ = Mathf.Atan2(playerPos.y, playerPos.x) * Mathf.Rad2Deg;

        weaponHolder.eulerAngles = new Vector3(0f, weaponHolder.eulerAngles.y, rotationZ);
    }
}
