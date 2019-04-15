using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Room", menuName = "Room Type")]
public class RoomType : ScriptableObject
{
    #region Variables
    public List<RoomType> connectablePlatformTypes;
    public List<GameObject> platformVariations;
    #endregion

    #region Custom Functions
    public RoomType ReturnConnectableRoomType(ref RoomType currentRoomType) // Returns a randomized connectable room type
    {
        int randNum = Random.Range(0, connectablePlatformTypes.Count);

        currentRoomType = connectablePlatformTypes[randNum]; //Sets current room type to randomized connectable room type

        return connectablePlatformTypes[randNum];
    }

    public GameObject ReturnGO() // Returns a randomized variation of a room type
    {
        int randNum = Random.Range(0, platformVariations.Count);
        
        return platformVariations[randNum];
    }
    #endregion
}
