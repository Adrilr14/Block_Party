using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour
{
    public static int[] shuffleIntArray(int[] array)
    {
        for (int positionOfArray = 0; positionOfArray < array.Length; positionOfArray++)
        {
            int n = array[positionOfArray];
            int randomizeArray = Random.Range(0, positionOfArray);
            array[positionOfArray] = array[randomizeArray];
            array[randomizeArray] = n;
        }
        return array;
    }
}
