using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchieveManager : MonoBehaviour
{
    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;
    public GameObject uiNotice;

    enum Achieve { UnlockGrape, UnlockOrange } // アチーブメントを列挙型で定義
    Achieve[] achieves; 
    WaitForSecondsRealtime wait;

    private void Awake()
    {
        achieves = (Achieve[])Enum.GetValues(typeof(Achieve));
        wait = new WaitForSecondsRealtime(5); // 5秒間の待機

        if (!PlayerPrefs.HasKey("MyData"))　
        // プレイヤープリファレンスにキー "MyData"がない場合
        {
            Init();
        }
    }

    void Init() // すべてのアチーブメントを初期化
    {
        PlayerPrefs.SetInt("MyData", 1);

        foreach (Achieve achieve in achieves)
        {
            PlayerPrefs.SetInt(achieve.ToString(), 0);
        }
    }

    private void Start() // オブジェクトが有効になった直後に呼び出される
    {
        UnlockCharacter();
    }

    void UnlockCharacter()
    {
        for (int i = 0; i < lockCharacter.Length; i++)
        {
            string achieveName = achieves[i].ToString();
            // プレイヤープリファレンスの値が1だったらキャラクターを解放
            bool inUnlock = PlayerPrefs.GetInt(achieveName) == 1;
            lockCharacter[i].SetActive(!inUnlock);
            unlockCharacter[i].SetActive(inUnlock);
        }
    }

    private void LateUpdate() // すべての他の更新が完了した後に呼び出される
    {
        foreach (Achieve achieve in achieves) // すべてのアチーブメントをチェック
        {
            CheckAchieve(achieve);
        }
    }

    void CheckAchieve(Achieve achieve)
    {
        bool isAchieve = false;

        switch (achieve)
        {
            case Achieve.UnlockGrape: //敵を10体以上倒すと
                isAchieve = GameManager.instance.kill >= 10; 
                break;
            case Achieve.UnlockOrange: // ゲームをクリアしたら
                isAchieve = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
                break;
        }

        if (isAchieve && PlayerPrefs.GetInt(achieve.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achieve.ToString(), 1);
            // プレイヤープリファレンスの値を1にする、アチーブメント獲得

            for (int i = 0; i < uiNotice.transform.childCount; i++)
            {
                bool isActive = i == (int)achieve;
                uiNotice.transform.GetChild(i).gameObject.SetActive(isActive);
                // アチーブメントの獲得の通知を表示する
            }

            StartCoroutine(NoticeRoutine());
        }
    }

    IEnumerator NoticeRoutine() // アチーブメントの獲得の通知の表示と待機時間を処理
    {
        uiNotice.SetActive(true);

        AudioManager.instance.playSfx(AudioManager.Sfx.LevelUp);

        yield return wait;

        uiNotice.SetActive(false);
    }
}
