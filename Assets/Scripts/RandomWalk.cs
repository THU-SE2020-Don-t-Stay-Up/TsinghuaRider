using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWalk : MonoBehaviour
{
    static Vector3 leftTop = new Vector3 (-5, 5, 0);
    static Vector3 rightTop = new Vector3 (5, 5, 0);
    static Vector3 leftBottom = new Vector3 (-5, -5, 0);
    static Vector3 rightBottom = new Vector3 (5, -5, 0);
    Vector3[] positionList = { leftTop, rightTop, rightBottom, leftBottom};
    int positionIndex = 0;

    public float MoveSpeed = 5;
    int delta = 0;
    // Update is called once per frame
    void Update()
    {
        if (delta < 60)
        {
            MoveRandom(delta);
            delta += 1;
        }
        else
        {
            delta = 0;
        }
    }

    void FixedUpdate()
    {
        MoveRandom(delta);
    }

    void MoveRandom(int t)
    {

        Vector3 nextPosition = positionList[positionIndex];
        Vector3 nowPosition = transform.position;
        Vector3 distance = nextPosition - nowPosition;
        Vector3 direction = Vector3.Normalize(distance);

        if (Vector3.Magnitude(distance) < 0.1)
        {
            positionIndex = (positionIndex + 1) % 4;
        }
        else
        {
            transform.Translate(MoveSpeed * direction * Time.deltaTime);
        }
    }
}
