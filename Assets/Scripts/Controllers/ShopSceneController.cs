using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSceneController : MonoBehaviour
{
    #region General
    [Header("General")]
    public Transform shopSpawnPoint;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.player.controller.PlayTeleportAnimation();
        UIManager.Instance.controlUI.ShowCanvas();
        GameManager.Instance.player.transform.position = new Vector2(shopSpawnPoint.position.x + .1f, shopSpawnPoint.position.y + 0.45f);
        UIManager.Instance.transitionUI.PlayTransitionAnimation(1);
    }
}
