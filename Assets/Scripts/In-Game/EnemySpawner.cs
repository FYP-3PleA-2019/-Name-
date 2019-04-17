using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private Animator _animator;
    private Transform spawnPoint;
    private ComponentsRandomizer myParent;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        spawnPoint = GetComponentsInChildren<Transform>()[1];
        myParent = GetComponentsInParent<ComponentsRandomizer>()[0];
    }

    public void SpawnEnemy(Enemy enemyToSpawn)
    {
        // _animator.SetTrigger("Spawning");
        Enemy enemyObject = Instantiate(enemyToSpawn, spawnPoint.position, Quaternion.identity);
        enemyObject.transform.SetParent(transform.parent);
        enemyObject.IsSpawned = true;
        myParent.ExistingEnemies++;
    }
}
