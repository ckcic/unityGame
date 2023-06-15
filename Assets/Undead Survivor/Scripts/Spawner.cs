using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    public float levelTime;

    int level;
    float timer;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        levelTime = GameManager.instance.maxGameTime / spawnData.Length;
    }

    void Update() // フレームごとに実行
    {
        if (!GameManager.instance.isLive) { return; } // ゲームがプレイしている場合のみ処理
        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / levelTime), spawnData.Length - 1);
        // FloorToInt : 小数点以下の値を切り捨てて最も近い整数を返す関数
        // CeilToInt : 小数点以下の値を切り上げて最も近い整数を返す関数

        if (timer > spawnData[level].spawnTime) // spawnData の spawnTime と比較してスポーン
        {
            timer = 0f;
            Spawn();
        }
    }

    void Spawn() // 敵キャラクターをスポーン
    {
        GameObject enemy = GameManager.instance.pool.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position; // スポーンポイントの中からランダムな位置
        enemy.GetComponent<Enemy>().Init(spawnData[level]); // 敵キャラクターの初期化
    }
}

[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}
