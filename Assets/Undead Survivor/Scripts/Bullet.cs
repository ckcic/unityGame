using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per; // 貫通回数

    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir) // 初期化
    {
        this.damage = damage;
        this.per = per;

        if (per >= 0) // 遠距離武器なら
        {
            rigid.velocity = dir * 15f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // 他のコライダーと衝突したら
    {
        if (!collision.CompareTag("Enemy") || per == -100) { return; }

        per--;
        // 貫通回数を減らす

        if (per < 0) // 0未満になった場合は弾丸を停止
        { 
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) // 衝突判定のエリアから出た場合
    {
        if (!collision.CompareTag("Area") || per == -100) { return; }
        // 弾丸を無効化する
        gameObject.SetActive(false);
    }

}
