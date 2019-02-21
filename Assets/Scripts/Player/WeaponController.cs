using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    //!!!Player's Shoot Control!!!

    #region Player Variables
    [Header("Weapon")]
    public float fireRate;

    public Transform weaponHolder;

    public Weapon currWeapon;
    public Weapon prevWeapon;

    private bool facingLeft;
    #endregion

    private void Awake()
    {
        weaponHolder = transform.parent;
    }

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // -------------------------------- Setters --------------------------------

    //Set weapon facing
    public void SetFacingLeft(bool facingLeft)
    {
        this.facingLeft = facingLeft;
        Flip(facingLeft);
    }

    // -------------------------------- Getters --------------------------------


    // -------------------------------- Checkers --------------------------------

    //Check player facing
    public bool FacingLeft()
    {
        bool result = facingLeft;
        return result;
    }

    // -------------------------------- Functions --------------------------------

    public void Reset()
    {
        SetFacingLeft(false);

        Rotate();
    }

    public void Rotate()
    {
        float rotationZ;

        Vector3 shootDir = InputManager.Instance.shootDir;

        if (shootDir.x < 0 && !FacingLeft()) SetFacingLeft(true);
        else if (shootDir.x > 0 && FacingLeft()) SetFacingLeft(false);

        if (FacingLeft()) rotationZ = Mathf.Atan2(shootDir.y, -shootDir.x) * Mathf.Rad2Deg;
        else rotationZ = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;

        transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, rotationZ);

        #region Slerp rotation
        /*float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);*/
        #endregion
    }

    void Flip(bool facingLeft)
    {
        float rotationY = 0f;

        if (facingLeft) rotationY = 180f;
        else if (!facingLeft) rotationY = 0f;

        weaponHolder.eulerAngles = new Vector3(0f, rotationY, 0f);
    }

    public void Shoot()
    {

    }
}
