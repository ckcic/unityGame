using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;

    bool isLive;

    Rigidbody2D rigid;
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;

    private void Awake() // 初期化処理
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.isLive) { return; }
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit")) { return; }
        // GetCurrentAnimatorStateInfo : 現在の状態情報を取得する関数
        // 生きているし、攻撃を受けていない場合
        // プレイヤーの方向に向かって移動
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;

    }

    private void LateUpdate()
    {
        if (!GameManager.instance.isLive) { return; }
        if (!isLive) { return; }
        // プレイヤーの位置が左の方にあるとスプライトを反転する
        spriter.flipX = target.position.x < rigid.position.x;
    }

    private void OnEnable() // オブジェクトが有効になったときに呼ばれる
    {
        // 敵の初期化処理
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
        health = maxHealth;
    }

    public void Init(SpawnData data) // スポーンデータを受け取って敵のパラメータを設定
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    private void OnTriggerEnter2D(Collider2D collision)　// 他のオブジェクトとの衝突時に呼ばれる
    {
        if (!collision.CompareTag("Bullet") || !isLive) { return; }
        // 敵が生きているし、弾丸に当たった場合
        health -= collision.GetComponent<Bullet>().damage;

        //StartCoroutine("KnockBack");
        StartCoroutine(KnockBack());

        if (health > 0) 
        {
            // .. Live, Hit Action
            anim.SetTrigger("Hit");
            AudioManager.instance.playSfx(AudioManager.Sfx.Hit);
        }
        else // 体力が0以下になると敵は死亡
        {
            // .. Die
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);
            GameManager.instance.kill++;
            //GameManager.instance.GetExp();
            Dead();
            // 経験値を落とす
            Transform exp = GameManager.instance.pool.Get(3).transform;
            exp.position = transform.position;
            exp.GetComponent<Exp>().Init(1);
            float randomNumber = Mathf.Round(UnityEngine.Random.Range(1f, 10f) * 100) / 100f;
            // 確率的にChestを落とす
            if (randomNumber >= 9.95f)
            {
                // Debug.Log("Chest Drop");
                Transform chest = GameManager.instance.pool.Get(4).transform;
                chest.position = transform.position;
            }
            if (GameManager.instance.isLive) 
            { 
                AudioManager.instance.playSfx(AudioManager.Sfx.Dead); 
            }
        }
    }

    // コルーチン Coroutine : ライフサイクルと非同期のように実行される関数
    // IEnumerator : コルーチンだけの戻り型インターフェース
    IEnumerator KnockBack() // コルーチンで、敵が攻撃を受けた際のノックバック効果
    {
        // yield : C#のコルーチン（Coroutine）機能において使用されるキーワード
        // yield return null; // 1フレーム待機。
        // yield return new WaitForSeconds(2f); // 2秒間待機

        yield return wait;  // 物理演算フレームが完了するまで待機
        Vector3 palyerPos = GameManager.instance.player.transform.position;
        // プレイヤー基準の反対方向 : 現在の位置 - プレイヤーの位置
        // 敵はプレイヤーの位置に対して反対方向に力を受ける
        Vector3 dirVec = transform.position - palyerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        // Impulse : 瞬間的な力

    }


    void Dead()
    {
        gameObject.SetActive(false);
        // オブジェクトプーリングを使うので、無効化、削除しない
    }

}
