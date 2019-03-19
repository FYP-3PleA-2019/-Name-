using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlurScript : MonoBehaviour
{
    private Canvas blurCanvas;
    private Image blurImage;

    public float BlurDuration
    {
        get { return _blurDuration; }
        set
        {
            _blurDuration = value;
        }
    }
    private float _blurDuration;

    public float BlurSize
    {
        get { return _blurSize; }
        set
        {
            _blurSize = value;
        }
    }
    private float _blurSize;

    private void Start()
    {
        blurCanvas = GetComponent<Canvas>();
        blurImage = GetComponentInChildren<Image>();
        _blurDuration = 0;
        _blurSize = 0;
    }

    public void Reset()
    {
        blurImage.material.SetFloat("_Size", 0.0f);
    }

    public IEnumerator StartBlur()
    {
        float currBlurSize = blurImage.material.GetFloat("_Size");
        float blurDifference = _blurSize - currBlurSize;
        float sizeToIncrease = blurDifference / (_blurDuration / Time.deltaTime);

        while (currBlurSize < _blurSize)
        {
            blurImage.material.SetFloat("_Size", currBlurSize + sizeToIncrease);
            currBlurSize = blurImage.material.GetFloat("_Size"); ;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    public IEnumerator EndBlur()
    {
        float currBlurSize = blurImage.material.GetFloat("_Size");
        float sizeToIncrease = _blurSize / (_blurDuration / Time.deltaTime);

        while (currBlurSize > 0.0f)
        {
            blurImage.material.SetFloat("_Size", currBlurSize - sizeToIncrease);
            currBlurSize = blurImage.material.GetFloat("_Size"); ;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    public void EnableCanvas()
    {
        blurCanvas.enabled = true;
    }

    public void DisableCanvas()
    {
        blurCanvas.enabled = false;
    }
}
