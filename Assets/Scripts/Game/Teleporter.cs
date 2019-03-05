using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Teleporter : MonoBehaviour, ISubject
{
    #region observer
    public void Notify(NOTIFY_TYPE type)
    {
        for (int i = 0; i < UIManager.Instance.registeredObserver.Count; i++)
        {
            UIManager.Instance.registeredObserver[i].OnNotify(type);
        }
    }
    #endregion

    #region General Variables
    [Header("General")]
    public bool isActivated;
    public bool interactable;

    public string indicatorText;

    public GameObject indicator;
    public Transform connectedTeleporter;
    public GAME_SCENE gatewayTo;

    private Animator _animator;
    #endregion

    private void Awake()
    {
        if (_animator == null)
            _animator = gameObject.GetComponent<Animator>();

        indicator.GetComponentInChildren<Text>().text = indicatorText;
    }

    private void Start()
    {
        UIManager.Instance.RegisterSubject(this);

        indicator.SetActive(false);
    }

    // -------------------------------- Functions --------------------------------
    
    IEnumerator WaitForInteract()
    {
        while(!InputManager.Instance.HasInteracted())
        {
            yield return null;
        }

        InputManager.Instance.SetHasInteracted(false);
        Interacted();
    }

    void TeleportPlayer()
    {
        GameManager.Instance.player.transform.position = connectedTeleporter.transform.position;
    }

    void Interacted()
    {
        Notify(NOTIFY_TYPE.UI_SHOOT_BUTTON);
        StopCoroutine("WaitForInteract");

        _animator.SetTrigger("Interacted");
        StartCoroutine(DisableUI());
        CustomSceneManager.Instance.LoadSceneWait(gatewayTo, 0.5f);
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
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && isActivated)
        {
            EnableUI();

            if (interactable)
            {
                Notify(NOTIFY_TYPE.UI_INTERACT_BUTTON);
                StartCoroutine("WaitForInteract");
            }
            else
            {
                TeleportPlayer();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(DisableUI());

            if (interactable)
            {
                Notify(NOTIFY_TYPE.UI_SHOOT_BUTTON);
                StopCoroutine("WaitForInteract");
            }
        }
    }
}
