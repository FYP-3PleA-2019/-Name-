using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Teleporter : MonoBehaviour
{
    private Animator _animator;
    private Transform target;

    //public Transform connectedTeleporter;
    public GameObject indicator;
    public string indicatorText;

    private void Start()
    {
        if(_animator == null)
            _animator = gameObject.GetComponent<Animator>();

        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        indicator.GetComponentInChildren<Text>().text = indicatorText;
        indicator.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Interacted();
    }

    public void Interacted()
    {
        _animator.SetTrigger("Interacted");
        StartCoroutine(DisableUI());
        //ChangeScene();
    }

    void EnableUI()
    {
        indicator.SetActive(true);
    }

    IEnumerator DisableUI()
    {
        indicator.GetComponent<Animator>().SetTrigger("Close");

        yield return new WaitForSeconds(0.1f);
        indicator.SetActive(false);
    }

    //void TeleportObject(GameObject obj)
    //{
    //    obj.transform.position = connectedTeleporter.transform.position;
    //}

    void ChangeScene(/*Scene*/)
    {
        //Change scene via SceneManager
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            EnableUI();
            //Call player interact button
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(DisableUI());
            //Call player interact button
        }
    }
}
