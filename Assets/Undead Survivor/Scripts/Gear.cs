using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType type; // 列挙型の値を格納
    public float rate; // 装備の効果率

    public void Init(ItemData data) // ギアの基本設定とプロパティを初期化
    {
        // Basic Set
        name = "Gear " + data.itemId;
        transform.parent = GameManager.instance.player.transform; // 親オブジェクトを設定
        transform.localPosition = Vector3.zero; // 位置をプレイヤーの位置に合わせる

        // Property Set
        type = data.itemType;
        rate = data.damages[0];
        ApplyGear();
    }

    public void LevelUp(float rate) // 装備がレベルアップした場合
    {
        this.rate = rate;
        ApplyGear();
    }

    void ApplyGear() // 装備の種類によって処理
    {
        switch(type) 
        {
            case ItemData.ItemType.Glove:
                RateUp();
                break;
            case ItemData.ItemType.Shoe:
                SpeedUp(); 
                break;
        }
    }

    void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();
        foreach(Weapon weapon in weapons)
        {
            switch(weapon.id)
            {
                case 0: // 近距離武器、回転速度を早める
                    float speed = 150 * Character.AtackSpeed;
                    weapon.speed = speed + (speed * rate);
                    break;
                default: // 他の武器、今は遠距離武器しかいない、発射のディレイの減少
                    speed = 0.5f * Character.AtackRate;
                    weapon.speed = speed * (1f - rate);
                    break;
            }
        }
    }

    void SpeedUp() // プレイヤーの移動速度を上昇させる
    {
        float speed = 3 * Character.Speed; 
        GameManager.instance.player.speed = speed + (speed * rate);
    }
}
