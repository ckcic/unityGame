using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Exp : MonoBehaviour
{
    public float expValue;
    public Sprite[] sprites;
    public Rigidbody2D target;
    public int prefabId = 3;

    bool isCollect;

    Rigidbody2D rigid;
    Collider2D coll;
    SpriteRenderer spriter;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        spriter = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Collect")) { return; }
            isCollect = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) { return; }
        isCollect = false;
        gameObject.SetActive(false);
        GameManager.instance.GetExp(expValue);
    }
    private void FixedUpdate()
    {
        if (!GameManager.instance.isLive) { return; }
        if (!isCollect ) { return; }
        float speed = 5;
        // GetCurrentAnimatorStateInfo : 현재 상태 정보를 가져오는 함수
        Vector2 targetPosition = GameManager.instance.player.transform.position;
        Vector2 dirVec = targetPosition - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }


    public void Init(float expValue)
    {
        this.expValue = expValue;
    }
}
