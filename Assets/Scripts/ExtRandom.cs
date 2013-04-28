using UnityEngine;
using System.Collections.Generic;
 
public class ExtRandom<T>
{
    public static void RandomizeSeed()
    {
        Random.seed = System.Math.Abs(((int)(System.DateTime.Now.Ticks % 2147483648L) - (int)(Time.realtimeSinceStartup + 2000f)) / ((int)System.DateTime.Now.Day - (int)System.DateTime.Now.DayOfWeek * System.DateTime.Now.DayOfYear));
        Random.seed = System.Math.Abs((int)((Random.value * (float)System.DateTime.Now.Ticks * (float)Random.Range(0, 2)) + (Random.value * Time.realtimeSinceStartup * Random.Range(1f, 3f))) + 1);
    }
 	
	/// <summary>
	/// Random 1/2 chance 
	/// </summary>
    public static bool SplitChance()
    {
        return Random.Range(0, 2) == 0 ? true : false;
    }
 
	/// <summary>
	/// The chance below given probality factor
	/// </summary>
    public static bool Chance(int nProbabilityFactor, int nProbabilitySpace)
    {
        return Random.Range(0, nProbabilitySpace) < nProbabilityFactor ? true : false;
    }
 
	/// <summary>
	/// Random choose from a array
	/// </summary>
    public static T Choice(T[] array)
    {
        return array[Random.Range(0, array.Length)];
    }
	
	/// <summary>
	/// Choice the specified number of member from input array.
	/// </summary>
	public static List<T> Choice(T[] array, int number){
		return Choice(new List<T>(array), number);
	}
 
	
	/// <summary>
	/// Random choose from a List
	/// </summary>
    public static T Choice(List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }
	
	/// <summary>
	/// Choice the specified number of members from list.
	/// </summary>
	public static List<T> Choice(List<T> list, int number){
		if(number >= list.Count || number < 0){
			return list;
		}else if(number == 0){
			return new List<T>();
		}else{
			List<T> result = new List<T>(list);
			while(result.Count != number){
				T tmp = Choice(result);
				result.Remove(tmp);
			}
			return result;
		}
	}
 
	/// <summary>
	/// Random choose from a array with weights
	/// </summary>
    public static T WeightedChoice(T[] array, int[] nWeights)
    {
        int nTotalWeight = 0;
        for(int i = 0; i < array.Length; i++)
        {
            nTotalWeight += nWeights[i];
        }
        int nChoiceIndex = Random.Range(0, nTotalWeight);
        for(int i = 0; i < array.Length; i++)
        {
            if(nChoiceIndex < nWeights[i])
            {
                nChoiceIndex = i;
                break;
            }
            nChoiceIndex -= nWeights[i];
        }
 
        return array[nChoiceIndex];
    }
 
	/// <summary>
	/// Random choose from a List with weights
	/// </summary>
    public static T WeightedChoice(List<T> list, List<int> nWeights)
    {
        int nTotalWeight = 0;
        for(int i = 0; i < list.Count; i++)
        {
            nTotalWeight += nWeights[i];
        }
        int nChoiceIndex = Random.Range(0, nTotalWeight);
        for(int i = 0; i < list.Count; i++)
        {
            if(nChoiceIndex < nWeights[i])
            {
                nChoiceIndex = i;
                break;
            }
            nChoiceIndex -= nWeights[i];
        }
 
        return list[nChoiceIndex];
    }
 
	/// <summary>
	/// Shuffle the specified array.
	/// </summary>
    public static T[] Shuffle(T[] array)
    {
        T[] shuffledArray = new T[array.Length];
        List<int> elementIndices = new List<int>(0);
        for(int i = 0; i < array.Length; i++)
        {
            elementIndices.Add(i);
        }
        int nArrayIndex;
        for(int i = 0; i < array.Length; i++)
        {
            nArrayIndex = elementIndices[Random.Range(0, elementIndices.Count)];
            shuffledArray[i] = array[nArrayIndex];
            elementIndices.Remove(nArrayIndex);
        }
 
        return shuffledArray;
    }
 
	/// <summary>
	/// Shuffle the specified list.
	/// </summary>
    public static List<T> Shuffle(List<T> list)
    {
        List<T> shuffledList = new List<T>(0);
        int nListCount = list.Count;
        int nElementIndex;
        for(int i = 0; i < nListCount; i++)
        {
            nElementIndex = Random.Range(0, list.Count);
            shuffledList.Add(list[nElementIndex]);
            list.RemoveAt(nElementIndex);
        }
 
        return shuffledList;
    }
}