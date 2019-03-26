using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Effectors : MonoBehaviour
{
    protected bool canKnockBack;

    protected void KnockBack(Vector2 currPos, float knockBackForce, float knockBackDuration)
    { 
        StartCoroutine(_knockBack(currPos, knockBackForce, knockBackDuration));
    }

    private IEnumerator _knockBack(Vector2 knockBackDir, float knockBackForce, float knockBackDuration)
    {
        if(canKnockBack)
        {
            canKnockBack = false;
            InputManager.Instance.SetCanMove(false);
            Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
            rb.velocity += knockBackDir * knockBackForce;

            yield return new WaitForSeconds(knockBackDuration);
            
            rb.velocity = new Vector2(0f, 0f);
            InputManager.Instance.SetCanMove(true); 
            canKnockBack = true;
        }
    }

    protected void SpriteColorEffect(Color effectColor)
    {
        SpriteRenderer tempSprite = gameObject.GetComponent<SpriteRenderer>();
        tempSprite.color = effectColor;
    }

    protected IEnumerator TemporarySpriteColorEffect(Color effectColor, float effectDuration)
    {
        SpriteRenderer tempSprite = gameObject.GetComponent<SpriteRenderer>();
        tempSprite.color = effectColor;

        yield return new WaitForSeconds(effectDuration);

        tempSprite.color = Color.white;
    }
}
