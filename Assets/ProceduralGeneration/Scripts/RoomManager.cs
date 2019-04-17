using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnPoint
{
    Top,
    Bottom,
    Left,
    Right
}

public class RoomManager : MonoBehaviour
{
    #region Singleton
    private static RoomManager mInstance;

    public static RoomManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject[] tempObjectList = GameObject.FindGameObjectsWithTag("RoomManager");

                if (tempObjectList.Length > 1)
                {
                    Debug.LogError("You have more than 1 RoomManager in the Scene");
                }
                else if (tempObjectList.Length == 0)
                {
                    GameObject obj = new GameObject("_RoomManager");
                    mInstance = obj.AddComponent<RoomManager>();
                    obj.tag = "RoomManager";
                }
                else
                {
                    if (tempObjectList[0] != null)
                    {
                        mInstance = tempObjectList[0].GetComponent<RoomManager>();
                    }
                }
                DontDestroyOnLoad(mInstance.gameObject);
            }
            return mInstance;
        }
    }

    public static RoomManager CheckInstanceExist()
    {
        return mInstance;
    }
    #endregion

    #region Variables
    //Publics
    public List<RoomType> startingRoomTypes;

    //[Space(10)]
    //[Header("Player Related")]
    //public PlayerCoreController player;

    [Space(10)]
    [Header("Room Related")]
    public int roomLimit;
    public int startingNumberOfRooms;
    public int amountToSpawn;
    
    //Privates
    public List<GameObject> spawnedRooms;
    RoomType currentRoomType;

    //Variables below are uncomfirmed / Might be used in latest implementation of enemy spawner system
    private int difficultyLevel;

    private int enemyPoints;
    public int EnemyPoint
    {
        get { return enemyPoints; }
        set
        {
            if (value >= 0)
                enemyPoints = value;
        }
    }
    #endregion

    #region Unity Functions
    void Awake()
    {
        if (RoomManager.CheckInstanceExist())
        {
            Destroy(this.gameObject);
        }
                
        spawnedRooms = new List<GameObject>();
    }
    
    private void Start()
    {
        //if (player == null) //Automatically sets player as target for this game object if it is not assigned in the inspector
        //{
        //    Debug.LogWarning("BEWARE : [Player] have not been assigned in the inspector!");
        //    player = GameManager.Instance.player;
        //}
    }
    #endregion

    #region Custom Functions

    void SpawnRoom()
    {
        int randNum = Random.Range(0, startingRoomTypes.Count); //Random initial room type to spawn
        Transform roomSpawnPoint;
        GameObject roomToSpawn;

        if (spawnedRooms.Count < 1) //If no room is spawned, randomly spawn a type of room
        {
            roomSpawnPoint = GameManager.Instance.player.transform; //Set room's spawn point
            roomToSpawn = startingRoomTypes[randNum].ReturnGO();
            currentRoomType = startingRoomTypes[randNum]; //Set current room type
        }

        else //If there is existing rooms, randomly spawn a room variation based on newest room's connectable room types
        {
            roomSpawnPoint = spawnedRooms[spawnedRooms.Count - 1].GetComponent<ComponentsRandomizer>().nextRoomSpawnPoint; //Set room's spawn point
            roomToSpawn = currentRoomType.ReturnConnectableRoomType(ref currentRoomType).ReturnGO();
        }
        
        GameObject newRoom = Instantiate(roomToSpawn, roomSpawnPoint.position, Quaternion.identity) as GameObject; //Spawn randomized room

        AddRoomToList(spawnedRooms, newRoom); //Add spawned room into list
    }

    public void EnteredRoomChecker(GameObject other)
    {
        if(other == spawnedRooms[spawnedRooms.Count - 1])
        {
            for (int i = 0; i < amountToSpawn; i++)
            {
                SpawnRoom();

                if (spawnedRooms.Count > roomLimit)
                    DespawnRoom();
            }
        }
    }

    void DespawnRoom()
    {
        Destroy(spawnedRooms[0]);
        RemoveRoomFromList(spawnedRooms, spawnedRooms[0]);
    }

    public void AddRoomToList(List<GameObject> roomList, GameObject roomToAdd)
    {
        if(!roomList.Contains(roomToAdd))
            roomList.Add(roomToAdd);
    }

    public void RemoveRoomFromList(List<GameObject> roomList, GameObject roomToRemove)
    {
        if (roomList.Contains(roomToRemove))
            roomList.Remove(roomToRemove);
    }

    public void Initialize()
    {
        for (int i = 0; i < startingNumberOfRooms; i++)
            SpawnRoom();
    }

    public void Reset()
    {
        spawnedRooms.Clear();
    }
    #endregion
}
