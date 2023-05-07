using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    public GameObject[] titles;

    public void Lose()
    {
        titles[0].gameObject.SetActive(true);
    }

    public void Win()
    {
        titles[1].gameObject.SetActive(true);
    }
}
