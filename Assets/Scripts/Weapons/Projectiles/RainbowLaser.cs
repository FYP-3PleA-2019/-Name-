using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowLaser : Projectile
{
    #region Rainbow Laser Variables
    [Header("Rainbow Laser")]
    public LayerMask destroyableLayer;

    public LineRenderer lineRenderer;

    public Color startColor;
    
    private float intervalCounter;
    #endregion

    public void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        intervalCounter = fireRate;
    }

    public override void Update()
    {
        UpdateLaser();
    }

    private void UpdateLaser()
    {
        intervalCounter += Time.deltaTime;
        Transform shootPoint = GameManager.Instance.player.weapon.GetShootPoint();
        
        shootDir = InputManager.Instance.GetShootDir();

        RaycastHit2D hit = Physics2D.Raycast(shootPoint.position, shootDir, fireRange, destroyableLayer);

        Vector3 startColorChange = new Vector3(Mathf.Clamp(Mathf.Sin(startColor.r * intervalCounter * 5), 0.6f, 0.9f),
             Mathf.Clamp(Mathf.Sin(startColor.g * intervalCounter * 5), 0.3f, 0.7f),
             Mathf.Clamp(Mathf.Sin(startColor.b * intervalCounter * 5), 0.2f, 0.8f));

        Vector3 endColorChange = new Vector3(Mathf.Clamp(Mathf.Cos(startColor.r * intervalCounter * 5), 0.6f, 0.9f),
            Mathf.Clamp(Mathf.Cos(startColor.g * intervalCounter * 5), 0.3f, 0.7f),
            Mathf.Clamp(Mathf.Cos(startColor.b * intervalCounter * 5), 0.2f, 0.8f));

        Color tempStartColor = new Color(startColorChange.x, startColorChange.y, startColorChange.z);
        Color tempEndColor = new Color(endColorChange.x, endColorChange.y, endColorChange.z);
        lineRenderer.startColor = tempStartColor;
        lineRenderer.endColor = tempEndColor;

        if (hit.collider != null)
        {
            lineRenderer.SetPosition(0, shootPoint.position);
            lineRenderer.SetPosition(1, hit.collider.transform.position);

            if (intervalCounter >= fireRate)
            {
                intervalCounter = 0;
                if (hit.collider.tag == "Enemy")
                {
                    hit.collider.GetComponent<EnemyBase>().ReceiveDamage(damage);
                }

                else if (hit.collider.tag == "Generator")
                {
                    hit.collider.GetComponent<Generator>().InitiateGeneratorFunction();
                }
            }
        }
        else
        {
            lineRenderer.SetPosition(0, shootPoint.position);
            lineRenderer.SetPosition(1, shootPoint.position + (shootDir * fireRange));
        }

        lineRenderer.enabled = true;

        #region Electric effect
        /*for (int i = 1; i < laserSegmentLength; i++)
        {
            var pos = new Vector3(i * 1.2f, Mathf.Sin(i + Time.time * Random.Range(0.5f, 1.3f)), 0f);
            laserLineRendererArc.SetPosition(i, pos);
        }*/
        #endregion
    }
}
