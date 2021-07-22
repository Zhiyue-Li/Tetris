using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] shapes;
    // Start is called before the first frame update
    void Start()
    {
        SpawnBlocks();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnBlocks()
    {
        int randomIndex = Random.Range(0, shapes.Length);
        Instantiate(shapes[randomIndex], new Vector3(4, 18, 0), shapes[randomIndex].transform.rotation);
    }
}
