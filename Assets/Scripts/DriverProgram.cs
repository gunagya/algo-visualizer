using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverProgram : MonoBehaviour
{
    public int size = 0;
    public GameObject toSpawn;
    public AudioSource swapSound;
    public AudioSource liftSound;
    public AudioSource dropSound;

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
        StartCoroutine(binary_sort(0, size - 1));
    }

    void Update()
    {
        if (stackCount == 0)
        {
            Debug.Log("Done");
            stackCount = -1;
        }
    }

    IEnumerator binary_sort(int l, int r)
    {
        stackCount++;
        if (l < r)
        {
            int pivot_index, i = l, j;
            lift(r, 2f);
            yield return new WaitForSeconds(1);
            for (j = l; j < r; j++)
            {
                lift(j, 1f);
                yield return new WaitForSeconds(0.5f);
                drop(j);
                yield return new WaitForSeconds(0.5f);
                if (values[j] < values[r])
                {
                    if (i != j)
                    {
                        swap(i, j);
                        yield return new WaitForSeconds(1.2f);
                    }
                    i++;
                }
            }
            drop(r);
            yield return new WaitForSeconds(1);
            if (i != r)
            {
                swap(i, r);
                yield return new WaitForSeconds(1.2f);
            }
            pivot_index = i;
            yield return StartCoroutine(binary_sort(l, pivot_index - 1));
            yield return StartCoroutine(binary_sort(pivot_index + 1, r));   //Remove yield return to obtain a parallel algorithm
        }
        stackCount--;
    }

    void lift(int index, float h)
    {
        Vector3 lifted = mapLocations[index] + new Vector3(0, h, 0);
        ElementBehaviour aScript = array[index].GetComponent<ElementBehaviour>();
        if (aScript == null)
            Debug.Log("No script found");
        else
        {
            if (liftSound && h==2f)
                liftSound.Play();
            aScript.wayPointList = new Vector3[1];
            aScript.wayPointList[0] = lifted;
            aScript.speed = 4f;
            aScript.currentWayPoint = 0;
        }
    }
    void drop (int index)
    {
        Vector3 dropped = mapLocations[index];
        ElementBehaviour aScript = array[index].GetComponent<ElementBehaviour>();
        if (aScript == null)
            Debug.Log("No script found");
        else
        {
            if (dropSound && array[index].transform.position.y==3f)
                dropSound.Play();
            aScript.wayPointList = new Vector3[1];
            aScript.wayPointList[0] = dropped;
            aScript.speed = 4f;
            aScript.currentWayPoint = 0;
        }
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
            iScript.speed = mapLocations[j].x-mapLocations[i].x+2;
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
            jScript.speed = mapLocations[j].x - mapLocations[i].x + 2;
            jScript.currentWayPoint = 0;
        }

        GameObject temp_obj = array[i];
        array[i] = array[j];
        array[j] = temp_obj;
    }
}
