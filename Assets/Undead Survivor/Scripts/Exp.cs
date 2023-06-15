using System.Collections;
using UnityEngine;

public class Exp : MonoBehaviour
{
    public float expValue; // 経験値の値を格納
    public Sprite[] sprites;
    public Rigidbody2D target;

    public bool isCollect; // 経験値オブジェクトが収集状態かどうか
    public bool isMerge; // 経験値オブジェクトがマージ状態かどうか

    Rigidbody2D rigid;
    Collider2D coll;
    SpriteRenderer spriter;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>(); // 経験値オブジェクトのRigidbody2Dコンポーネントを参照
        coll = GetComponent<Collider2D>(); // 経験値オブジェクトのCollider2Dコンポーネントを参照
        spriter = GetComponent<SpriteRenderer>(); // 経験値オブジェクトのSpriteRendererコンポーネントを参照
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Collect")) { return; }
        // 「Collect」タグのオブジェクトに衝突した場合
        if (!isMerge) // 経験値オブジェクトがマージ状態でない時
        {
            isCollect = true; // 経験値オブジェクトが収集状態にする
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player": // 経験値オブジェクトがプレイヤーと衝突した場合
                isCollect = false; // 経験値オブジェクトの収集状態を解除
                gameObject.SetActive(false); // 経験値オブジェクトを無効化
                GameManager.instance.GetExp(expValue); // 経験値を上げる
                break;
            case "Exp": // 経験値オブジェクトが他の経験値オブジェクト衝突した場合
                Exp other = collision.gameObject.GetComponent<Exp>();
                if (!other.isCollect && !isCollect && !other.isMerge && !isMerge)
                {
                    float meX = transform.position.x;
                    float meY = transform.position.y;
                    float otherX = other.transform.position.x;
                    float otherY = other.transform.position.y;
                    // 経験値オブジェクトが下にある場合または右にある場合
                    if (meY < otherY || meX > otherX)
                    {
                        // 他のオブジェクトを無効化
                        other.Hide(transform.position);
                        // 経験値の値を増やす
                        expValue += other.expValue;
                    }
                }
                break;
        }
    }


    public void Hide(Vector3 targetPos)
    {
        isMerge = true; // 経験値オブジェクトをマージ状態にする
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
            if (gameObject.activeSelf) // 経験値を移動させてマージするように見せる
            {
                transform.position = Vector3.Lerp(transform.position, targetPos, 0.5f);
            }
            yield return null;
        }

        isMerge = false; // 経験値オブジェクトのマージ状態を解除
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }


    private void FixedUpdate()
    {
        if (!GameManager.instance.isLive) { return; }

        // 経験値の値によって経験値のスプライトを変更
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
        // 経験値オブジェクトをプレイヤーの位置に向かって移動させる
        float speed = 5;
        Vector2 targetPosition = GameManager.instance.player.transform.position;
        Vector2 dirVec = targetPosition - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }


    public void Init(float expValue) // 経験値オブジェクトの初期設定
    {
        this.expValue = expValue;
    }
}
