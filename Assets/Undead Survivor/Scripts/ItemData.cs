using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 커스텀 메뉴를 생성하는 속성
[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{
    public enum ItemType { Melee, Range, Glove, Shoe, Heal } // アイテムの種類を表す列挙型

    [Header("# Main Info")]
    public ItemType itemType;
    public int itemId; // アイテムの識別子
    public string itemName; // アイテムの名前
    [TextArea]
    public string itemDesc; // アイテムの説明
    public Sprite itemIcon; // アイテムのアイコンスプライト

    [Header("# Level Data")]
    public float baseDamage; //基本のダメージ量
    public int baseCount; // 基本のカウント数
    public float[] damages; // 各レベルごとのダメージ増加率を示す配列
    public int[] counts; // 各レベルごとのカウント増加量を示す配列

    [Header("# Weapon")]
    public GameObject projectile; // 発射される弾のプレハブ
    public Sprite hand; // 武器を表示される手のスプライト
}
