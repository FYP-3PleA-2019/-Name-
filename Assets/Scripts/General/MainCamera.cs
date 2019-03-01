using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //pX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref currentVel.x, smoothX);
        //pY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref currentVel.y, smoothY);

        Vector3 playerPos = GameManager.Instance.player.transform.position;
        transform.position = new Vector3(playerPos.x, playerPos.y, transform.position.z);
    }
}
