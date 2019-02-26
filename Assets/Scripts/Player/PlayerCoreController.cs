using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCoreController : MonoBehaviour
{
    #region General Variables
    [Header("General")]
    [HideInInspector] public PlayerController controller;
    [HideInInspector] public CrosshairController crosshair;
    [HideInInspector] public WeaponController weapon;
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