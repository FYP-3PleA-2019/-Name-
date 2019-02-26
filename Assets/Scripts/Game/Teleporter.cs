using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TeleporterState
{
    Deactivated,
    Activated
}

public class Teleporter : MonoBehaviour
{
    private Animator _animator;
    private Transform target;

    public Transform connectedTeleporter;
    public float interactableRange;

    private TeleporterState state;

    private void Start()
    {
        if(_animator == null)
            _animator = gameObject.GetComponent<Animator>();

        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        if (connectedTeleporter == null)
            state = TeleporterState.Deactivated;

        else
            state = TeleporterState.Activated;
    }

    private void Update()
    {
        switch(state)
        {
            case TeleporterState.Activated:
                Activated();
                break;

            default:
                Deactivated();
                break;
        }
    }

    void Deactivated()
    {
        //_animator.SetTrigger("Deactivated");
    }

    void Activated()
    {
        //_animator.SetTrigger("Activated");

        float distanceFromTarget = Vector2.Distance(transform.position, target.position);

        if (distanceFromTarget <= interactableRange)
            Debug.Log("hi");
            //Observer call player main button
    }

    public void TeleportObject(GameObject obj)
    {
        obj.transform.position = connectedTeleporter.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (state == TeleporterState.Deactivated)
            return;

        if (other.tag == "Player")
            TeleportObject(other.gameObject);
    }
}
