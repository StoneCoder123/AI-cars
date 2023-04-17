using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Level
{
    public float[] inputs;
    public float[] outputs;
    public float[][] weights;
    public float[] biases;
    public Level(int InputCount, int OutputCount)
    {
        inputs = new float[InputCount];
        outputs = new float[OutputCount];
        biases = new float[OutputCount];
        weights = new float[InputCount][];

        for (int i = 0; i < InputCount; i++)
        {
            weights[i] = new float[OutputCount];
        }

        randomise();
    }

    private void randomise()
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            for (int j = 0; j < outputs.Length; j++)
            {
                weights[i][j] = Random.Range(-1f, 1f);

            }
        }

        for (int i = 0; i < outputs.Length; i++)
        {
            biases[i] = Random.Range(-2f,2f);
        }
    }

    public float[] Output(float[] Inputs)
    {
        float[] outputValues = new float[outputs.Length];
        for (int i = 0; i < outputs.Length; i++)
        {
            outputValues[i] = 0f;
        }
        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i] = Inputs[i];
        }

        for (int i = 0; i < outputs.Length; i++)
        {
            for (int j = 0; j < inputs.Length; j++)
            {
                outputValues[i] += inputs[j] * weights[j][i];
            }
        }

        for (int i = 0; i < outputs.Length; i++)
        {
            if (outputValues[i] >= biases[i])
            {
                outputs[i] = 1;
            }
            else
            {
                outputs[i] = 0;
            }

            outputValues[i] = outputValues[i] + biases[i];
        }

        return Activation_Function(outputValues);
    }

    private float[] Activation_Function(float[] inputs)
    {
        float[] output = new float[inputs.Length];

        for (int i = 0; i < inputs.Length; i++)
        {
            float x = inputs[i];
            output[i] = 2 / (1 + Mathf.Exp(-x)) - 1;

        }

        return output;
    }



}
