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
        GameManager.Instance.player.transform.position = shopSpawnPoint.position;
    }
}
