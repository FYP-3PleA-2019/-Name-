using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum ChestType
//{
//    REWARD,
//    RISK
//}

public class ChestDrops : MonoBehaviour {

    #region Variables
    [System.Serializable]
    public class itemPool
    {
        public string name;
        public GameObject dropItems;
        public int itemRarity;
    }

    public List<itemPool> itemList = new List<itemPool>();
    private Animator _animator;
    private bool chestOpened;
    //public ChestType chestType;
    #endregion

    #region Unity Functions
    private void Start()
    {
        chestOpened = false;
        _animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
        //Test purposes
		if(Input.GetKeyDown(KeyCode.E))
        {
            dropProb();
        }
	}
    #endregion

    #region Custom Functions
    public void dropProb()
    {
        if(chestOpened)
        {
            return;
        }

        SoundManager.instance.playSingle(SoundManager.instance.chestOpen);
        chestOpened = true;
        _animator.SetTrigger("Interact");

        int dropRarity = 0;

        for(int i = 0; i < itemList.Count; i++)
        {
            dropRarity += itemList[i].itemRarity;

        }

        int randVal = Random.Range(0, dropRarity);

        for(int j = 0; j < itemList.Count; j++)
        {
            if(randVal <= itemList[j].itemRarity)
            {
                Instantiate(itemList[j].dropItems, transform.position, Quaternion.identity);
                return;
            }
            randVal -= itemList[j].itemRarity;
        }
    }
    #endregion
}
