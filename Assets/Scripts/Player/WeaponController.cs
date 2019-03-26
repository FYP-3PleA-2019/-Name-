using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    //!!!Player's Shoot Control!!!

    #region Player Variables
    [Header("Weapon")]
    public Weapon currWeapon;
    public Weapon prevWeapon;

    private SpriteRenderer weaponSprRdr;

    private Transform weaponHolder;
    private Transform shootPoint;

    private bool facingLeft;
    #endregion

    private void Awake()
    {
        weaponSprRdr = GetComponent<SpriteRenderer>();

        weaponHolder = transform.parent;
        shootPoint = GetComponentsInChildren<Transform>()[1];
    }

    // Start is called before the first frame update
    void Start()
    {
        //Reset();
    }

    // -------------------------------- Setters --------------------------------

    //Set weapon facing
    public void SetFacingLeft(bool facingLeft)
    {
        this.facingLeft = facingLeft;
        Flip(facingLeft);
    }

    public void SetCurrentWeapon(Weapon weaponToSet)
    {
        currWeapon = weaponToSet;
        currWeapon.Reset();

        UpdateSprite();
    }

    // -------------------------------- Getters --------------------------------

    public Transform GetShootPoint()
    {
        return shootPoint;
    }

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

        UpdateSprite();

        currWeapon.Reset();
        prevWeapon.Reset();
    }

    public void OnShootBegin()
    {
        StartCoroutine(currWeapon.Shoot());
    }

    public void OnShootEnd()
    {
        //StopCoroutine(currWeapon.Shoot(shootPoint));
    }

    public void SwitchWeapon()
    {
        SoundManager.instance.playSingle(SoundManager.instance.weaponSwitch);
        Weapon temp = currWeapon;
        currWeapon = prevWeapon;
        prevWeapon = temp;

        currWeapon.Reset();
        prevWeapon.Reset();

        UpdateSprite();
    }

    public void Rotate()
    {
        float rotationZ;

        Vector3 shootDir = InputManager.Instance.GetShootDir();

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

    private void Flip(bool facingLeft)
    {
        float rotationY = 0f;

        if (facingLeft) rotationY = 180f;
        else if (!facingLeft) rotationY = 0f;

        weaponHolder.eulerAngles = new Vector3(0f, rotationY, 0f);
    }

    private void UpdateSprite()
    {
        weaponSprRdr.sprite = currWeapon.GetSprite();
    }
}
