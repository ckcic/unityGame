using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Sprite sprites;
    public Rigidbody2D target;

    Rigidbody2D rigid;
    Collider2D coll;
    SpriteRenderer spriter;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        spriter = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) { return; }
        GameManager.instance.uiLevelUp.Show();
        gameObject.SetActive(false);
    }
}
