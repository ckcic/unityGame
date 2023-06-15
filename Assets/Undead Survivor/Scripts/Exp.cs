using System.Collections;
using UnityEngine;

public class Exp : MonoBehaviour
{
    public float expValue;
    public Sprite[] sprites;
    public Rigidbody2D target;

    public bool isCollect;
    public bool isMerge;

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
        if (!isMerge)
        {
            isCollect = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                isCollect = false;
                gameObject.SetActive(false);
                GameManager.instance.GetExp(expValue);
                break;
            case "Exp":
                Exp other = collision.gameObject.GetComponent<Exp>();
                if (!other.isCollect && !isCollect && !other.isMerge && !isMerge)
                {
                    float meX = transform.position.x;
                    float meY = transform.position.y;
                    float otherX = other.transform.position.x;
                    float otherY = other.transform.position.y;
                    // 아래있는 경우, 오른쪽에 있는 경우
                    if (meY < otherY || meX > otherX)
                    {
                        // 상대방 숨기기
                        other.Hide(transform.position);
                        // 경험치량 증가
                        expValue += other.expValue;
                    }
                }
                break;
        }
    }


    public void Hide(Vector3 targetPos)
    {
        isMerge = true;
        if (gameObject.activeSelf)
        {
            StartCoroutine(HideRoutine(targetPos));
        }
    }

    IEnumerator HideRoutine(Vector3 targetPos)
    {
        int frameCount = 0;
        while (frameCount < 10)
        {
            frameCount++;
            if (gameObject.activeSelf)
            {
                transform.position = Vector3.Lerp(transform.position, targetPos, 0.5f);
            }
            yield return null;
        }

        isMerge = false;
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }


    private void FixedUpdate()
    {
        if (!GameManager.instance.isLive) { return; }

        if (expValue >= 10 && expValue < 50)
        {
            spriter.sprite = sprites[1];
        }
        else if (expValue >= 50)
        {
            spriter.sprite = sprites[2];
        }
        else
        {
            spriter.sprite = sprites[0];
        }

        if (!isCollect) { return; }
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
