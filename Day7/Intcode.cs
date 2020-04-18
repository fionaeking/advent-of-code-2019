using System;
using System.Collections.Generic;
using System.Linq;

// Intcode computer

class Intcode
{
    private int instructionPointer;
    private List<int> puzzleInput;
    public Intcode(List<int> inputList)
    {
        instructionPointer = 0;
        puzzleInput = inputList;
    }

    public int Run(int phaseSetting, int previousOutputValue)
    {
        List<int> newInputsToReadIn = new List<int> { phaseSetting, previousOutputValue };
        List<int> outputDiagnosticCodes = new List<int>();
        var opcode = getOpcode(puzzleInput[0]);
        var instructionLength = checkInstruction(opcode);
        while (instructionLength != 0)
        {
            //Get parameter modes for each value in instruction
            var instructionValues = getInputValues(puzzleInput, instructionPointer, instructionLength);
            // Get instruction pointer for next loop
            incrementInstructionPointer(instructionLength);
            performInstruction(opcode, instructionValues, outputDiagnosticCodes, newInputsToReadIn);
            opcode = getOpcode(puzzleInput[0 + instructionPointer]);
            instructionLength = checkInstruction(opcode);
        }
        //Console.WriteLine("The diagnostic code is {0}", outputDiagnosticCodes.LastOrDefault());
        return outputDiagnosticCodes.LastOrDefault();
    }

    List<Tuple<int, int>> getInputValues(List<int> inputNumList, int offset, int length)
    {
        var inputValues = new List<Tuple<int, int>>();

        //Check instruction - remove opcode (last 2 digits)
        int instruction = inputNumList[offset];
        int currDigits = instruction / 100;
        for (int i = 1; i < length; i++)
        {
            int mode = currDigits % 10;
            inputValues.Add(new Tuple<int, int>(inputNumList[i + offset], mode));

            currDigits /= 10;
        }
        return inputValues;
    }

    int getOpcode(int instruction)
    {
        // Only select last 2 digits for opcode
        var opcode = instruction % 100;
        return opcode;
    }

    int checkInstruction(int opcode)
    {
        // Return length of instruction
        switch (opcode)
        {
            case 1: return 4;
            case 2: return 4;
            case 3: return 2;
            case 4: return 2;
            case 5: return 3;
            case 6: return 3;
            case 7: return 4;
            case 8: return 4;
            case 99: return 0;
            default: throw new Exception("Error - unrecognised opcode");
        }
    }
    void performInstruction(int opcode, List<Tuple<int, int>> instructionValues, List<int> outputDiagnosticCodes, List<int> newInputsToReadIn)
    {
        int firstInt, secondInt, valueToWrite;
        var outputValue = 0;
        switch (opcode)
        {
            case 1:  // Addition
                firstInt = getValueFromMode(puzzleInput, instructionValues[0]);
                secondInt = getValueFromMode(puzzleInput, instructionValues[1]);
                // Note to self - using list mutability
                puzzleInput[instructionValues[2].Item1] = firstInt + secondInt;
                break;
            case 2:  // Multiplication
                firstInt = getValueFromMode(puzzleInput, instructionValues[0]);
                secondInt = getValueFromMode(puzzleInput, instructionValues[1]);
                puzzleInput[instructionValues[2].Item1] = firstInt * secondInt;
                break;
            case 3:
                puzzleInput[instructionValues[0].Item1] = newInputsToReadIn[0];
                newInputsToReadIn.RemoveAt(0);
                //Console.WriteLine("Enter an input value");
                //puzzleInput[instructionValues[0].Item1] = Convert.ToInt32(Console.ReadLine());
                break;
            case 4:
                outputValue = getValueFromMode(puzzleInput, instructionValues[0]);
                Console.WriteLine($"The value is {outputValue}");
                outputDiagnosticCodes.Add(outputValue);
                break;
            case 5:
                //jump-if-true
                firstInt = getValueFromMode(puzzleInput, instructionValues[0]);
                if (firstInt != 0)
                {
                    instructionPointer = getValueFromMode(puzzleInput, instructionValues[1]);
                }
                break;
            case 6:
                //jump-if-false: 
                firstInt = getValueFromMode(puzzleInput, instructionValues[0]);
                if (firstInt == 0)
                {
                    instructionPointer = getValueFromMode(puzzleInput, instructionValues[1]);
                }
                break;
            case 7:
                firstInt = getValueFromMode(puzzleInput, instructionValues[0]);
                secondInt = getValueFromMode(puzzleInput, instructionValues[1]);
                valueToWrite = Convert.ToInt32(firstInt < secondInt);
                puzzleInput[instructionValues[2].Item1] = valueToWrite;
                break;
            case 8:
                firstInt = getValueFromMode(puzzleInput, instructionValues[0]);
                secondInt = getValueFromMode(puzzleInput, instructionValues[1]);
                valueToWrite = Convert.ToInt32(firstInt == secondInt);
                puzzleInput[instructionValues[2].Item1] = valueToWrite;
                break;
            default:
                throw new Exception("Unrecognised input");
        }
    }

    int getValueFromMode(List<int> inputNums, Tuple<int, int> input)
    {
        return (input.Item2 == 0) ? inputNums[input.Item1] : input.Item1;
    }

    void incrementInstructionPointer(int instructionLength)
    {
        instructionPointer += instructionLength;
    }


}