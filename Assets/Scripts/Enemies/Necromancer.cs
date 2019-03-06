using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Necromancer : Enemy
{
    #region Variables
    [Header("Spawning Variables")]
    public float spawnRange;
    public GameObject undeadPrefab;
    public int noOfEnemies;
    #endregion

    #region Custom Functions
    public override void Attack()
    {
        attackTimer = 0; //Reset Attack Timer

        //_animator.SetTrigger("Attack"); //Play Attack Animation
        GameObject newEnemy;

        for (int i = 0; i < noOfEnemies; i++)
        {
            Vector2 rand = Random.insideUnitCircle * spawnRange;
            Vector2 randSpawnPoint = new Vector2(transform.position.x + rand.x, transform.position.y + rand.y);

            newEnemy = Instantiate(undeadPrefab, randSpawnPoint, Quaternion.identity) as GameObject; //Spawn minions
        }

        Debug.Log("Attacking!");

        //if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("AttackAnimationName")) //Check to see if animator is still playing attack animation
        idleDuration = Random.Range(1.0f, 2.5f); //Random idle duration
        _currentState = CurrentState.Idle; //Switch state to idle if attack animation is finished playing
    }
    #endregion

    #region Gizmos
    public override void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    #endregion
}
