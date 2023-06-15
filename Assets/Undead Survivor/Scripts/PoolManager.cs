using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // プールするオブジェクトのプレハブを格納する配列
    public GameObject[] prefabs;

    // 各プレハブのオブジェクトプールを管理するためのリストの配列
    List<GameObject>[] pools;

    private void Awake()
    {
        pools = new List<GameObject>[prefabs.Length]; // オブジェクトプールのリストを初期化

        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }

        // Debug.Log(pools.Length);
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        // 指定されたインデックスのプールから無効化されたオブジェクトを取得します。
        foreach (GameObject item in pools[index]) 
        { 
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // 見つかった場合
        if (!select) 
        {
            // 新しいオブジェクトを作成
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}
