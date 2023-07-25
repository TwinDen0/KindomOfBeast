using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsManager : MonoBehaviour
{
    [SerializeField] GameObject[] levels;
    [SerializeField] int level;
    private void Start()
    {
        if (PlayerPrefs.HasKey("Level"))
        {
            level = PlayerPrefs.GetInt("Level");
            if(level >= levels.Length)
            {
                level = levels.Length - 1;
                PlayerPrefs.SetInt("Level", levels.Length - 1);
            }
        }
        else
        {
            PlayerPrefs.SetInt("Level", 0);
        }
        Instantiate(levels[level], transform);
    }
}
