using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformType
{
    Static,
    Movable
}

public class FallOffPlatform : MonoBehaviour
{
    public GameObject platform;

    public PlatformType _platformType;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (_platformType == PlatformType.Movable)
            {
                if (platform.GetComponent<MovingPlatform>().isGrounded == false)
                    Destroy(other.gameObject);
            }

            else
                Destroy(other.gameObject);
        }
    }
}
