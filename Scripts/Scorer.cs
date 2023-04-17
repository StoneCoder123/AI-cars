using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Scorer : MonoBehaviour
{
    public GameObject[] agents;
    public float[] Distances;
    [SerializeField] GameObject[] TopAgents;
    [SerializeField] int TopGs = 0;
    public bool Nextgen = false;
    public TextMeshProUGUI gens;
    public int genCount;
    public float CurrentDist;
    public float PrevDist;
    public float MutationCoeff = 10f;
    public Level[][] prevToppers;
    public float[] prevMaxDistances;
    public float[] Modifiers;
    public float D = 0;
    public int mutationRate = 5;
    [SerializeField] float PercentageMutation = 1f;
    [SerializeField] float PercentageOfMaxDistance = 10;
    public struct State
    {
        float[][] weights;
        float[] biases;
    };
    // Start is called before the first frame update
    void Start()
    {
        
        agents = GameObject.FindGameObjectsWithTag("Player");
        genCount = 1;
        PrevDist = 0f;
        CurrentDist = 0f;
        prevToppers = new Level[200][];
        Modifiers = new float[200];
        prevMaxDistances = new float[200];
        for(int i = 0; i<200; i++)
        {
            prevToppers[i] = new Level[4];
            Modifiers[i] = 1f;
            prevMaxDistances[i] = 0f;
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        gens.text = "Generation: " + genCount;
        
            for(int j = 0; j<agents.Length - 1; j++)
            {
                for(int i = 0; i<agents.Length-1; i++)
                {
                    if (agents[i].GetComponent<Car_Controller>().Distance > agents[i+1].GetComponent<Car_Controller>().Distance)
                    {
                        GameObject temp = agents[i+1];
                        agents[i + 1] = agents[i];
                        agents[i] = temp;
                    }
                }
            }
        

        
        print("Top scorer is: " + agents[agents.Length-1]);

        
        if (Input.GetKeyDown(KeyCode.P))
        {
            GeneticAlgorithm2();
            
        }



    }

  

   /* public void GeneticAlgorithm()
    {



        Car_Controller GenericController = agents[0].GetComponent<Car_Controller>();
        PrevDist = CurrentDist;
        for(int i = 1; i<=5; i++)
        {
            TopScorers[i - 1] = agents[Distances.Length - i];
        }
        Car_Controller cont = TopScorers[0].GetComponent<Car_Controller>();
        float[][][] CurrentWeights = new float[TopScorers.Length][][];
        float[][][] PreviousWeights = new float[TopScorers.Length][][];
        
        State[] currentState = new State[TopScorers.Length] ;
        CurrentDist = cont.Distance;

        //Tracking previous top weights;
        Level[] TopState = { agents[99].GetComponent<Car_Controller>().InputLayer, agents[99].GetComponent<Car_Controller>().HiddenLayer1, agents[99].GetComponent<Car_Controller>().HiddenLayer2, agents[99].GetComponent<Car_Controller>().HiddenLayer3 };
        prevToppers[genCount - 1] = TopState;
        prevMaxDistances[genCount - 1] = agents[99].GetComponent<Car_Controller>().Distance;
        if (agents[99].GetComponent<Car_Controller>().Distance <= 120)
        {
            Modifiers[genCount - 1] = 0.3f;
        }
        D += agents[99].GetComponent<Car_Controller>().Distance;




        float TotalDistance = 0;
        foreach(GameObject car in agents)
        {
            TotalDistance += car.GetComponent<Car_Controller>().Distance;
        }

        
        {
            for (int i = 0; i < agents.Length; i++)
            {
                //float Mutation = Random.Range(-1f, 1f);
                float Mutation = -MutationCoeff/2 + MutationCoeff * i / (agents.Length-1);

                for (int j = 0; j < cont.InputLayer.inputs.Length; j++)
                {
                    for (int k = 0; k < cont.InputLayer.outputs.Length; k++)
                    {
                        //float sum = 0f;
                        // foreach (GameObject car in TopScorers)
                        //  {
                        //      Car_Controller carcont = car.GetComponent<Car_Controller>();
                        //       sum += carcont.InputLayer.weights[j][k] * Random.Range(0f, 1f);
                        //  }

                        // agents[i].GetComponent<Car_Controller>().InputLayer.weights[j][k] = sum + Mutation;
                        float sum = 0;
                        for (int l = 0; l < genCount; l++)
                        {
                            sum += prevToppers[l][0].weights[j][k] * Random.Range(0f, 1.01f) * prevMaxDistances[l] / D;

                        }
                        agents[i].GetComponent<Car_Controller>().InputLayer.weights[j][k] = sum/genCount;
                    }
                }

                for (int j = 0; j < cont.InputLayer.outputs.Length; j++)
                {
                    // float sum = 0f;
                    // foreach (GameObject car in TopScorers)
                    // {
                    //      Car_Controller carcont = car.GetComponent<Car_Controller>();
                    //      sum += carcont.InputLayer.biases[j] * Random.Range(0f, 1f);
                    //  }

                    //  agents[i].GetComponent<Car_Controller>().InputLayer.biases[j] = sum + Mutation;
                    float sum = 0;
                    for (int l = 0; l < genCount; l++)
                    {
                        sum += prevToppers[l][0].biases[j] * Random.Range(0f, 1.01f) * prevMaxDistances[l] / D;

                    }
                    agents[i].GetComponent<Car_Controller>().InputLayer.biases[j] = sum/genCount;
                }


            }

        }
        
        //Hidden Layer: 

        

        
        {
            for (int i = 0; i < agents.Length; i++)
            {
                //float Mutation = Random.Range(-1f, 1f);
                float Mutation = -MutationCoeff / 2 + MutationCoeff * i / (agents.Length-1);

                for (int j = 0; j < cont.HiddenLayer1.inputs.Length; j++)
                {
                    for (int k = 0; k < cont.HiddenLayer1.outputs.Length; k++)
                    {
                        // float sum = 0f;
                        // foreach (GameObject car in TopScorers)
                        //  {
                        //     Car_Controller carcont = car.GetComponent<Car_Controller>();
                        //      sum += carcont.InputLayer.weights[j][k] * Random.Range(0f, 1f);
                        //  }

                        //agents[i].GetComponent<Car_Controller>().HiddenLayer.weights[j][k] = sum + Mutation;
                        float sum = 0;
                        for (int l = 0; l < genCount; l++)
                        {
                            sum += prevToppers[l][1].weights[j][k] * Random.Range(0f, 1.01f) * prevMaxDistances[l]/D;

                        }
                        agents[i].GetComponent<Car_Controller>().HiddenLayer1.weights[j][k] = sum/genCount;
                    }
                }

                for (int j = 0; j < cont.HiddenLayer1.outputs.Length; j++)
                {
                    // float sum = 0f;
                    // foreach (GameObject car in TopScorers)
                    // {
                    //   Car_Controller carcont = car.GetComponent<Car_Controller>();
                    // sum += carcont.InputLayer.biases[j] * Random.Range(0f, 1f);
                    //p }

                    float sum = 0;
                    for (int l = 0; l < genCount; l++)
                    {
                        sum += prevToppers[l][1].biases[j] * Random.Range(0f, 1.01f) * prevMaxDistances[l] / D;

                    }
                    agents[i].GetComponent<Car_Controller>().HiddenLayer1.biases[j] = sum / genCount;

                }

            }

            for (int i = 0; i < agents.Length; i++)
            {
                //float Mutation = Random.Range(-1f, 1f);
                float Mutation = -MutationCoeff / 2 + MutationCoeff * i / (agents.Length-1);

                for (int j = 0; j < cont.HiddenLayer2.inputs.Length; j++)
                {
                    for (int k = 0; k < cont.HiddenLayer2.outputs.Length; k++)
                    {
                        // float sum = 0f;
                        // foreach (GameObject car in TopScorers)
                        //  {
                        //     Car_Controller carcont = car.GetComponent<Car_Controller>();
                        //      sum += carcont.InputLayer.weights[j][k] * Random.Range(0f, 1f);
                        //  }

                        //agents[i].GetComponent<Car_Controller>().HiddenLayer.weights[j][k] = sum + Mutation;
                        float sum = 0;
                        for (int l = 0; l < genCount; l++)
                        {
                            sum += prevToppers[l][2].weights[j][k] * Random.Range(0f, 1.01f) * prevMaxDistances[l] / D;

                        }
                        agents[i].GetComponent<Car_Controller>().HiddenLayer2.weights[j][k] = sum / genCount;
                    }
                }

                for (int j = 0; j < cont.HiddenLayer2.outputs.Length; j++)
                {
                    // float sum = 0f;
                    // foreach (GameObject car in TopScorers)
                    // {
                    //   Car_Controller carcont = car.GetComponent<Car_Controller>();
                    // sum += carcont.InputLayer.biases[j] * Random.Range(0f, 1f);
                    //p }

                    float sum = 0;
                    for (int l = 0; l < genCount; l++)
                    {
                        sum += prevToppers[l][2].biases[j] * Random.Range(0f, 1.01f) * prevMaxDistances[l] / D;

                    }
                    agents[i].GetComponent<Car_Controller>().HiddenLayer2.biases[j] = sum / genCount;

                }

            }

            for (int i = 0; i < agents.Length; i++)
            {
                //float Mutation = Random.Range(-1f, 1f);
                float Mutation = -MutationCoeff / 2 + MutationCoeff * i / (agents.Length-1);

                for (int j = 0; j < cont.HiddenLayer3.inputs.Length; j++)
                {
                    for (int k = 0; k < cont.HiddenLayer3.outputs.Length; k++)
                    {
                        // float sum = 0f;
                        // foreach (GameObject car in TopScorers)
                        //  {
                        //     Car_Controller carcont = car.GetComponent<Car_Controller>();
                        //      sum += carcont.InputLayer.weights[j][k] * Random.Range(0f, 1f);
                        //  }

                        //agents[i].GetComponent<Car_Controller>().HiddenLayer.weights[j][k] = sum + Mutation;
                        float sum = 0;
                        for (int l = 0; l < genCount; l++)
                        {
                            sum += prevToppers[l][3].weights[j][k] * Random.Range(0f, 1.01f) * prevMaxDistances[l] / D;

                        }
                        agents[i].GetComponent<Car_Controller>().HiddenLayer3.weights[j][k] = sum / genCount;
                    }
                }

                for (int j = 0; j < cont.HiddenLayer3.outputs.Length; j++)
                {
                    // float sum = 0f;
                    // foreach (GameObject car in TopScorers)
                    // {
                    //   Car_Controller carcont = car.GetComponent<Car_Controller>();
                    // sum += carcont.InputLayer.biases[j] * Random.Range(0f, 1f);
                    //p }

                    float sum = 0;
                    for (int l = 0; l < genCount; l++)
                    {
                        sum += prevToppers[l][3].biases[j] * Random.Range(0f, 1.01f) * prevMaxDistances[l] / D;

                    }
                    agents[i].GetComponent<Car_Controller>().HiddenLayer3.biases[j] = sum / genCount;
                }

                
            }

            for (int i = 0; i < agents.Length; i++)
            {
                //float Mutation = Random.Range(-1f, 1f);
                float Mutation = -MutationCoeff / 2 + MutationCoeff * i / (agents.Length - 1);

                for (int j = 0; j < cont.HiddenLayer4.inputs.Length; j++)
                {
                    for (int k = 0; k < cont.HiddenLayer4.outputs.Length; k++)
                    {
                        // float sum = 0f;
                        // foreach (GameObject car in TopScorers)
                        //  {
                        //     Car_Controller carcont = car.GetComponent<Car_Controller>();
                        //      sum += carcont.InputLayer.weights[j][k] * Random.Range(0f, 1f);
                        //  }

                        //agents[i].GetComponent<Car_Controller>().HiddenLayer.weights[j][k] = sum + Mutation;
                        float sum = 0;
                        for (int l = 0; l < genCount; l++)
                        {
                            sum += prevToppers[l][3].weights[j][k] * Random.Range(0f, 1.01f) * prevMaxDistances[l] / D;

                        }
                        agents[i].GetComponent<Car_Controller>().HiddenLayer4.weights[j][k] = sum / genCount;
                    }
                }

                for (int j = 0; j < cont.HiddenLayer4.outputs.Length; j++)
                {
                    // float sum = 0f;
                    // foreach (GameObject car in TopScorers)
                    // {
                    //   Car_Controller carcont = car.GetComponent<Car_Controller>();
                    // sum += carcont.InputLayer.biases[j] * Random.Range(0f, 1f);
                    //p }

                    float sum = 0;
                    for (int l = 0; l < genCount; l++)
                    {
                        sum += prevToppers[l][3].biases[j] * Random.Range(0f, 1.01f) * prevMaxDistances[l] / D;

                    }
                    agents[i].GetComponent<Car_Controller>().HiddenLayer4.biases[j] = sum / genCount;
                }


            }

            for (int i = 0; i < agents.Length; i++)
            {
                //float Mutation = Random.Range(-1f, 1f);
                float Mutation = -MutationCoeff / 2 + MutationCoeff * i / (agents.Length - 1);

                for (int j = 0; j < cont.HiddenLayer5.inputs.Length; j++)
                {
                    for (int k = 0; k < cont.HiddenLayer5.outputs.Length; k++)
                    {
                        // float sum = 0f;
                        // foreach (GameObject car in TopScorers)
                        //  {
                        //     Car_Controller carcont = car.GetComponent<Car_Controller>();
                        //      sum += carcont.InputLayer.weights[j][k] * Random.Range(0f, 1f);
                        //  }

                        //agents[i].GetComponent<Car_Controller>().HiddenLayer.weights[j][k] = sum + Mutation;
                        float sum = 0;
                        for (int l = 0; l < genCount; l++)
                        {
                            sum += prevToppers[l][3].weights[j][k] * Random.Range(0f, 1.01f) * prevMaxDistances[l] / D;

                        }
                        agents[i].GetComponent<Car_Controller>().HiddenLayer5.weights[j][k] = sum / genCount;
                    }
                }

                for (int j = 0; j < cont.HiddenLayer5.outputs.Length; j++)
                {
                    // float sum = 0f;
                    // foreach (GameObject car in TopScorers)
                    // {
                    //   Car_Controller carcont = car.GetComponent<Car_Controller>();
                    // sum += carcont.InputLayer.biases[j] * Random.Range(0f, 1f);
                    //p }

                    float sum = 0;
                    for (int l = 0; l < genCount; l++)
                    {
                        sum += prevToppers[l][3].biases[j] * Random.Range(0f, 1.01f) * prevMaxDistances[l] / D;

                    }
                    agents[i].GetComponent<Car_Controller>().HiddenLayer5.biases[j] = sum / genCount;
                }


            }

            for (int i = 0; i < agents.Length; i++)
            {
                //float Mutation = Random.Range(-1f, 1f);
                float Mutation = -MutationCoeff / 2 + MutationCoeff * i / (agents.Length - 1);

                for (int j = 0; j < cont.HiddenLayer6.inputs.Length; j++)
                {
                    for (int k = 0; k < cont.HiddenLayer6.outputs.Length; k++)
                    {
                        // float sum = 0f;
                        // foreach (GameObject car in TopScorers)
                        //  {
                        //     Car_Controller carcont = car.GetComponent<Car_Controller>();
                        //      sum += carcont.InputLayer.weights[j][k] * Random.Range(0f, 1f);
                        //  }

                        //agents[i].GetComponent<Car_Controller>().HiddenLayer.weights[j][k] = sum + Mutation;
                        float sum = 0;
                        for (int l = 0; l < genCount; l++)
                        {
                            sum += prevToppers[l][3].weights[j][k] * Random.Range(0f, 1.01f) * prevMaxDistances[l] / D;

                        }
                        agents[i].GetComponent<Car_Controller>().HiddenLayer6.weights[j][k] = sum / genCount;
                    }
                }

                for (int j = 0; j < cont.HiddenLayer6.outputs.Length; j++)
                {
                    // float sum = 0f;
                    // foreach (GameObject car in TopScorers)
                    // {
                    //   Car_Controller carcont = car.GetComponent<Car_Controller>();
                    // sum += carcont.InputLayer.biases[j] * Random.Range(0f, 1f);
                    //p }

                    float sum = 0;
                    for (int l = 0; l < genCount; l++)
                    {
                        sum += prevToppers[l][3].biases[j] * Random.Range(0f, 1.01f) * prevMaxDistances[l] / D;

                    }
                    agents[i].GetComponent<Car_Controller>().HiddenLayer6.biases[j] = sum / genCount;
                }


            }





        }




        genCount++;
        

    } */

    private void GeneticAlgorithm2()
    {
        float Distance = agents[agents.Length-1].GetComponent<Car_Controller>().Distance;
        TopGs = 0;

        //if (genCount == 1)
        //{
          //  TopGs = 5;
        //}
        //else
        {
            foreach (GameObject agent in agents)
            {
                if (agent.GetComponent<Car_Controller>().Distance > (1 - (PercentageOfMaxDistance / 100)) * Distance)
                {
                    TopGs++;
                }
            }
        }
        Debug.Log((1 - (PercentageOfMaxDistance/100)) * Distance);
        TopAgents = new GameObject[TopGs];
        for(int i = 0; i<TopAgents.Length; i++)
        {
            TopAgents[i] = agents[agents.Length - 1 - i];
        }

        


        Level[][] newLayers = new Level[TopAgents.Length][];
        for(int i = 0; i<TopAgents.Length; i++)
        {
            newLayers[i] = TopAgents[i].GetComponent<Car_Controller>().NN;
        }
        

        for (int i = 0; i<agents.Length; i++)
        {
            for(int j = 0; j<agents[i].GetComponent<Car_Controller>().NN.Length; j++)
            {
                for(int k = 0; k< agents[i].GetComponent<Car_Controller>().NN[j].inputs.Length; k++)
                {
                    for(int l = 0; l < agents[i].GetComponent<Car_Controller>().NN[j].outputs.Length; l++)
                    {
                        int x = Random.Range(0, TopAgents.Length);
                        agents[i].GetComponent<Car_Controller>().NN[j].weights[k][l] = newLayers[x][j].weights[k][l];
                    }
                }
            }

            for (int j = 0; j < agents[i].GetComponent<Car_Controller>().NN.Length; j++)
            {
                for (int k = 0; k < agents[i].GetComponent<Car_Controller>().NN[j].outputs.Length; k++)
                { 
                        int x = Random.Range(0, TopAgents.Length);
                        agents[i].GetComponent<Car_Controller>().NN[j].biases[k] = newLayers[x][j].biases[k];

                }
            }


            for (int j = 0; j < agents[0].GetComponent<Car_Controller>().NN.Length; j++)
            {
                agents[i].GetComponent<Car_Controller>().NN[j] = Mutate(agents[i].GetComponent<Car_Controller>().NN[j], mutationRate);
            }
                


        }

        genCount++;

       // for(int i = 0; i<agents.Length; i++)
      //  {
       //     agents[i].GetComponent<Car_Controller>().NN = newLayers[i];
       // }
    }

    Level Mutate(Level input, int mutationRate)
    {



        for (int i = 0; i < ((mutationRate > (input.inputs.Length * input.outputs.Length)) ? (input.inputs.Length * input.outputs.Length) : mutationRate); i++) 
        {
            int i1 = Random.Range(0, input.inputs.Length);
            int i2 = Random.Range(0, input.inputs.Length);
            int j1 = Random.Range(0, input.outputs.Length);
            int j2 = Random.Range(0, input.outputs.Length);
            float temp = input.weights[i1][j1];
            input.weights[i1][j1] = input.weights[i2][j2] * (1 + Random.Range(-PercentageMutation / 100, PercentageMutation / 100));
            input.weights[i2][j2] = temp * (1 + Random.Range(-PercentageMutation / 100, PercentageMutation / 100));
        }

        return input;
    }
}
