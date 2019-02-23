using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentsRandomizer : MonoBehaviour
{
    #region Variables
    public List<GameObject> objects;
    public List<GameObject> spawnPoints;

    [Space(5)]
    [Header("Next Room Variables")]
    public Transform nextRoomSpawnPoint;

    [Header("Next Room SpawnDirection")]
    public SpawnPoint _spawnPoint;
    #endregion

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
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            int rand = Random.Range(0, objects.Count);
            Vector3 sp = spawnPoints[i].transform.position;

            GameObject GO = Instantiate(objects[rand], sp, Quaternion.identity) as GameObject;
            GO.transform.parent = gameObject.transform; //Set spawned objects as children of the room.

            //Remove all spawnpoints from scene to reduce excess Game objects
            Destroy(spawnPoints[i]);
        }

        //Clear lists to free up memory
        spawnPoints.Clear();
        objects.Clear();
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
