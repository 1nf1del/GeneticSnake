﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class NeatAi {

    public int _poolSize,
               _inputCount,
               _outputCount;

    public int generationCount = 0;
    int genomeCount = 0;

    List<Generation> memory;

    List<float> desired;

    List<float> input;

    public void Instantiate (List<float> Desired, List<float> Input, int poolSize) {
        memory = new List<Generation>();

        _poolSize = poolSize;
        _inputCount = Input.Count;
        _outputCount = Desired.Count;

        Generation tempGen = new Generation();
        tempGen.Instantiate(generationCount, _poolSize, _inputCount, _outputCount);
        tempGen.FillPool();
        memory.Add(tempGen);

        desired = Desired;
        input = Input;
    }
	
	public List<float> Tick()
    {
        memory[generationCount].SetInputs(input);
        memory[generationCount].Calculate();
		memory[generationCount].CalcFitness(desired);
        memory[generationCount].RankGenomes();
        //memory[generationCount].CalcProbablity();
        return memory[generationCount].GetOutputs(0); //returns fittest genome
    }

    public void Evolve()
    {
		List<Genome> tempPool = memory [generationCount].Selection();
		tempPool = memory[generationCount].FillNewGeneration(tempPool);
        memory.Add(new Generation());
        generationCount++;
        memory[generationCount].Instantiate(generationCount, _poolSize, _inputCount, _outputCount);
        memory[generationCount].SetGeneration(tempPool);
        memory[generationCount].Mutate();
    }

	public void Mutate()
	{
		memory [generationCount].pool [genomeCount].Mutate ();
	}

	public void PrintAll(Vector3 pos)
	{
		if (Application.isPlaying) {
            int offsetCounter = 0;
            for (int i = 0; i < memory.Count; i++)
            {
                for (int k = 0; k < _poolSize; k++)
                {
                    memory[i].pool[k].Print(pos, offsetCounter);
                }
                offsetCounter += memory[i].GetLargestGenome().NodeCollection.Count - _inputCount - _outputCount + 3;
            }
            
		}
	}

    public void PrintBest(Vector3 pos)
    {
        if (Application.isPlaying)
        {
            int offsetCounter = 0;
            for (int i = 0; i < memory.Count; i++)
            {
                Genome fittest = memory[i].GetProbabiliestGenome();
                fittest.Print(pos, offsetCounter, true);
                offsetCounter += fittest.NodeCollection.Count - _inputCount - _outputCount + 3;
            }

        }
    }

    public int GetGenerationIndex()
    {
        return generationCount;
    }

    public Genome GetBestFitness()
    {
        return memory[generationCount].GetFittestGenome();
    }

    public Genome GetBestProbability()
    {
        return memory[generationCount].GetProbabiliestGenome();
    }
}

//first iteration -> test fitness
//Check fitness ranking
//Check diversity ranking ~
//combine ranks ~
//Decide probablity
//Crossover
//Mutation
//Copy to new generation
