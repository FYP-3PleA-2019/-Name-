using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentsRandomizer : MonoBehaviour
{
    #region Variables
    public List<GameObject> objects;
    public List<GameObject> spawnPoints;
    public List<Enemy> enemyList;

    [Space(5)]
    [Header("Next Room Variables")]
    public Transform nextRoomSpawnPoint;

    [Header("Next Room SpawnDirection")]
    public SpawnPoint _spawnPoint;
    #endregion

    //Temporary
    public int enemyValue;
    public int minimumValue;

    #region Unity Functions
    // Start is called before the first frame update
    private void Awake()
    {
        //Setting next room's spawn point
        float roomSize = gameObject.GetComponent<BoxCollider2D>().size.x;

        if (_spawnPoint == SpawnPoint.Top)
            nextRoomSpawnPoint.position = new Vector3(transform.position.x, transform.position.y + roomSize, 0.0f);

        else if (_spawnPoint == SpawnPoint.Bottom)
            nextRoomSpawnPoint.position = new Vector3(transform.position.x, transform.position.y - roomSize, 0.0f);

        else if (_spawnPoint == SpawnPoint.Left)
            nextRoomSpawnPoint.position = new Vector3(transform.position.x - roomSize, transform.position.y, 0.0f);

        else if (_spawnPoint == SpawnPoint.Right)
            nextRoomSpawnPoint.position = new Vector3(transform.position.x + roomSize, transform.position.y, 0.0f);
    }

    void Start()
    {
        FillRoom();
    }
    #endregion

    #region Custom Functions
    void FillRoom()
    {
        if (enemyValue == 0)
            return;

        while(enemyValue - minimumValue >= 0)
        {
            int randObj = Random.Range(0, objects.Count);
            int randPoint = Random.Range(0, spawnPoints.Count);

            GameObject GO = Instantiate(objects[randObj], spawnPoints[randPoint].transform.position, Quaternion.identity);
            GO.transform.parent = gameObject.transform; //Set spawned objects as children of the room.

            //Add enemy into enemy list
            enemyList.Add(GO.GetComponent<Enemy>());

            int enemyVal = GO.GetComponent<EnemyBase>().myValue;

            if (enemyValue - enemyVal < 0)
                Destroy(GO);

            else
                enemyValue -= GO.GetComponent<EnemyBase>().myValue;

            //Remove position that was randomed to avoid spawning next object at the same position
            spawnPoints.Remove(spawnPoints[randPoint]);
        }

        //Clear lists to free up memory
        spawnPoints.Clear();
        objects.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            RoomManager.Instance.EnteredRoomChecker(this.gameObject);

            //for(int i = 0; i < enemyList.Count; i++)
            //{
            //    enemyList[i].SetEnemyState(ENEMY_STATE.MOVE);
            //}
        }
    }
    #endregion

    #region Gizmos
    public virtual void OnDrawGizmosSelected() //Draw a cube that serves as an attack range indicator in the [Scene View]
    {
        float roomSize = gameObject.GetComponent<BoxCollider2D>().size.x;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(roomSize, roomSize, 0));
    }
    #endregion 
}
