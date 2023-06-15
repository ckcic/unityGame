    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    Collider2D coll;
    private void Awake()
    {
        coll = GetComponent<Collider2D>(); // オブジェクトの Collider2D コンポーネントへの参照
    }
    private void OnTriggerExit2D(Collider2D collision) 
    // オブジェクトが他の Collider2D とのトリガー領域から出た場合に呼び出される
    {
        if (!collision.CompareTag("Area")) { return; } // 出た領域のタグが "Area" でない場合は処理を終了

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;

        float dirX = playerPos.x - myPos.x;
        float dirY = playerPos.y - myPos.y;
        float diffX = Mathf.Abs(dirX);
        float diffY = Mathf.Abs(dirY);
        dirX = dirX > 0 ? 1 : -1;
        dirY = dirY > 0 ? 1 : -1;
        
        // 相対的な位置関係に基づいて、オブジェクトを再配置
        switch (transform.tag) 
        {
            case "Ground":
                if (diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 40);
                }
                else if (diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 40);
                }
                else
                {
                    transform.Translate(Vector3.right * dirX * 40);
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;
            case "Enemy":
                if(coll.enabled) 
                {
                    Vector3 dist = playerPos - myPos;
                    Vector3 ran = new Vector3(Random.Range(-3,3), 0);
                    transform.Translate(ran + dist * 2);
                }
                break;
        }
    }
}
