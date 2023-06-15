using UnityEngine;

public class ChestOpen : MonoBehaviour // 後でChest用に変える必要がある
// LevelUp同じ
{
    RectTransform rect;
    public Item[] items;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void Show() // レベルアップ画面を表示
    {
        Next();
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
        // AudioManager.instance.playSfx(AudioManager.Sfx.LevelUp);
        AudioManager.instance.EffectBgm(true);
    }

    public void Hide() // レベルアップ画面を非表示
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
        // AudioManager.instance.playSfx(AudioManager.Sfx.Select);
        AudioManager.instance.EffectBgm(false);
    }

    public void Select(int index)
    {
        items[index].OnClick(); // Item.OnClick(), LevelUp.Hide()が呼び出される
    }

    void Next()
    {
        // 1. すべてのアイテムを無効化する
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }
        // 2. その中でランダムに3つのアイテムを選択
        int[] ran = new int[3];
        while (true)
        {
            ran[0] = Random.Range(0, items.Length);
            ran[1] = Random.Range(0, items.Length);
            ran[2] = Random.Range(0, items.Length);

            if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2]) { break; }
        }

        for (int i = 0; i < ran.Length; i++)
        {
            Item ranItem = items[ran[i]];
            // 3. 最大レベルのアイテムは消費アイテムにする
            if (ranItem.level == ranItem.data.damages.Length)
            {
                items[Random.Range(4, items.Length)].gameObject.SetActive(true);
            }
            else
            {
                ranItem.gameObject.SetActive(true);
            }
        }
    }
}
