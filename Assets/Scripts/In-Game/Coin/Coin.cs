using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue;
    [Range(0, 10)] public float attractionRange;
    [Range(0, 15)] public float moveSpeed;
    private Transform player;
    private bool canMove;
    private float dropDuration;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        dropDuration = transform.parent.GetComponent<CoinSpawner>().dropDuration;
        canMove = false;
        StartCoroutine(Wait());
    }

    private void Update()
    {
        if (canMove)
        {
            float distanceFromPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceFromPlayer < attractionRange)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Player" && canMove)
        {
            if(GameManager.Instance.currGameState == GAME_STATE.IN_GAME)
            {
                GameManager.Instance.GameCoins += coinValue;
            }

            else
            {
                GameManager.Instance.Coins += coinValue;
                UIManager.Instance.coinUI.UpdateCoinUI();
            }

            Destroy(gameObject);
            //TO-DO :Play Player UI and coin UI
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(dropDuration);
        canMove = true;
    }
}
