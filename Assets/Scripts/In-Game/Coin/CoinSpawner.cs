﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coin;
    public float dropDuration;
    private int _amountToSpawn;

    private void Start()
    {
        _amountToSpawn = 5; //Set with Game Manager or something
        SpawnCoins();
    }

    public void SpawnCoins()
    {
        for (int i = 0; i < _amountToSpawn; i++)
        {
            GameObject GO = Instantiate(coin, transform.position, Quaternion.identity);
            GO.transform.parent = transform;
            SetForce(GO);
            StartCoroutine(DeactivateGravity(GO));
        }

        Destroy(gameObject, 1.0f);
    }

    void SetForce(GameObject GO)
    {
        float forceX = Random.Range(-500, 500);
        float forceY = Random.Range(-500, 500);
        Vector2 force = new Vector2(forceX, forceY);
        GO.GetComponent<Rigidbody2D>().AddForce(force);
    }

    IEnumerator DeactivateGravity(GameObject GO)
    {
        yield return new WaitForSeconds(dropDuration);
        GO.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        GO.transform.parent = null;
    }
}
