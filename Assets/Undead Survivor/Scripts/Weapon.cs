using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    float timer;
    Player player;

    private void Awake()
    {
        player = GameManager.instance.player;
    }

    void Update()
    {
        if (!GameManager.instance.isLive) { return; }
        switch (id)
        {
            case 0: // 0 の場合は近距離武器、武器を回転
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            case 1: // 1 の場合は遠距離武器、一定間隔で発射
                timer += Time.deltaTime;

                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;

            default:
                break;
        }
    }

    public void LevelUp(float damage, int count) // 武器のレベルアップ時に呼び出される
    {
        this.damage = damage * Character.Damage;
        this.count += count;

        if (id == 0)
        {
            PlaceWeapon();
        }

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
        //　Gear.ApplyGear()
    }

    public void Init(ItemData data) // 武器を初期化
    {
        // Basic Set
        name = "Weapon " + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        // Property Set
        id = data.itemId;
        damage = data.baseDamage * Character.Damage;
        count = data.baseCount + Character.Count;

        for (int i = 0; i < GameManager.instance.pool.prefabs.Length; i++) 
        {
            if (data.projectile == GameManager.instance.pool.prefabs[i])
            {
                prefabId = i;
                break;
            }
        }

        switch (id)
        {
            case 0:
                speed = 150 * Character.AtackSpeed;
                PlaceWeapon();
                break;
            case 1:
                speed = 0.5f * Character.AtackRate;
                break;

            default:
                break;
        }

        // Hand Set
        Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
        // BroadcastMessage : ゲームオブジェクトまたは子オブジェクトにあるすべての MonoBehaviour を継承したクラスにある methodName 名のメソッドを呼び出します。
    }

    void PlaceWeapon() // 武器を配置
    {
        for (int i = 0; i < count; i++)
        {
            Transform bullet;

            if (i < transform.childCount)
            {
                bullet = transform.GetChild(i);
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }

            // bullet.localPosition = Vector3.zero;
            // bullet.localRotation = Quaternion.identity;
            bullet.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity); // Transformのローカル座標と回転を一度に設定


            Vector3 rotVec = Vector3.forward * 360 * i / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); // -100 is Infinity Per.
        }
    }

    void Fire() // プレイヤーのスキャナーで最も近いターゲットに向けて弾丸を発射
    {
        if (!player.scanner.nearestTarget) { return; }

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir);

        AudioManager.instance.playSfx(AudioManager.Sfx.Range);
    }
}
