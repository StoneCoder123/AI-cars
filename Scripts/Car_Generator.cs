using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Generator : MonoBehaviour
{
    public Transform[] Generators;
    public GameObject[] cars;

    // Start is called before the first frame update
    void Start()
    {
        CreateTraffic();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateTraffic()
    {
        foreach(Transform gen in Generators)
        {
            int index = Random.Range(0, 4);
            GameObject car = Instantiate(cars[index], gen);
            car.GetComponent<Traffic>().velocity = Random.Range(15f, 25f);
        }

        Invoke("CreateTraffic", 3f);
        
    }
}
