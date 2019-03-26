using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum ChestType
//{
//    REWARD,
//    RISK
//}

public class ChestDrops : MonoBehaviour, ISubject {

    #region Observer
    public void Notify(NOTIFY_TYPE type)
    {
        for (int i = 0; i < UIManager.Instance.registeredObserver.Count; i++)
        {
            UIManager.Instance.registeredObserver[i].OnNotify(type);
        }
    }
    #endregion

    #region Variables
    [System.Serializable]
    public class itemPool
    {
        public string name;
        public GameObject dropItems;
        public int itemRarity;
    }

    public List<itemPool> itemList = new List<itemPool>();
    public float waitDuration;
    private Animator _animator;
    private bool chestOpened;
    //public ChestType chestType;
    #endregion

    #region Unity Functions
    private void Start()
    {
        UIManager.Instance.RegisterSubject(this);
        chestOpened = false;
        _animator = gameObject.GetComponent<Animator>();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Notify(NOTIFY_TYPE.UI_INTERACT_BUTTON);

            StartCoroutine(WaitForInteract());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Notify(NOTIFY_TYPE.UI_SHOOT_BUTTON);
        }
    }
    #endregion

    #region Custom Functions
    IEnumerator WaitForInteract()
    {
        while (!InputManager.Instance.HasInteracted())
        {
            yield return null;
        }

        InputManager.Instance.SetHasInteracted(false);
        dropProb();
    }

    IEnumerator WaitToSpawn()
    {
        yield return new WaitForSeconds(waitDuration);
        SpawnObject();
    }

    public void dropProb()
    {
        if(chestOpened)
        {
            return;
        }

        //SoundManager.instance.playSingle(SoundManager.instance.chestOpen);
        chestOpened = true;
        _animator.SetTrigger("Interact");

        StartCoroutine(WaitToSpawn());
        Notify(NOTIFY_TYPE.UI_SHOOT_BUTTON);
    }

    void SpawnObject()
    {
        int dropRarity = 0;

        for (int i = 0; i < itemList.Count; i++)
        {
            dropRarity += itemList[i].itemRarity;
        }

        int randVal = Random.Range(0, dropRarity);

        for (int j = 0; j < itemList.Count; j++)
        {
            if (randVal <= itemList[j].itemRarity)
            {
                Instantiate(itemList[j].dropItems, transform.position, Quaternion.identity);
                return;
            }
            randVal -= itemList[j].itemRarity;
        }
    }
    #endregion
}
