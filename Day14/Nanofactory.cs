using System;
using System.Collections.Generic;

class Nanofactory
{
    Dictionary<string, List<Tuple<string, int>>> inputAsDict;
    Dictionary<string, int> excess;

    public Nanofactory(Dictionary<string, List<Tuple<string, int>>> puzzleInput)
    {
        inputAsDict = puzzleInput;
        excess =  new Dictionary<string, int>();
    }

    public int getOreCount()
    {
        var oreCount = 0;
        var fuelComponents = inputAsDict[Constants.FUEL];
        while(fuelComponents.Count>1)
        {
            fuelComponents = new List<Tuple<string, int>>(inputAsDict[Constants.FUEL]);
            oreCount = updateFuelDictionary(oreCount);
        }
        return oreCount;
    }

    int increaseOreCount(Tuple<string, int> inputTuple)
    {
        inputAsDict[Constants.FUEL].Remove(inputTuple);
        return inputTuple.Item2;
    }

    int updateFuelDictionary(int oreCount)
    {
        var fuelComponents = new List<Tuple<string, int>>(inputAsDict[Constants.FUEL]);
        for (int chemicalIndex=1; chemicalIndex<fuelComponents.Count; chemicalIndex++)
        {
            var chemical = fuelComponents[chemicalIndex];
            if (chemical.Item1==Constants.ORE)
            {
                oreCount += increaseOreCount(chemical);
            }
            else
            {
                calculateInputFuelReqd(chemical);
            }
        }
        return oreCount;
    }

    int performSubstitution(List<Tuple<string, int>> inputFuel, int outputFuelAmount)
    {
        var inputFuelAmount = inputFuel[0].Item2;
        var multiplier = 1;
        while(inputFuelAmount<outputFuelAmount)
        {
            multiplier += 1;
            inputFuelAmount += inputFuel[0].Item2;
        }
        for (int j=1; j<inputFuel.Count; j++)
        {
            var newAmount = inputFuel[j].Item2 * multiplier;
            inputAsDict[Constants.FUEL].Add(new Tuple<string, int>(inputFuel[j].Item1, newAmount));
        }
        return inputFuelAmount - outputFuelAmount;
    }

    void calculateInputFuelReqd(Tuple<string, int> chemical)
    {
        var outputFuelAmount = chemical.Item2;
        if (excess.ContainsKey(chemical.Item1))  // Check for excess
        {
            if (outputFuelAmount<=excess[chemical.Item1])
            {
                excess[chemical.Item1] -= outputFuelAmount;
                outputFuelAmount = 0;
                inputAsDict[Constants.FUEL].Remove(chemical);
            }
            else
            {
                outputFuelAmount -= excess[chemical.Item1];
                excess.Remove(chemical.Item1);
                callPerformSubstitution(chemical, outputFuelAmount);
            }
        }
        else
        {
            callPerformSubstitution(chemical, outputFuelAmount);
        }
    }

    void callPerformSubstitution(Tuple<string, int> chemical, int outputFuelAmount)
    {
        var inputFuel = inputAsDict[chemical.Item1];
        var excessAmount = performSubstitution(inputFuel, outputFuelAmount);
        if (excessAmount>0)
        {
            excess[chemical.Item1] = excessAmount;
        }
        inputAsDict[Constants.FUEL].Remove(chemical);
    }
}