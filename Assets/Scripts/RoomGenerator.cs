using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [Header("房间属性")]
    public GameObject roomPrefab;
    
    public int generateNumber;
    public float xOffset;
    public float yOffset;

    Transform generatorPoint;
    enum Direction {up, down, left, right};
    Direction direction;
    List<GameObject> roomList = new List<GameObject>();
    List<Vector3> roomPositionList = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        generatorPoint = GetComponent<Transform>();

        for (int i = 0; i < generateNumber; i++){
            roomList.Add(Instantiate(roomPrefab, generatorPoint.position, Quaternion.identity));
            roomPositionList.Add(generatorPoint.position);

            ChangeGeneratePosition();
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
