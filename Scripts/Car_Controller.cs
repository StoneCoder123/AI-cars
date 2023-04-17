using UnityEngine;




public class Car_Controller : MonoBehaviour
{   
    
    public float maxAcceleration = 5f;
    public float maxVel = 10f;
    public float maxfriction = 2f;
    public float rotSpeed = 5f;
    public float Range = 20f;
    public GameObject[] Wheels;
    private Vector3 velocity = new Vector3(0,0,0);
    private Rigidbody rb;
    private float friction = 0f;
    int NumRays = 50;
    public float[] Distances = new float[100];
    public float[] Outputs = new float[4];
    // public float[] OutputValues;
    public Level[] NN;
    private bool collided = false;
    public Scorer scorer;
    public Transform start;
    public float Distance;
    public float DistanceScale = 100f;
    public BoxCollider CollisionTrigger;
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
        int LayerCount = 5;
        CollisionTrigger = gameObject.GetComponent<BoxCollider>();
        target = GameObject.FindGameObjectWithTag("Target").GetComponent<Transform>();
        CollisionTrigger.enabled = true;
        NN = new Level[LayerCount];
        rb = gameObject.GetComponent<Rigidbody>();
        for(int i = 0; i<LayerCount; i++)
        {
            int input = 100 - ((96) * i / LayerCount);
            int output = 100 - ((96) * (i + 1) / LayerCount);
            NN[i] = new Level(input, output);
        }
        
        collided = false;
        scorer = GameObject.FindGameObjectWithTag("Scorer").GetComponent<Scorer>();
        start = GameObject.FindGameObjectWithTag("Start").GetComponent<Transform>();
       // Invoke("EnableColliders", 5f);
        
    }

    // Update is called once per frame
    void Update()
    {
        print(Time.time);
        if (Time.time > 10f)
        {
            if (!collided) { Move(); }
            
            Distance = (transform.position - start.position).magnitude;
            //if (transform.position.x > 37f || transform.position.x < -37f)
           // {
           //     collided = true;
           // }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ResetCars();

            }
        }
        
        
    }

    private void FixedUpdate()
    {
        if (!collided)
        { Sensors(); }
        
    }

    void Move()
    {
        Vector3 displacement = new Vector3(0f, 0f, 0f);
        // float forward = Input.GetAxis("Vertical");
        float forward = (-Outputs[0] + Outputs[1]) / 2;
        // float Horizontal = Input.GetAxis("Horizontal");
        float Horizontal = (Outputs[2] - Outputs[3]) / 2;
        int Fricdir = ((forward > 0) ? -1: 1);
        
        
        if(velocity.magnitude != 0)
        {
            friction = maxfriction;
        }
        else if(maxfriction > (Mathf.Abs(forward * maxAcceleration)))
        {
            friction = Mathf.Abs(forward * maxAcceleration);
        }
        else
        {
            friction = maxfriction;
        }
       

        if (velocity.magnitude > maxVel)
        {
            velocity = velocity.normalized * maxVel;
        }
        else
        {
            if (velocity.magnitude == 0)
            { velocity += (forward * maxAcceleration + Fricdir * friction) * gameObject.transform.forward * Time.deltaTime; }
            else
            {
                velocity += (forward * maxAcceleration * gameObject.transform.forward - velocity.normalized * friction) * Time.deltaTime;
            }
        }

        float dir = Vector3.Dot(transform.forward, velocity.normalized);

        velocity = velocity.magnitude * transform.forward * dir;
        float angle = 0;
        if (Horizontal != 0)
        {
            
            float ratio;
           // if (forward == 0)
         //   {
           //     ratio = 0f;
          //  }
          //  else
          //  {
                ratio = Horizontal / forward;
         //   }
            angle = Mathf.Atan(ratio);

            
            transform.RotateAround(transform.position, transform.up, angle * velocity.magnitude * rotSpeed* Time.deltaTime);
        }
        if (collided)
        {
            velocity = new Vector3(0f, 0f, 0f);
        }

       

        displacement += velocity * Time.deltaTime;

        transform.position += displacement;

        
    }

    void Sensors()
    {
        
        RaycastHit hit;
        Ray[] rays;

        Vector3 diff1 = (transform.right - transform.forward).normalized;
        Vector3 diff2 = -(transform.right + transform.forward).normalized;

        for(int i = 0; i<NumRays/2; i++)
        {
            Distances[i] = 0;
            Vector3 Raydir = (transform.forward + (i * diff1) / (NumRays/2)).normalized;
            if (Physics.Raycast(transform.position, Raydir, out hit, Range))
            {
                Distances[i] = hit.distance /DistanceScale;
                if (velocity.magnitude != 0)
                { Debug.DrawRay(transform.position, Raydir * hit.distance, Color.yellow); }
            }

         
        }

        for (int i = NumRays/2; i < NumRays; i++)
        {
            Distances[i] = 0;
            Vector3 Raydir = (transform.right + (i * diff2) / (NumRays / 2)).normalized;
            if (Physics.Raycast(transform.position, Raydir, out hit, Range))
            {
                Distances[i] = hit.distance/DistanceScale;
                if (velocity.magnitude != 0)
                { Debug.DrawRay(transform.position, Raydir * hit.distance, Color.yellow); }
            }


        }

        for (int i = NumRays; i < 3* NumRays / 2; i++)
        {
            Distances[i] = 0;
            Vector3 Raydir = (-transform.forward - (i * diff1) / (NumRays / 2)).normalized;
            if (Physics.Raycast(transform.position, Raydir, out hit, Range))
            {
                Distances[i] = hit.distance/DistanceScale;
                if (velocity.magnitude != 0)
                { Debug.DrawRay(transform.position, Raydir * hit.distance, Color.yellow); }
            }


        }

        for (int i = 3*NumRays/2; i < NumRays; i++)
        {
            Distances[i] = 0;
            Vector3 Raydir = (-transform.right - (i * diff2) / (NumRays / 2)).normalized;
            if (Physics.Raycast(transform.position, Raydir, out hit, Range))
            {
                Distances[i] = hit.distance/DistanceScale;
                if (velocity.magnitude != 0)
                { Debug.DrawRay(transform.position, Raydir * hit.distance, Color.yellow); }
            }


        }



        
        float[] vs = NN[0].Output(Distances);
        for(int i = 1; i<NN.Length; i++)
        {
            float[] vs1 = NN[i].Output(vs);
            vs = new float[NN[i].outputs.Length];
            vs = vs1;
        }
            
            Outputs = vs;
        
       

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "environment")
        {
            collided = true;
        }
    }

    private void ResetCars()
    {
        transform.position = start.position;
        velocity = new Vector3(0f, 0f, 0f);
        transform.rotation = start.rotation;
        collided = false;
        //CollisionTrigger.enabled = false;
        //Invoke("EnableColliders", 5f);


    }

    private void EnableColliders()
    {
        CollisionTrigger.enabled = true;
    }

    private void Wait()
    {

    }
}
