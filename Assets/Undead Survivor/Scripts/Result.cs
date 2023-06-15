using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour // ゲームの結果を表示
{
    public GameObject[] titles;

    public void Lose() // ゲームに負けた場合
    {
        titles[0].gameObject.SetActive(true);
    }

    public void Win() // ゲームに勝った場合
    {
        titles[1].gameObject.SetActive(true);
    }
}
