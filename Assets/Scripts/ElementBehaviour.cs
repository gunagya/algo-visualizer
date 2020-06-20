using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementBehaviour : MonoBehaviour
{
    public float speed = 2f;
    public Vector3[] wayPointList;
    public int currentWayPoint = 0;

    // Update is called once per frame
    void Update()
    {
        if (currentWayPoint<wayPointList.Length)
        {
            move_smoothly();
        }
    }

    void move_smoothly()
    {
        Vector3 target = wayPointList[currentWayPoint];
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (transform.position == target)
            currentWayPoint++;
    }
}
