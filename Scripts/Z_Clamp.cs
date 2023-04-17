using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Z_Clamp : MonoBehaviour
{
    public Transform target;
    public GameObject scorer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        target = scorer.GetComponent<Scorer>().agents[scorer.GetComponent<Scorer>().agents.Length-1].GetComponent<Transform>();
        Vector3 desired_pos = new Vector3(target.position.x, transform.position.y, target.position.z-4.75f);
        transform.position = Vector3.Lerp(transform.position, desired_pos, 10f * Time.deltaTime);
    }
}
