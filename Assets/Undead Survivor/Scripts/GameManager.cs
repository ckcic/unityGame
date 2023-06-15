using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("# Game Control")]
    public bool isLive; // ゲームがプレイ中かどうか
    public float gameTime; // ゲームの経過時間
    public float maxGameTime = 60 * 20f;
    [Header("# Player Info")]
    public int playerId;
    public float health;
    public float maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600}; // 次のレベルに必要な経験値の配列
    [Header("# Game Object")]
    public PoolManager pool; // オブジェクトプールを管理するPoolManagerクラスのインスタンス
    public Player player; // プレイヤーオブジェクトの参照
    public LevelUp uiLevelUp; // レベルアップUIの参照
    public ChestOpen uiChestOpen; // ChestOpenUIの参照
    public Result uiResult; // ゲーム結果UIの参照
    public Transform uiJoy; // ジョイスティックUIの参照
    public GameObject enemyCleaner; // 敵をクリーンアップするオブジェクトの参照

    private void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
    }

    public void GameStart(int id) // キャラクターを選択する時に呼び出される。
    {
        playerId = id;
        health = maxHealth;
        uiJoy.localScale = Vector3.one;
        player.gameObject.SetActive(true); // 有効化する
        uiLevelUp.Select(playerId % 2);
        isLive = true;

        AudioManager.instance.playBgm(true);
        AudioManager.instance.playSfx(AudioManager.Sfx.Select);
    }

    public void GameOver() // キャラクターが死んだ時に呼び出される。
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;

        yield return new WaitForSeconds(0.5f); // 0.5秒待機

        uiResult.gameObject.SetActive(true); // 有効化する
        uiResult.Lose();
        Stop();

        AudioManager.instance.playBgm(false); // Bgmを停止する
        AudioManager.instance.playSfx(AudioManager.Sfx.Lose);
    }

    public void GameVictory() // ゲームを勝った時に呼び出される。
    {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f); // 0.5秒待機

        uiResult.gameObject.SetActive(true); // 有効化する
        uiResult.Win();
        Stop();

        AudioManager.instance.playBgm(false); // Bgmを停止する
        AudioManager.instance.playSfx(AudioManager.Sfx.Win);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
        // LoadScene : 名前もしくはインデックスでシーンを新たに呼び出す関数、リセット
        Resume();
    }

    public void GameQuit()
    {
        Application.Quit();
    }

    void Update()
    {
        if (!isLive) { return; }
        // ゲームをプレイしている場合
        
        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime) // ゲームの経過時間がmaxGameTimeを超えた時、勝利する
        {
            gameTime = maxGameTime;
            GameVictory();
        }
    }

    public void GetExp(float expValue)
    {
        if(!isLive) { return; }

        exp += (int)expValue;

        if(exp >= nextExp[Mathf.Min(level, nextExp.Length-1)])
        // 次のレベルアップに必要な経験値または最後の要素以上であれば
        {
            level++; // レベルを1つ上げる
            exp = 0; // 0にリセット
            uiLevelUp.Show(); // レベルアップUIを表示
        }
    }

    public void Stop() // ゲームプレイ一時停止
    {
        isLive = false;
        // timeScaleプロパティは時間の流れる速度の倍率を設定
        Time.timeScale = 0;
        uiJoy.localScale = Vector3.zero;
    }
    public void Resume() // ゲームプレイ再開
    {
        isLive = true;
        Time.timeScale = 1;
        uiJoy.localScale = Vector3.one;
    }
}
