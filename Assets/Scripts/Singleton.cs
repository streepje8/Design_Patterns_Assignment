// ----------------------------------------------------------------------------
// Singleton Baseclass
// 
// Author: streep
// Date:   24/03/2022
// ----------------------------------------------------------------------------
using UnityEngine;

public class Singleton<T> : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            return instance;
        }
        set
        {
            if (instance == null)
            {
                instance = value;
            }
            else
            {
                instance = value;
                Debug.LogWarning("You can only have one instance of a singleton, ive overwritten the previous singleton instance!"); //This originally was a logError, but i wanted to overwrite the singleton so i changed it!
            }
        }
    }
}