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

    [Space(5)][Header("Arrow Indicator")]
    public GameObject arrowIndicator;
    public Sprite indicatorSprite;

    private void Awake()
    {
        _animator = gameObject.GetComponent<Animator>();

        indicator.GetComponentInChildren<Text>().text = indicatorText;
    }

    private void Start()
    {
        UIManager.Instance.RegisterSubject(this);

        indicator.SetActive(false);

        InstantiateArrowIndicator();
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
        GameManager.Instance.player.transform.position = new Vector2(connectedTeleporter.transform.position.x,
                                                                        connectedTeleporter.transform.position.y + 10f);
    }

    void Interacted()
    {
        GameManager.Instance.player.controller.playerAnimator.SetTrigger("StartTeleport");
        GameManager.Instance.player.controller.weaponHolder.SetActive(false);

        Notify(NOTIFY_TYPE.UI_SHOOT_BUTTON);
        StopCoroutine("WaitForInteract");

        _animator.SetTrigger("Interacted");
        StartCoroutine(DisableUI());
        InputManager.Instance.SetCanControl(false);
        UIManager.Instance.controlUI.HideCanvas();

        if (gatewayTo == GAME_SCENE.SHOP_SCENE) GameManager.Instance.SetGameState(GAME_STATE.SHOP);
        else if (gatewayTo == GAME_SCENE.GAME_SCENE) GameManager.Instance.SetGameState(GAME_STATE.PAUSED);
        else if (gatewayTo == GAME_SCENE.LOBBY_SCENE) GameManager.Instance.SetGameState(GAME_STATE.LOBBY);

        UIManager.Instance.transitionUI.PlayTransitionAnimation(0);

        CustomSceneManager.Instance.LoadSceneWait(gatewayTo, 1.5f);
        SoundManager.instance.playSingle(SoundManager.instance.playerTeleport);

        //Save progress
        GameManager.Instance.SaveData();
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

    void InstantiateArrowIndicator()
    {
        GameObject indicator = Instantiate(arrowIndicator, transform.position, Quaternion.identity);
        indicator.transform.SetParent(this.transform);
        indicator.GetComponent<ArrowIndicator>().Target = this.transform;
        indicator.GetComponent<ArrowIndicator>().SpriteToDisplay = indicatorSprite;
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
