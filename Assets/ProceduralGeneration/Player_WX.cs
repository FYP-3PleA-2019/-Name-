using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_WX : MonoBehaviour
{
    public int health;
    public GameObject enemy;

    private void Start()
    {
        health = 3;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            enemy.GetComponent<EnemyBase>().ReceiveDamage(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Room")
        {
            RoomManager.Instance.EnteredRoomChecker(other.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "IcePlatform")
        {
            other.GetComponent<AreaEffector2D>().forceAngle = FaceMouse();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "IcePlatform")
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
        }
    }

    float FaceMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector2 direction = new Vector2(
            mousePos.x - transform.position.x,
            mousePos.y - transform.position.y
            );

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        return angle;
    }
}
