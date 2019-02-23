using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowLaser : Projectile
{
    #region Rainbow Laser Variables
    [Header("Rainbow Laser")]
    public LineRenderer lineRenderer;

    #endregion

    public override void Start()
    {
        shootDir = InputManager.Instance.GetShootDir();
        lineRenderer = GetComponent<LineRenderer>();

        RaycastHit2D hit = Physics2D.Raycast(transform.position, shootDir);

        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + shootDir);




        
        /*for (int i = 1; i < laserSegmentLength; i++)
        {
            var pos = new Vector3(i * 1.2f, Mathf.Sin(i + Time.time * Random.Range(0.5f, 1.3f)), 0f);
            laserLineRendererArc.SetPosition(i, pos);
        }*/
    }

    public override void Update()
    {
        
    }
}
