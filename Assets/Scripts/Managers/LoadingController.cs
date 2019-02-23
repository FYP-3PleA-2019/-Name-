using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingController : MonoBehaviour
{
    public float waitTime;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("LoadLogo");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LoadLogo()
    {
        yield return new WaitForSeconds(waitTime);

        GameManager.Instance.SetGameState(GAME_STATE.MAIN_MENU);
    }
}
