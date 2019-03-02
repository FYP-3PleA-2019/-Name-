using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOffPlatform : MonoBehaviour
{
    public GameObject movingPlatform;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if(movingPlatform.GetComponent<MovingPlatform>().isGrounded != true)
                Destroy(other.gameObject);
        }
    }
}
