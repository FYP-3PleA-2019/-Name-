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
    
    public IEnumerator coroutine;
    public SpriteRenderer sprite;
    public PlayerCoreController player;
    #endregion

    private void Awake()
    {
        coroutine = Fade();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        player = GameManager.Instance.player;

        Reset();
    }

    public void Reset()
    {
        DecreaseAlpha();

        Vector3 playerPos = player.transform.position;
        Vector3 shootDir = InputManager.Instance.GetShootDir();
        transform.position = playerPos + (shootDir * offsetPos);
    }

    public void Move()
    {
        Vector3 playerPos = player.transform.position;
        Vector3 shootDir = InputManager.Instance.GetShootDir();
        Vector3 movePos = playerPos + (shootDir * offsetPos);

        transform.position = Vector3.MoveTowards(transform.position , movePos, 1.5f); // Method 1
        //transform.Translate(playerPos + (direction * offset)); //Method 2
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
}
