using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCoreController : MonoBehaviour
{
    #region General Variables
    [Header("General")]
    public PlayerController controller;
    public CrosshairController crosshair;
    public WeaponController weapon;
    #endregion

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        crosshair = GetComponentInChildren<CrosshairController>();
        weapon = GetComponentInChildren<WeaponController>();
    }

    // -------------------------------- Functions --------------------------------

    public void Reset()
    {
        controller.Reset();
        crosshair.Reset();
        weapon.Reset();
    }
}