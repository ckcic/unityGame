using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public Scanner scanner;
    public Hand[] hands;
    public RuntimeAnimatorController[] animCon;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator animator;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);
        // 無効化されたコンポーネントを受け取る時
    }

    private void OnEnable() // オブジェクトが有効になったときに呼ばれる
    {
        speed *= Character.Speed;
        animator.runtimeAnimatorController = animCon[GameManager.instance.playerId];
    }

    private void FixedUpdate()
    {
        // 位置移動, normalized：対角線も同じく
        // Inputsystemですでにnormalizedを適用済み
        if (!GameManager.instance.isLive) { return; }
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    private void LateUpdate() // フレームの最後に呼ばれる
    {
        if (!GameManager.instance.isLive) { return; }
        animator.SetFloat("Speed", inputVec.magnitude);
        // magnitude : ベクターの純粋なサイズ

        if (inputVec.x != 0) { // 左の方に動くとスプライトを反転
            spriter.flipX = inputVec.x < 0;
        }
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

    private void OnCollisionStay2D(Collision2D collision) // プレイヤーが他のオブジェクトと衝突した場合に呼び出される。
    {
        if (!GameManager.instance.isLive) { return; }
        // フレームごとにイベントが発生するため、ディレイを増やす
        GameManager.instance.health -= Time.deltaTime * 10;

        if (GameManager.instance.health <= 0)
        {
            for (int i = 2; i < transform.childCount; i++)
            {
                // GetChild : 与えられたインデックスの子オブジェクトを返す関数
                transform.GetChild(i).gameObject.SetActive(false);
            }

            animator.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }
}
