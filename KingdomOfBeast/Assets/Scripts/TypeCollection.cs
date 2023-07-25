using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeCollection : MonoBehaviour
{
    public List<GameObject> levels;
    public void SetCount(int count)
    {
        for (int i = 0; i < count; i++)
        {
            levels[i].GetComponent<Image>().color = Color.white;
        }
    }
}
