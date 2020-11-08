 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [Header("房间预制体")]
    public GameObject startRoom;
    public GameObject endRoom;
    public GameObject[] roomPrefabs;
    
    [Header("生成规则")]
    public int generateNumber;
    public float xOffset;
    public float yOffset;

    Transform generatorPoint;
    enum Direction { up, down, left, right };
    Direction direction;
    List<Room> roomList = new List<Room>();
    List<Vector3> roomPositionList = new List<Vector3>();

    
    void Start()
    {
        GameObject roomPrefab;
        Room newRoom;
        generatorPoint = GetComponent<Transform>();

        for (int i = 0; i < generateNumber; i++){
            if (i == 0){
                roomPrefab = startRoom;
            }
            else if (i == generateNumber - 1){
                roomPrefab = endRoom;
            }
            else{
                roomPrefab = roomPrefabs[(int)Random.Range(-0.5f,roomPrefabs.Length-0.5f)];
            }

            newRoom = Instantiate(roomPrefab, generatorPoint.position, Quaternion.identity).GetComponent<Room>();

            roomList.Add(newRoom);
            roomPositionList.Add(generatorPoint.position);

            ChangeGeneratePosition();
        }

        SetupDoor();

    }


    void ChangeGeneratePosition(){
        float dist;

        do {
            direction = (Direction)Random.Range(0, 4);
            switch(direction){
                case Direction.up:
                    generatorPoint.position += new Vector3(0, yOffset, 0);
                    break;
                case Direction.down:
                    generatorPoint.position += new Vector3(0, -yOffset, 0);
                    break;
                case Direction.left:
                    generatorPoint.position += new Vector3(-xOffset, 0, 0);
                    break;
                case Direction.right:
                    generatorPoint.position += new Vector3(xOffset, 0, 0);
                    break;
            }

            dist = 1.0f;
            foreach (Vector3 roomPosition in roomPositionList) {
                dist = Mathf.Min(dist, Vector3.Distance(roomPosition, generatorPoint.position));
            }
        } while(dist < 0.5f);
    }


    void SetupDoor(){
        float distUp, distDown, distLeft, distRight;
        Vector3 roomPosition;
        foreach (Room room in roomList){
            roomPosition = room.gameObject.transform.position;
            room.flagUp = false;
            room.flagDown = false;
            room.flagLeft = false;
            room.flagRight = false;
            foreach (Vector3 otherRoomPosition in roomPositionList) {
                distUp = Vector3.Distance(otherRoomPosition, roomPosition + new Vector3(0, yOffset, 0));
                distDown = Vector3.Distance(otherRoomPosition, roomPosition + new Vector3(0, -yOffset, 0));
                distLeft = Vector3.Distance(otherRoomPosition, roomPosition + new Vector3(-xOffset, 0, 0));
                distRight = Vector3.Distance(otherRoomPosition, roomPosition + new Vector3(xOffset, 0, 0));
                if (distUp < 0.5f) {
                    room.flagUp = true;
                }
                if (distDown < 0.5f) {
                    room.flagDown = true;                
                }
                if (distLeft < 0.5f) {
                    room.flagLeft = true;
                }
                if (distRight < 0.5f) {
                    room.flagRight = true;
                }
            }
        }
    }

}
