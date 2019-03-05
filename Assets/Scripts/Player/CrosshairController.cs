using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairController : MonoBehaviour {

    #region General Variables
    [Header("General")]
    public float offsetPos;
    public float minAlpha;
    public float fadeDelay;
    public float fadeRange; //default = 0.05f;
    
    private SpriteRenderer sprite;
    public List<GameObject> enemyList;
    #endregion

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        enemyList = new List<GameObject>();
    }

    private void Start()
    {

    }

    public void Reset()
    {
        DecreaseAlpha();

        Vector3 playerPos = GameManager.Instance.player.transform.position;
        Vector3 shootDir = InputManager.Instance.GetShootDir();
        transform.position = playerPos + (shootDir * offsetPos);
    }

    public void Move()
    {
        Vector3 playerPos = GameManager.Instance.player.transform.position;
        Vector3 shootDir = InputManager.Instance.GetShootDir();
        Vector3 movePos = playerPos + (shootDir * offsetPos);

        transform.position = movePos;
        //transform.position = Vector3.MoveTowards(transform.position , movePos, 1.5f); // Method 1
        //transform.Translate(playerPos + (direction * offset)); //Method 2
    }

    public void FindNearestEnemy()
    {
        int index = 0;
        float distance = int.MaxValue;

        for(int i = 0; i < enemyList.Count; i++)
        {
            float tempDistance = Vector3.Distance(GameManager.Instance.player.transform.position, enemyList[i].transform.position);

            if(tempDistance < distance)
            {
                index = i;
                distance = tempDistance;
            }
        }

        CalculateDistance(enemyList[index].transform.position);
        InputManager.Instance.SetIsFreeAim(false);
    }

    void CalculateDistance(Vector3 enemyPos)
    {
        Vector3 playerPos = GameManager.Instance.player.transform.position;

        Vector3 dir = enemyPos - playerPos;

        dir.Normalize();

        InputManager.Instance.SetShootDir(dir);
    }

    public void IncreaseAlpha()
    {
        StopCoroutine("Fade");

        Color newColor = sprite.color;
        newColor.a = 1.0f;
        sprite.color = newColor;
    }

    //When the crosshair is not transparent, decrease aplha
    public void DecreaseAlpha()
    {
        if(sprite.color.a == 1.0f) StartCoroutine("Fade");
    }

    public IEnumerator Fade()
    {
        yield return new WaitForSeconds(fadeDelay);

        for (float i = sprite.color.a; i > minAlpha; i -= fadeRange)
        {
            Color newColor = sprite.color;
            newColor.a = i;
            sprite.color = newColor;
            yield return new WaitForSeconds(fadeRange);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            if(collision.gameObject.GetComponent<EnemyBase>() != null)
            {
                enemyList.Add(collision.gameObject);

                FindNearestEnemy();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            if (collision.gameObject.GetComponent<EnemyBase>() != null)
            {
                enemyList.Remove(collision.gameObject);

                if(enemyList.Count <= 0)
                {
                    InputManager.Instance.SetIsFreeAim(true);
                }
            }
        }
    }
}
