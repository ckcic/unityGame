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

    private void Awake()
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
        // GetCurrentAnimatorStateInfo : 현재 상태 정보를 가져오는 함수
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        if (!GameManager.instance.isLive) { return; }
        if (!isLive) { return; }
        spriter.flipX = target.position.x < rigid.position.x;
    }

    private void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
        health = maxHealth;
    }

    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive) { return; }

        health -= collision.GetComponent<Bullet>().damage;

        //StartCoroutine("KnockBack");
        StartCoroutine(KnockBack());

        if (health > 0) 
        {
            // .. Live, Hit Action
            anim.SetTrigger("Hit");
            AudioManager.instance.playSfx(AudioManager.Sfx.Hit);
        }
        else
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
            Transform exp = GameManager.instance.pool.Get(3).transform;
            exp.position = transform.position;
            exp.GetComponent<Exp>().Init(1);
            float randomNumber = Mathf.Round(UnityEngine.Random.Range(1f, 10f) * 100) / 100f;   // 소수점 둘째 자리까지 표현
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

    // 코루틴 Coroutine : 생명 주기와 비동기처럼 실행되는 함수
    // IEnumerator : 코루틴만의 반환형 인터페이스
    IEnumerator KnockBack()
    {
        // yield : 코루틴의 반환 키워드
        //yield return null;  // 1프레임 쉬기
        //yield return new WaitForSeconds(2f);  // 2초 쉬기

        yield return wait;  // 하나의 물리 프레임을 딜레이
        Vector3 palyerPos = GameManager.instance.player.transform.position;
        // 플레이어 기준의 반대 방향 : 현재 위치 - 플레이어 위치
        Vector3 dirVec = transform.position - palyerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        // Impulse : 순간적인 힘

    }

    
    void Dead()
    {
        gameObject.SetActive(false);
        // 오브젝트 풀링을 쓰기 때문에 비활성화, 파괴하지 않음
    }
    
}
