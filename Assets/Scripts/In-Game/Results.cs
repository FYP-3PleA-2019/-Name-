using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Results : MonoBehaviour
{
    public List<GameObject> resultObjects;
    public GameObject highScoreImage;
    public float delayTime;

    private Animator _animator;
    private Animator _blinkTextAnimator;

    private Text _coinText;
    private Text _scoreText;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _blinkTextAnimator = GetComponentsInChildren<Animator>()[2];

        _scoreText = GetComponentsInChildren<Text>()[0];
        _coinText = GetComponentsInChildren<Text>()[1];

        highScoreImage.SetActive(false);

        DisableAllImages();
    }

    private void OnEnable()
    {
        StartCoroutine(EnableAllImages(delayTime));
    }

    void DisableAllImages()
    {
        for(int i = 0; i < resultObjects.Count; i++)
        {
            resultObjects[i].SetActive(false);
        }
    }

    IEnumerator EnableAllImages(float delay)
    {
        _scoreText.text = GameManager.Instance.Score + "m";
        if (GameManager.Instance.Score > GameManager.Instance.HighScore)
        {
            highScoreImage.SetActive(true);
        }

        _coinText.text = "+" + GameManager.Instance.GameCoins;

        while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Results_Idle"))
        {
            yield return null;
        }

        for(int i = 0; i < resultObjects.Count; i++)
        {
            yield return new WaitForSeconds(delay);

            resultObjects[i].SetActive(true);
        }

        yield return new WaitForSeconds(delay);
        _blinkTextAnimator.SetTrigger("Blink");
    }
}
