using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount;
    public float shakeFactor;

    public Color targetColor;

    private Color originalColor;
    
    // Start is called before the first frame update
    void Start()
    {
        originalColor = Camera.main.backgroundColor;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = GameManager.Instance.player.transform.position;
        transform.position = new Vector3(playerPos.x, playerPos.y, transform.position.z);
    }

    public void CameraShake(float index, float distance, float minDistToShakeCamera)
    {
        transform.position = transform.position + (Random.insideUnitSphere * shakeAmount * (index * shakeFactor));

        float lerp = mapValue(distance, 0, minDistToShakeCamera, 0f, 1f);

        Color lerpColor = Color.Lerp(targetColor, originalColor, lerp);

        Camera.main.backgroundColor = lerpColor;

        //float r = ((targetColorR / 255f) - originalColor.r) * 1 / minDistToShakeCamera;
        //float g = ((targetColorG / 255f) - originalColor.g) * 1 / minDistToShakeCamera;
        //float b = ((targetColorB / 255f) - originalColor.b) * 1 / minDistToShakeCamera; 

        //GetComponent<Camera>().backgroundColor = new Color(originalColor.r + r * index, originalColor.g + g * index, originalColor.b + b * index);
    }

    float mapValue(float mainValue, float inValueMin, float inValueMax, float outValueMin, float outValueMax)
    {
        return (mainValue - inValueMin) * (outValueMax - outValueMin) / (inValueMax - inValueMin) + outValueMin;
    }
}
