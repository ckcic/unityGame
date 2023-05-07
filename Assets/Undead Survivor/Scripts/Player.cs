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
        // 비활성화된 컴포넌트를 받을 때
    }

    private void OnEnable()
    {
        speed *= Character.Speed;
        animator.runtimeAnimatorController = animCon[GameManager.instance.playerId];
    }

    //void Update()
    //{
    //    if (!GameManager.instance.isLive) { return; }
    //    inputVec.x = Input.GetAxisRaw("Horizontal");
    //    inputVec.y = Input.GetAxisRaw("Vertical");
    //}

    private void FixedUpdate()
    {
        /*
        // 1. 힘을 준다
        //rigid.AddForce(inputVec);

        // 2. 속도 제어
        //rigid.velocity = inputVec;
        */

        // 3. 위치 이동, normalized 대각선도 크기를 같게
        // Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        // Inputsystem 에서 이미 노멀라이즈를 적용함
        if (!GameManager.instance.isLive) { return; }
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    private void LateUpdate()
    {
        if (!GameManager.instance.isLive) { return; }
        animator.SetFloat("Speed", inputVec.magnitude);
        // magnitude : 벡터의 순수한 크기

        if (inputVec.x != 0) { 
            spriter.flipX = inputVec.x < 0;
        }
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive) { return; }
        // 프레임마다 이벤트가 발생하므로 딜레이를 줌
        GameManager.instance.health -= Time.deltaTime * 10;

        if (GameManager.instance.health <= 0)
        {
            for (int i = 2; i < transform.childCount; i++)
            {
                // GetChild : 주어진 인덱스의 자식 오브젝트를 반환하는 함수, transform이나와서 gameObject로 올라가야함
                transform.GetChild(i).gameObject.SetActive(false);
            }

            animator.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }
}
