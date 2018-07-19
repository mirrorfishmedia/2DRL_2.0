using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Strata
{

    [System.Serializable]
    public class ChanceBoardLibraryEntry
    {
        public ChanceChar[] chanceChars;
        private List<char> outputChars = new List<char>();

        public char[] outputCharArray = new char[100];

        public void BuildChanceCharListProbabilities()
        {
            int arrayPercentIndex = 0;
            for (int i = 0; i < chanceChars.Length; i++)
            {
                for (int j = 0; j < chanceChars[i].percentChanceToChoose; j++)
                {

                    //outputChars.Add(chanceChars[i].outputCharIds);
                    outputCharArray[arrayPercentIndex] = chanceChars[i].outputCharIds;
                    //Debug.Log("outputChars count " + outputChars.Count);
                    arrayPercentIndex++;
                }

            }
        }

        public char GetChanceCharId()
        {
            char outputChar = '0';
            if (outputChars.Count > 0)
            {
                //outputChar = outputChars[Random.Range(0, outputChars.Count)];
                outputChar = outputCharArray[Random.Range(0, outputCharArray.Length)];
            }

            return outputChar;
        }
    }

}
