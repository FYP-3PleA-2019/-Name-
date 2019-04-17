using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    #region General Variables
    [Header("General")]
    public float offsetPos;
    public float minAlpha;
    public float fadeDelay;
    public float fadeRange; //default = 0.05f;

    public LayerMask destroyableLayer;

    private SpriteRenderer sprite;
    private Transform target;
    #endregion

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {

    }

    void Update()
    {
        //if (GameManager.Instance.GetCurrGameState() != GAME_STATE.IN_GAME)
            //return;

        FindNearest();
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
        if(InputManager.Instance.CanFreeAim())
        {
            Vector3 playerPos = GameManager.Instance.player.transform.position;
            Vector3 shootDir = InputManager.Instance.GetShootDir();
            Vector3 movePos = playerPos + (shootDir * offsetPos);

            transform.position = movePos;
            //transform.position = Vector3.MoveTowards(transform.position , movePos, 1.5f); // Method 1
            //transform.Translate(playerPos + (direction * offset)); //Method 2
        }
        else
        {
            transform.position = target.position;
        }
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
        if (sprite.color.a == 1.0f) StartCoroutine("Fade");
    }
    
    private void FindNearest()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(GameManager.Instance.player.transform.position, 4f, destroyableLayer);
        Debug.Log(colliders.Length);
        if (colliders.Length > 0)
        {
            int index = -1;
            float shortestDist = int.MaxValue;

            for (int i = 0; i < colliders.Length; i++)
            {
                float tempDist = Vector2.Distance(GameManager.Instance.player.transform.position, colliders[i].transform.position);
                
                if (colliders[i].tag == "Generator")
                {
                    if(CheckList(colliders, "Enemy"))
                        continue;
                }

                if (tempDist < shortestDist)
                {
                    index = i;
                    shortestDist = tempDist;
                }
            }

            InputManager.Instance.SetCanFreeAim(false);

            target = colliders[index].transform;
            CalculateDir();
        }
        else InputManager.Instance.SetCanFreeAim(true);
    }

    private bool CheckList(Collider2D[] collider, string tagName)
    {
        for(int i = 0; i < collider.Length; i++)
        {
            if (collider[i].tag == tagName)
                return true;
        }

        return false;
    }

    private void CalculateDir()
    {
        Vector3 dir = target.position - GameManager.Instance.player.transform.position;

        dir.Normalize();

        InputManager.Instance.SetShootDir(dir);
    }

    private IEnumerator Fade()
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
}
