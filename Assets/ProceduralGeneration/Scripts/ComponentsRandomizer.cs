using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentsRandomizer : MonoBehaviour
{
    #region Variables
    public bool dontSpawnEnemies;

    public List<GameObject> objects;
    public List<Transform> spawnPoints;
    public List<Enemy> enemyList;
    private List<Enemy> spawnedList;
    private List<EnemySpawner> spawnerList;

    public GameObject enemySpawner;
    private Door door;
    public bool haveDoor;

    [Space(5)]
    [Header("Next Room Variables")]
    public Transform nextRoomSpawnPoint;

    [Header("Next Room SpawnDirection")]
    public SpawnPoint _spawnPoint;
    #endregion

    //Temporary
    [Space(5)]
    [Header("Spawning Related")]
    public float spawnInterval;
    private int enemyValue;
    public int minimumValue;
    public int noOfSpawners;
    public int noOfObjects;

    public int ExistingEnemies
    {
        get { return _existingEnemies; }
        set
        {
            _existingEnemies = value;
        }
    }
    private int _existingEnemies;

    private bool _finishedSpawning;

    #region Unity Functions
    // Start is called before the first frame update
    private void Awake()
    {
        spawnedList = new List<Enemy>();
        spawnerList = new List<EnemySpawner>();

        if (haveDoor) // Meaning there will be enemies
        {
            door = GetComponentInChildren<Door>();
        }

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

    private void Start()
    {
        FillRoom();
        CheckRoomClear();

        enemyValue = minimumValue + (GameManager.Instance.Score * 3);
    }
    #endregion

    #region Custom Functions
    void FillRoom()
    {
        SpawnChecker();

        if (noOfSpawners > 0) //Check if room is intended to spawn enemies
            InstantiateEnemySpawner();

        if (objects.Count > 0) //Check if room is intended to spawn objects
        {
            if (noOfObjects > spawnPoints.Count)
                noOfObjects = spawnPoints.Count;

            if (noOfObjects > 0)
            {
                for (int i = 0; i < noOfObjects; i++)
                    SpawnItems();
            }
        }

        //Clear lists to free up memory
        spawnPoints.Clear();
        objects.Clear();
    }

    void InstantiateEnemySpawner()
    {
        if (enemySpawner == null)
        {
            Debug.Log("EnemySpawner prefab is not assigned!");
            return;
        }

        for (int i = 0; i < noOfSpawners; i++)
        {
            int randPoint = Random.Range(0, spawnPoints.Count);
            Transform tempPos = spawnPoints[randPoint];
            GameObject spawner = Instantiate(enemySpawner, tempPos.position, Quaternion.identity);
            spawner.transform.parent = this.transform;

            spawnerList.Add(spawner.GetComponent<EnemySpawner>());

            spawnPoints.Remove(spawnPoints[randPoint]);
        }
    }

    private IEnumerator SpawnEnemies()
    {
        if(enemyList.Count < 1)
        {
            Debug.Log("No enemy is assigned!");
            yield break;
        }
        
        while (enemyValue - minimumValue >= 0)
        {
            int randEnemy = Random.Range(0, enemyList.Count);
            int randSpawner = Random.Range(0, spawnerList.Count);

            Enemy tempEnemy = enemyList[randEnemy];
            int enemyVal = enemyList[randEnemy].myValue;

            if(enemyValue - enemyVal >= 0)
            {
                randSpawner = Random.Range(0, spawnerList.Count);
                spawnerList[randSpawner].SpawnEnemy(tempEnemy);
                enemyValue -= enemyVal;
                yield return new WaitForSeconds(spawnInterval);
            }
        }

        _finishedSpawning = true;
    }
    
    private void SpawnItems()
    {
        if(spawnPoints.Count < 1)
        {
            Debug.Log("No SpawnPoints to spawn object");
            return;
        }

        int randObject = Random.Range(0, objects.Count);
        int randPoint = Random.Range(0, spawnPoints.Count);

        GameObject tempObject = objects[randObject];
        Vector3 tempPos = spawnPoints[randPoint].position;

        GameObject GO = Instantiate(tempObject, tempPos, Quaternion.identity);

        spawnPoints.Remove(spawnPoints[randPoint]);

        GO.transform.parent = this.transform;
    }

    void SpawnChecker()
    {
        if (noOfSpawners > 0)
            _finishedSpawning = false;

        else
            _finishedSpawning = true;
    }

    public void CheckRoomClear()
    {
        if(_existingEnemies <= 0 && _finishedSpawning && haveDoor)
        {
            door.OpenDoor();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            RoomManager.Instance.EnteredRoomChecker(this.gameObject);

            if (!dontSpawnEnemies)
            {
                if (noOfSpawners > 0) //Only start spawning enemies when player enters the platform
                    StartCoroutine(SpawnEnemies());
            }
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
