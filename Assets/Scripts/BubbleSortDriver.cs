using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSortDriver : MonoBehaviour
{
    public int size = 0;
    public float rate = 2f;
    public GameObject toSpawn;
    public AudioSource swapSound;

    Vector3[] mapLocations;
    GameObject[] array;
    float[] values;
    int stackCount = -1;
    
    void Start()
    {
        if (size>0)
        {
            mapLocations = new Vector3[size];
            array = new GameObject[size];
            values = new float[size];
            for (int i = 0; i < size; i++)
            {
                mapLocations[i] = new Vector3(-10 + ((2 * i + 1f) / (2 * size)) * 20, 1f, 0);
                array[i] = Instantiate(toSpawn, mapLocations[i], transform.rotation) as GameObject;
                array[i].transform.localScale = Random.Range(0.1f, 1f) * new Vector3(1, 1, 1);
                values[i] = array[i].transform.localScale.x;
                array[i].transform.parent = gameObject.transform;
            }
        }
        stackCount = 0;
        StartCoroutine(bubble_sort());
    }

    void Update()
    {
        if (stackCount == 0)
        {
            Debug.Log("Done");
            stackCount = -1;
        }
    }

    IEnumerator bubble_sort()
    {
        stackCount++;
        int i, j;
        for (i=0; i<size-1; i++)
            for (j=0; j<size-i-1; j++)
                if (values[j]>values[j+1])
                {
                    swap(j, j + 1);
                    yield return new WaitForSeconds(0.5f);
                }
        stackCount--;
    }

    void swap(int i, int j)
    {
        //I need to accomplish 3 things - switching of values
        //Translation of gameobjects
        //Switching of gameobjects

        float temp_val = values[i];
        values[i] = values[j];
        values[j] = temp_val;

        if (swapSound)
            swapSound.Play();

        ElementBehaviour iScript = array[i].GetComponent<ElementBehaviour>();
        if (iScript == null)
            Debug.Log("No script found");
        else
        {
            iScript.currentWayPoint = 3;
            iScript.wayPointList = new Vector3[3];
            iScript.wayPointList[0] = mapLocations[i]+new Vector3(0, -1f, 0);
            iScript.wayPointList[1] = mapLocations[j] + new Vector3(0, -1f, 0);
            iScript.wayPointList[2] = mapLocations[j];
            iScript.speed = rate*(mapLocations[j].x-mapLocations[i].x+2);
            iScript.currentWayPoint = 0;
        }

        ElementBehaviour jScript = array[j].GetComponent<ElementBehaviour>();
        if (jScript == null)
            Debug.Log("No script found");
        else
        {
            jScript.currentWayPoint = 3;
            jScript.wayPointList = new Vector3[3];
            jScript.wayPointList[0] = mapLocations[j] + new Vector3(0, 1f, 0);
            jScript.wayPointList[1] = mapLocations[i] + new Vector3(0, 1f, 0);
            jScript.wayPointList[2] = mapLocations[i];
            jScript.speed = rate*(mapLocations[j].x - mapLocations[i].x + 2);
            jScript.currentWayPoint = 0;
        }

        GameObject temp_obj = array[i];
        array[i] = array[j];
        array[j] = temp_obj;
    }
}
