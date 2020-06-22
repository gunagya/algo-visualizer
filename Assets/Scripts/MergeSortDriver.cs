using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeSortDriver : MonoBehaviour
{
    public int size = 0;
    public GameObject toSpawn;

    public AudioSource liftSound;
    public AudioSource dropSound;

    Vector3[] mapLocations;
    GameObject[] array;
    float[] values;
    int stackCount = -1;

    AudioSource swapSound;

    void Start()
    {
        swapSound = GetComponent<AudioSource>();

        if (size > 0)
        {
            mapLocations = new Vector3[size];
            array = new GameObject[size];
            values = new float[size];
            for (int i = 0; i < size; i++)
            {
                mapLocations[i] = new Vector3(-10 + ((2 * i + 1f) / (2 * size)) * 20, 1f, 0);
                array[i] = Instantiate(toSpawn, mapLocations[i], transform.rotation) as GameObject;
                array[i].transform.localScale = Random.Range(0.1f, 1f) * new Vector3(1, 1, 1);
                array[i].transform.parent = gameObject.transform;
            }
        }
        stackCount = 0;
        StartCoroutine(merge_sort(0, size - 1));
    }

    void Update()
    {
        if (stackCount == 0)
        {
            Debug.Log("Done");
            stackCount = -1;
        }
    }

    IEnumerator merge_sort(int l, int r)
    {
        stackCount++;
        if (l < r)
        {
            int mid=(l+r)/2, i, j, k;
            yield return StartCoroutine(merge_sort(l, mid));
            yield return StartCoroutine(merge_sort(mid + 1, r));   //Remove yield return to obtain a parallel algorithm
            GameObject[] temp;
            temp = new GameObject[r - l + 1];           //Does this point to the same gameObject?
            for (i=l; i<=r; i++)
                values[i] = array[i].transform.localScale.x;
            i = l; j = mid + 1; k = 0;
            while (i<=mid && j<=r)
            {
                if (values[i]<=values[j])
                {
                    temp[k] = array[i];
                    lift_to(i, l + k, mapLocations[l + k].x - mapLocations[i].x + 2);
                    yield return new WaitForSeconds(1f);
                    i++;
                }
                else
                {
                    temp[k] = array[j];
                    lift_to(j, l + k, mapLocations[j].x - mapLocations[l + k].x + 2);
                    yield return new WaitForSeconds(1);
                    j++;
                }
                k++;
            }
            while (i<=mid)
            {
                temp[k] = array[i];
                lift_to(i, l + k, mapLocations[l + k].x - mapLocations[i].x + 2);
                yield return new WaitForSeconds(1);
                i++;
                k++;
            }
            while (j<=r)
            {
                temp[k] = array[j];
                lift_to(j, l + k, mapLocations[j].x - mapLocations[l + k].x + 2);
                yield return new WaitForSeconds(1);
                j++;
                k++;
            }
            for (i = l; i <= r; i++)
            {
                array[i] = temp[i - l];
                set_down(i);
            }
            yield return new WaitForSeconds(1);
        }
        stackCount--;
    }

    void lift_to(int i, int j, float speed)
    {
        Vector3 lifted = mapLocations[j] + new Vector3(0, 2, 0);
        ElementBehaviour aScript = array[i].GetComponent<ElementBehaviour>();
        if (aScript == null)
            Debug.Log("No script found");
        else
        {
            if (liftSound)
                liftSound.Play();
            aScript.wayPointList = new Vector3[1];
            aScript.wayPointList[0] = lifted;
            aScript.speed = speed;
            aScript.currentWayPoint = 0;
        }
    }

    void set_down(int i)
    {
        Vector3 dropped = mapLocations[i];
        ElementBehaviour aScript = array[i].GetComponent<ElementBehaviour>();
        if (aScript == null)
            Debug.Log("No script found");
        else
        {
            if (dropSound)
                dropSound.Play();
            aScript.wayPointList = new Vector3[1];
            aScript.wayPointList[0] = dropped;
            aScript.speed = 4f;
            aScript.currentWayPoint = 0;
        }
    }
}
