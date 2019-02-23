using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Room", menuName = "Room Type")]
public class RoomType : ScriptableObject
{
    #region Variables
    public List<RoomType> connectableRoomTypes;
    public List<GameObject> roomVariations;
    #endregion

    #region Custom Functions
    public RoomType ReturnConnectableRoomType(ref RoomType currentRoomType) // Returns a randomized connectable room type
    {
        int randNum = Random.Range(0, connectableRoomTypes.Count);

        currentRoomType = connectableRoomTypes[randNum]; //Sets current room type to randomized connectable room type

        return connectableRoomTypes[randNum];
    }

    public GameObject ReturnGO() // Returns a randomized variation of a room type
    {
        int randNum = Random.Range(0, roomVariations.Count);
        
        return roomVariations[randNum];
    }
    #endregion
}
