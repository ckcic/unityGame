using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange; // スキャンする範囲の半径
    public LayerMask targetLayer; // スキャン対象のレイヤーマスク
    public RaycastHit2D[] targets; // スキャンされた対象物の情報を格納するための RaycastHit2D の配列
    public Transform nearestTarget; // 最も近い対象物の Transform コンポーネント

    private void FixedUpdate()
    {
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        // CircleCastAll( キャスティングの位置、円の半径、キャスティングの方向、キャスティングの長さ、対象レイヤー) : 円形のキャストを撃ち、全ての結果を返す関数
        nearestTarget = GetNearset();
    }

    Transform GetNearset()
    {
        Transform result = null;
        float diff = 100;

        foreach (RaycastHit2D target in targets) // スキャンされた対象物の中から最も近い対象物を探し、その Transform コンポーネントを返す
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(myPos, targetPos);

            if (curDiff < diff) 
            { 
                diff = curDiff;
                result = target.transform;
            }
        }

        return result;
    }
}
