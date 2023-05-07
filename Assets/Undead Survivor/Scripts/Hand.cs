using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool isLeft;
    public SpriteRenderer spriter;

    SpriteRenderer player;

    Vector3 rightPos =  new Vector3 (0.35f, -0.15f, 0);
    Vector3 rightPosReverse =  new Vector3 (-0.15f, -0.15f, 0);
    Quaternion leftRot = Quaternion.Euler (0, 0, -35);
    Quaternion leftRotReverse = Quaternion.Euler (0, 0, -135);

    private void Awake()
    {
        player = GetComponentsInParent<SpriteRenderer>()[1];
        // 자기 자신에게 스프라이트 렌더러가 있으면 포함되기 때문에 인덱스 1
    }

    private void LateUpdate()
    {
        bool isReverse = player.flipX;
        if (isLeft) // 근접무기
        {
            transform.localRotation = isReverse ? leftRotReverse : leftRot;
            spriter.flipY = isReverse;
            spriter.sortingOrder = isReverse? 4 : 6;
        }
        //else if (GameManager.instance.player.scanner.nearestTarget) // 원거리무기 가까운 적으로 움직이게 하기
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
        else // 원거리무기
        {
            transform.localPosition = isReverse ? rightPosReverse : rightPos;
            spriter.flipX = isReverse;
            spriter.sortingOrder = isReverse? 6 : 4;
        }

    }
}
