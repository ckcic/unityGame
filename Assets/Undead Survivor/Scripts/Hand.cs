using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool isLeft; // 左手かどうか
    public SpriteRenderer spriter; // 手のスプライトレンダラー

    SpriteRenderer player;

    Vector3 rightPos =  new Vector3 (0.35f, -0.15f, 0); // 右手の位置
    Vector3 rightPosReverse =  new Vector3 (-0.15f, -0.15f, 0); // 左手の位置
    // プレイヤーが反転したら左の手の方向も変わるため角度を回転させる
    Quaternion leftRot = Quaternion.Euler (0, 0, -35); 
    Quaternion leftRotReverse = Quaternion.Euler (0, 0, -135);

    private void Awake()
    {
        player = GetComponentsInParent<SpriteRenderer>()[1];
        // 自分自身にスプライトレンダラーがあれば含まれるのでインデックス 1
    }

    private void LateUpdate()
    {
        bool isReverse = player.flipX;　// プレイヤーが反転したかどうか
        if (isLeft) // 左手
        {
            transform.localRotation = isReverse ? leftRotReverse : leftRot;
            spriter.flipY = isReverse;
            spriter.sortingOrder = isReverse? 4 : 6;
        }
        //else if (GameManager.instance.player.scanner.nearestTarget) // 遠距離武器を近くの敵に移動させる
        //{
        //    Vector3 targetPos = GameManager.instance.player.scanner.nearestTarget.position;
        //    Vector3 dir = targetPos - transform.position;
        //    transform.localPosition = isReverse ? rightPosReverse : rightPos;
        //    transform.localRotation = Quaternion.FromToRotation(Vector3.right, dir);
        //    spriter.flipX = isReverse;
        //    spriter.sortingOrder = 6;

        //    bool isRotA = transform.localRotation.eulerAngles.z > 90 && transform.localRotation.eulerAngles.z < 270;
        //    bool isRotB = transform.localRotation.eulerAngles.z < -90 && transform.localRotation.eulerAngles.z > -270;
        //    spriter.flipY = isRotA || isRotB;
        //}
        else // 右手
        {
            transform.localPosition = isReverse ? rightPosReverse : rightPos;
            spriter.flipX = isReverse;
            spriter.sortingOrder = isReverse? 6 : 4;
        }

    }
}
