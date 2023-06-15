using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        // カメラをプレイヤーを追うようにする
        rect.position = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);
        // WorldToScreenPoint : Unityのカメラに関連付けられたワールド座標をスクリーン座標に変換するための関数
    }
}
