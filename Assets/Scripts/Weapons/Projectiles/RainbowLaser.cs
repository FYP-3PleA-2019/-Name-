using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowLaser : Projectile
{
    #region Rainbow Laser Variables
    [Header("Rainbow Laser")]
    public LayerMask destroyableLayer;

    public LineRenderer lineRenderer;

    #endregion

    public override void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        UpdateLaser();
    }

    public override void Update()
    {
        UpdateLaser();
    }

    private void UpdateLaser()
    {
        Transform shootPoint = GameManager.Instance.player.weapon.GetShootPoint();
        
        shootDir = InputManager.Instance.GetShootDir();

        RaycastHit2D hit = Physics2D.Raycast(shootPoint.position, shootDir, range, destroyableLayer);
        
        if (hit.collider != null)
        {
            lineRenderer.SetPosition(0, shootPoint.position);
            lineRenderer.SetPosition(1, hit.collider.transform.position);
        }
        else
        {
            lineRenderer.SetPosition(0, shootPoint.position);
            lineRenderer.SetPosition(1, shootPoint.position + (shootDir * range));
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
