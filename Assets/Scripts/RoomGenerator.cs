using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

    [Header("传送规则")]
    public GameObject teleporterPrefab;
    public string targetScene;
    public float difficulty;

    Transform generatorPoint;
    enum Direction { up, down, left, right };
    Direction direction;
    List<Room> roomList = new List<Room>();
    bool stageClear = false;
    bool teleporterCreated = false;

    /// <summary>
    /// 计时器
    /// </summary>
    public float elapsedTime { get; set; }

    private void Start()
    {
        elapsedTime = 0;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (roomList.Count != 0)
        {
            if (roomList.Last().stageClear)
            {
                stageClear = true;
            }
        }

        if (stageClear)
        {
            Clear();
        }
    }

    public void Generate()
    {
        SetupRoom();
        SetupDoor();
    }

    void SetupRoom() 
    { 
        GameObject roomPrefab;
        Room newRoom;
        generatorPoint = GetComponent<Transform>();

        for (int i = 0; i < generateNumber; i++){
            if (i == 0){
                newRoom = Instantiate(startRoom, generatorPoint.position, Quaternion.identity).GetComponent<Room>();
            }
            else if (i == generateNumber - 1){
                newRoom = Instantiate(endRoom, generatorPoint.position, Quaternion.identity).GetComponent<Room>();
                newRoom.stageClear = false;  // 初始设置为未通关状态
            }
            else{
                roomPrefab = roomPrefabs[(int)Random.Range(-0.5f,roomPrefabs.Length-0.5f)];
                newRoom = Instantiate(roomPrefab, generatorPoint.position, Quaternion.identity).GetComponent<Room>();
                newRoom.stageClear = false;  // 初始设置为未通关状态

                MonsterGenerator monsterGenerator = newRoom.gameObject.GetComponentInChildren<MonsterGenerator>();
                monsterGenerator.difficulty = Global.difficulty;
            }
   
            roomList.Add(newRoom);

            ChangeGeneratePosition();
        }
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
            foreach (Room room in roomList) {
                dist = Mathf.Min(dist, Vector3.Distance(room.transform.position, generatorPoint.position));
            }
        } while(dist < 0.5f);
    }

    void SetupDoor(){
        float distUp, distDown, distLeft, distRight;
        Vector3 roomPosition, otherRoomPosition;
        TeleportController teleporter;
        foreach (Room room in roomList){
            roomPosition = room.transform.position;
            room.flagUp = false;
            room.flagDown = false;
            room.flagLeft = false;
            room.flagRight = false;
            foreach (Room otherRoom in roomList) {
                otherRoomPosition = otherRoom.transform.position;
                distUp = Vector3.Distance(otherRoomPosition, roomPosition + new Vector3(0, yOffset, 0));
                distDown = Vector3.Distance(otherRoomPosition, roomPosition + new Vector3(0, -yOffset, 0));
                distLeft = Vector3.Distance(otherRoomPosition, roomPosition + new Vector3(-xOffset, 0, 0));
                distRight = Vector3.Distance(otherRoomPosition, roomPosition + new Vector3(xOffset, 0, 0));
                if (distUp < 0.5f) {
                    room.flagUp = true;
                    teleporter = room.doorUp.GetComponent<TeleportController>();
                    teleporter.teleportDestination = otherRoom.transform;
                }
                if (distDown < 0.5f) {
                    room.flagDown = true;
                    teleporter = room.doorDown.GetComponent<TeleportController>();
                    teleporter.teleportDestination = otherRoom.transform;
                }
                if (distLeft < 0.5f) {
                    room.flagLeft = true;
                    teleporter = room.doorLeft.GetComponent<TeleportController>();
                    teleporter.teleportDestination = otherRoom.transform;
                }
                if (distRight < 0.5f) {
                    room.flagRight = true;
                    teleporter = room.doorRight.GetComponent<TeleportController>();
                    teleporter.teleportDestination = otherRoom.transform;
                }
            }
            room.doorUp.SetActive(false);
            room.doorDown.SetActive(false);
            room.doorLeft.SetActive(false);
            room.doorRight.SetActive(false);
        }
    }

    void Clear()
    {
        if (!teleporterCreated)
        {
            GameObject teleportRoom = Instantiate(teleporterPrefab, roomList.Last().transform.position, Quaternion.identity);
            SceneTeleporter teleporter = teleportRoom.GetComponent<SceneTeleporter>();
            teleporter.targetScene = targetScene;
            teleporterCreated = true;

            // 生成奖励 & 提高难度
            Global.difficulty += Global.difficultyStep;
        }
        
    }
}
