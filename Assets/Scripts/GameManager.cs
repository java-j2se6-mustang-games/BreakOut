using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Game Manager in the game scene. 
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("Player")]
    //-----------------------------------
    // Control fields
    //-----------------------------------
    #region Control fields

    /// <summary>
    /// Initial life of the game.
    /// </summary>
    [Tooltip("Initial life of the game.")]
    public int lifeMax = 3; // For the time being, set your own machine to 3.

    /// <summary>
    /// Movement speed of own machine.
    /// </summary>
    [Tooltip("Movement speed of own machine.")]
    public float playerSpeed = 1.0f;  // For the time being, set your own speed to 1f.

    /// <summary>
    /// Score to be displayed in the game scene.
    /// </summary>
    public static int score = 0;
    int oldScore = -1;

    /// <summary>
    /// Time to be displayed in the game scene.
    /// </summary>
    public static float timeAttack = -1f;

    /// <summary>
    /// Life to be displayed in the game scene.
    /// </summary>
    int life = 0;
    int oldLife = -1;

    /// <summary>
    /// Basetime to be cache in the game scene.
    /// </summary>
    float baseTime = -1f;

    /// <summary>
    /// The time measured in the game scene.
    /// </summary>
    float elapsedTime = -1f;
    float oldElapsedTime = -1f;

    /// <summary>
    /// Stoptime to be cache in the game scene.
    /// </summary>
    float stopTime = -1f;

    /// <summary>
    /// Result Scene Display Text.
    /// </summary>
    string result = "";
    #endregion

    //-----------------------------------
    // GameUIControl fields
    //-----------------------------------
    #region GameUIControl fields
    /// <summary>
    /// For block group management.
    /// </summary>
    [Tooltip("For block group management.")]
    public IList<GameObject> blocks = new List<GameObject>();

    /// <summary>
    /// For reference to own machine.
    /// </summary>
    [Tooltip("For reference to own machine.")]
    public GameObject Player;

    [Header("Ball")]
    /// <summary>
    /// For ball reference.
    /// </summary>
    [Tooltip("For ball reference.")]
    public Ball ball;

    [Header("GameUIControl")]
    /// <summary>
    /// For Gameover canvas reference.
    /// </summary>
    [Tooltip("For Gameover canvas reference.")]
    public GameObject gameOverCanvas;

    /// <summary>
    /// For Life Text UI reference.
    /// </summary>
    [Tooltip("For Life Text UI reference.")]
    public GameObject lifeObject;

    /// <summary>
    /// For Score Text UI reference.
    /// </summary>
    [Tooltip("For Score Text UI reference.")]
    public GameObject scoreObject;

    /// <summary>
    /// For Time Text UI reference.
    /// </summary>
    [Tooltip("For Time Text UI reference.")]
    public GameObject timeObject;

    ///// <summary>
    ///// BGM Controller in the menu scene.
    ///// </summary>
    //[Tooltip("BGM Controller in the menu scene.")]
    //public GameObject menuBgmObject;

    /// <summary>
    /// BGM Controller in the game scene.
    /// </summary>
    [Tooltip("BGM Controller in the game scene.")]
    public GameObject bgmObject;

    ///// <summary>
    ///// BGM Controller in the result scene.
    ///// </summary>
    //[Tooltip("BGM Controller in the result scene.")]
    //public GameObject resultBgmObject;
    #endregion

    #region GameUIControl(Color Block) fields  fields
    //-----------------------------------
    // GameUIControl(Color Block) fields 
    //-----------------------------------
    /// <summary>
    /// For red block management.
    /// </summary>
    [Tooltip("For red block management.")]
    public GameObject redBlock;

    /// <summary>
    /// For orange block management.
    /// </summary>
    [Tooltip("For orange block management.")]
    public GameObject orangeBlock;

    /// <summary>
    /// For yellow block management.
    /// </summary>
    [Tooltip("For yellow block management.")]
    public GameObject yellowBlock;

    /// <summary>
    /// For green block management.
    /// </summary>
    [Tooltip("For green block management.")]
    public GameObject greenBlock;

    /// <summary>
    /// For skyblue block management.
    /// </summary>
    [Tooltip("For skyblue block management.")]
    public GameObject blueBlock;

    /// <summary>
    /// For blue block management.
    /// </summary>
    [Tooltip("For blue block management.")]
    public GameObject blueBlock2;

    /// <summary>
    /// For purple block management.
    /// </summary>
    [Tooltip("For purple block management.")]
    public GameObject purpleBlock;

    #endregion

    #region Audio fields fields
    //-----------------------------------
    // Audio fields
    //-----------------------------------
    ///// <summary>
    ///// Menu screen loop BGM music clip.
    ///// </summary>
    //[Tooltip("Menu screen loop BGM music clip.")]
    //public AudioClip soundMenuLoopBGM;

    /// <summary>
    /// Game screen loop BGM music clip.
    /// </summary>
    [Tooltip("Game screen loop BGM music clip.")]
    public AudioClip soundLoopBGM;

    /// <summary>
    /// Music clip for BGM when the game is cleared on the result screen.
    /// </summary>
    [Tooltip("Music clip for BGM when the game is cleared on the result screen.")]
    public AudioClip soundGameClearBGM;

    /// <summary>
    /// Music clip for BGM when the game is over on the result screen.
    /// </summary>
    [Tooltip("Music clip for BGM when the game is over on the result screen.")]
    public AudioClip soundGameOverBGM;

    ///// <summary>
    ///// Music clip for loop BGM on the result screen.
    ///// </summary>
    //[Tooltip("Music clip for loop BGM on the result screen.")]
    //public AudioClip soundResultLoopBGM;

    /// <summary>
    /// SE sound when destroying a block.
    /// </summary>
    [Tooltip("SE sound when destroying a block.")]
    public AudioClip seBlockBreak;

    /// <summary>
    /// SE sound when touching the lower frame.
    /// </summary>
    [Tooltip("SE sound when touching the lower frame.")]
    public AudioClip sePlayerMiss;

    /// <summary>
    /// SE sound when your ship is destroyed.
    /// </summary>
    [Tooltip("SE sound when your ship is destroyed.")]
    public AudioClip seGameOver;

    #endregion

    #region FlagControl fields
    //-----------------------------------
    // FlagControl fields
    //-----------------------------------

    /// <summary>
    /// Launch ready status judgment.
    /// </summary>
    private bool isReadyToLanch = false; // 発射準備状態判定

    /// <summary>
    /// Game clear judgment.
    /// </summary>
    private bool isGameClear = false;    // ゲームクリア判定

    /// <summary>
    /// Game over judgment.
    /// </summary>
    private bool isGameOver = false;     // ゲームオーバー判定

    /// <summary>
    /// Judgment that score was obtained by breaking the block.
    /// </summary>
    private bool judgeLastCollisionScoreBlock = false;     // ブロックを壊してScoreを得た判定
    #endregion

    //-----------------------------------
    // Properties fields
    //-----------------------------------
    #region Properties
    // 自機ライフ(残機)のプロパティ
    /// <summary>
    /// Property of own machine life (remaining machine).
    /// </summary>
    int Life
    {
        get { return life; }
        set
        {
            life = value;

            if (life != oldLife)
            {
                Text life_text = lifeObject.GetComponent<Text>();
                if (life_text != null)
                    life_text.text = "残機：" + life.ToString();

                oldLife = life;
            }
        }
    }

    // スコアのプロパティ
    /// <summary>
    /// Score properties.
    /// </summary>
    int Score
    {
        get { return score; }
        set
        {
            score = value;

            if (score != oldScore)
            {
                Text score_text = scoreObject.GetComponent<Text>();
                if (score_text != null)
                    score_text.text = "得点：" + score.ToString();

                oldScore = score;
            }
        }
    }

    // 基準時間のプロパティ(経過時間の基準)
    /// <summary>
    /// Reference time property (based on elapsed time).
    /// </summary>
    float BaseTime
    {
        get { return baseTime; }
        set
        {
            baseTime = value;
        }
    }

    // 経過時間のプロパティ
    /// <summary>
    /// Elapsed time property.
    /// </summary>
    float ElapsedTime
    {
        get { return elapsedTime; }
        set
        {
            elapsedTime = value;

            if (elapsedTime != oldElapsedTime)
            {
                Text time_text = timeObject.GetComponent<Text>();
                if (time_text != null)
                    time_text.text = "時間：" + elapsedTime.ToString();

                oldElapsedTime = elapsedTime;
            }
        }
    }

    // 停止時間のプロパティ
    /// <summary>
    /// Downtime property.
    /// </summary>
    float StopTime
    {
        get { return stopTime; }
        set
        {
            stopTime = value;
        }
    }

    // リザルトシーンへ表示するテキスト文字列のプロパティ
    /// <summary>
    /// Properties of the text string to display in the result scene.
    /// </summary>
    string ResultText
    {
        get
        {
            Text gameOverText = gameOverCanvas.transform.Find("Text").gameObject.GetComponent<Text>();

            if (gameOverText != null)
            {
                return gameOverText.text;
            }
            else
            {
                return "";
            }
        }
        set
        {
            result = value;
            Text gameOverText = gameOverCanvas.transform.Find("Text").gameObject.GetComponent<Text>();

            if (gameOverText != null)
            {
                gameOverText.text = result;
            }
        }
    }

    #endregion

    //-----------------------------------
    // MonoBehaviour Methods
    //-----------------------------------
    #region MonoBehaviour Methods

    // Start is called before the first frame update
    // オブジェクト起動時に実行
    /// <summary>
    /// Executed when the object is started.
    /// </summary>
    void Start()
    {
        // 初期化
        Initialize();
        //SetTestBlocks(); // 【全消しTEST用】

        Debug.Log("[GAME START]:ゲーム　スタート！");
    }

    // Update is called once per frame
    // フレーム毎に連続実行
    /// <summary>
    /// Continuous execution for each frame.
    /// </summary>
    void Update()
    {
        // 自機の処理
        ProcessPlayer();

        // ゲームシーンで表示している時間の更新
        UpdateTimer();
    }

    #endregion

    //-----------------------------------
    // public Methods
    //-----------------------------------
    #region public Methods

    // ゲーム読み込み
    /// <summary>
    /// Game loading.
    /// </summary>
    public void LoadGame()
    {
        //StopSound(menuBgmObject); // BGM再生停止
        Debug.Log("Loading game()");
        SceneManager.LoadScene("Game");
    }

    // ゲーム終了
    /// <summary>
    /// Get out of the game.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Exiting game");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // メニュー画面読み込み
    /// <summary>
    /// Menu screen loading.
    /// </summary>
    public void LoadMenu()
    {
        Debug.Log("Loading menu()");

        StopSound(bgmObject); // BGM再生停止

        SceneManager.LoadScene("Title");

    }

    // リザルト画面読み込み
    /// <summary>
    /// Result screen loading.
    /// </summary>
    public void LoadResult()
    {
        Debug.Log("Loading result()");

        StopSound(bgmObject); // BGM再生停止

        SceneManager.LoadScene("Result");
    }

    /// <summary>
    /// Game retry.
    /// </summary>
    public void Retry()
    {
        Debug.Log("[GAME RETRY]:ゲーム　リトライ！");
        SceneManager.LoadScene("Game");
    }

    /// <summary>
    /// Confirm ready to launch.
    /// </summary>
    public bool IsReadyToLanch()
    {
        return isReadyToLanch;
    }

    // 現在の再生時間取得
    /// <summary>
    /// Get the play time of the current clip.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public float GetAudioSourceTime(GameObject obj)
    {
        return GetAudioSourceComponent(obj).time;
    }

    // 現在の曲の最大再生時間の取得
    /// <summary>
    /// Get the maximum play time for the current clip.
    /// </summary>
    /// <param name="soundClip"></param>
    /// <returns></returns>
    public float GetNowPlaySoundMaxTime(AudioClip soundClip)
    {
        return soundClip.length;
    }

    /// <summary>
    /// Processing at the time of mistake.
    /// </summary>
    public void Miss()
    {
        Life--;

        Debug.LogFormat("[GAME PLAY]: 残機={0}", Life);
        if (Life <= 0)
        {
            // ゲームオーバーの場合
            ProcessGameOver();
        }
        else
        {
            PlaySoundClip(Player, sePlayerMiss); // ミス時のSE音

            Debug.Log("[GAME PLAY CONTINIUE]:ゲーム　コンティニュー！");
            // ミスをしただけでゲームオーバーではない場合は
            // ボールを元の位置に戻してゲーム再開
            // Ready to launch
            ReadyTolaunch();
        }
    }

    /// <summary>
    /// Processing at the time of destroying a block.
    /// </summary>
    /// <param name="obj"></param>
    public void BlockBreak(GameObject obj)
    {
        PlaySoundClip(ball.gameObject, seBlockBreak); // ブロック破壊SE音

        obj.SetActive(false);
        Debug.LogFormat("[BlockBreak(GameObject obj)]: obj.tag={0}", obj.tag);

        if (obj.tag == "RED")
            Score += 100;
        if (obj.tag == "ORANGE")
            Score += 70;
        if (obj.tag == "YELLOW")
            Score += 60;
        if (obj.tag == "GREEN")
            Score += 50;
        if (obj.tag == "SKYBLUE")
            Score += 40;
        if (obj.tag == "BLUE")
            Score += 30;
        if (obj.tag == "PERPLE")
            Score += 20;

        judgeLastCollisionScoreBlock = true;　// ブロックを壊してScoreを得た
    }

    /// <summary>
    /// Process that was countered.
    /// </summary>
    public void HitBack()
    {
        // Score processing that was hit back by the own machine after the block was destroyed.
        if (judgeLastCollisionScoreBlock)
        {
            Score += 10; // Add 10 points when the ball bounces.
            judgeLastCollisionScoreBlock = false;
        }

        // CorrectBallSpeed();
    }

    /// <summary>
    /// Game over processing.
    /// </summary>
    public void ProcessGameOver()
    {
        if (!isGameOver)
        {
            GetStopElapsedTime(); //時間停止

            Debug.Log("[GAME OVER]:ゲーム　オーバー！");
            Result("GAME OVER !");

            isGameOver = true;

            // 試しにウエイトを入れてみる
            StartCoroutine(WaitGameOver(7.0f));

            // リザルト画面呼び出し
            LoadResult();
        }
    }

    /// <summary>
    /// Game clear process.
    /// </summary>
    public void ProcessGameClear()
    {
        if (!isGameClear)
        {
            if (blocks.Count(x => x.activeSelf) == 0)
            {
                GetStopElapsedTime(); //時間停止

                // ゲームクリア
                PlaySoundClip(bgmObject, soundGameClearBGM); // BGM設定

                Debug.Log("[END]:ゲームクリア");

                Result("GAME CLEAR !");

                isGameClear = true;

                // リザルト画面呼び出し
                LoadResult();
            }
        }

    }
    /// <summary>
    /// Play SE and BGM music clips.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="soundClip"></param>
    public void PlaySoundClip(GameObject obj, AudioClip soundClip)
    {
        Debug.LogFormat("[PlaySoundClip(GameObject obj, AudioClip soundClip)]: obj={0}, soundClip={1}", obj, soundClip);

        // オーディオソースコンポーネントが取得できる時は再生させる
        // Play when the audio source component is available
        AudioSource audioSource = GetAudioSourceComponent(obj);
        if (audioSource != null)
        {
            // 10秒までの短い音源はSEとして1回しか再生しない、10秒以上の長い音源はBGMとして再生する
            // Short sound sources up to 10 seconds are played only once as SE, and long sound sources longer than 10 seconds are played as BGM.
            if (Math.Round(GetNowPlaySoundMaxTime(soundClip), 0, MidpointRounding.ToEven) >= 10f)
            {
                audioSource.clip = soundClip;
                audioSource.Play();
            }
            else
            {
                audioSource.PlayOneShot(soundClip);
            }
        }

    }

    // スコア稼ぎ対策の10減点処理(「上の枠と打ち返す」ループ状態になるとスコアがどんどん増えていくので)
    /// <summary>
    /// 10 deduction processing for score earning measures.
    /// (Because the score will increase steadily in the loop state of "hit back with the upper frame")
    /// </summary>
    public void ProcessWallTopBack()
    {
        // ブロックを壊わしてない場合は処理しなくていい
        // If you haven't broken the block, you don't have to deal with it.
        if (!judgeLastCollisionScoreBlock)
        {
            Score -= 10;
        }
    }

    ///// <summary>
    ///// Initialize the result scene. (※Remnants of trying to do everything with GameManager.)
    ///// </summary>
    //public void InitializeResult()
    //{
    //    Debug.Log("Loading result");

    //    PlaySoundClip(resultBgmObject, soundResultLoopBGM); // BGM設定
    //}

    ///// <summary>
    ///// Initialize the menu scene (※Remnants of trying to do everything with GameManager.)
    ///// </summary>
    //public void InitializeMenu()
    //{
    //    Debug.Log("Loading menu");

    //    PlaySoundClip(menuBgmObject, soundMenuLoopBGM); // BGM設定
    //}

    // ゲームシーン上の時間のTimespanを取得
    /// <summary>
    /// Get Timespan of time on the game scene.
    /// </summary>
    public TimeSpan GetResultTimeSpan()
    {
        if (GameManager.timeAttack <= 0f)
            GameManager.timeAttack = 0f;

        int resultTimeSec = (int)GameManager.timeAttack;

        return new System.TimeSpan(0, 0, 0, 0, resultTimeSec * 1000);

    }

    #endregion

    //-----------------------------------
    // private Methods
    //-----------------------------------
    #region private Methods
    //-----------------------------------
    // 初期化処理
    //-----------------------------------
    /// <summary>
    /// Initialization process.
    /// </summary>
    private void Initialize()
    {
        Debug.LogFormat("[Start()]: BGMループ単位の再生時間（丸めた値）={0}", Math.Round(GetNowPlaySoundMaxTime(soundLoopBGM), 0, MidpointRounding.ToEven));
        Debug.LogFormat("[Start()]: BGMループ単位の再生時間={0}", GetNowPlaySoundMaxTime(soundLoopBGM));
        Debug.LogFormat("[Start()]: BGMがループするかどうか={0}", GetAudioSourceComponent(bgmObject).loop);

        PlaySoundClip(bgmObject, soundLoopBGM); // ゲームシーンのループBGM設定

        // 最初はゲームオーバー表示を非表示にする
        gameOverCanvas.SetActive(false);

        // 残機を設定
        Life = lifeMax;
        Debug.LogFormat("[Start()]: 残機={0}", Life);

        // スコアを設定
        Score = 0;
        Debug.LogFormat("[Start()]: スコア={0}", Score);

        // Ready to launch
        ReadyTolaunch();

        SetBlocks();
    }

    // ボール　発射準備態勢処理
    /// <summary>
    /// Ball launch preparation processing.
    /// </summary>
    private void ReadyTolaunch()
    {
        Debug.Log("[ReadyTolaunch()]:ボール　発射準備！");

        isReadyToLanch = true;

        // ボールの位置をプレイヤー位置へ変更
        ResetBallPosition();
    }

    // ボールの位置変更
    /// <summary>
    /// Change the position of the ball
    /// </summary>
    private void ResetBallPosition()
    {
        // ボールの位置をプレイヤー位置へ変更
        ball.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, Player.transform.position.z + 0.5f);
    }

    // リザルトのメッセージUI表示
    /// <summary>
    /// Result message UI display.
    /// </summary>
    /// <param name="message"></param>
    private void Result(string message)
    {
        Destroy(ball.gameObject); //ボールを消す
        ResultText = message;     // 表示内容をmessageの文字列に書き換え
        gameOverCanvas.SetActive(true); // リザルトのメッセージUIのキャンバス表示
    }

    // 終了待ち処理
    /// <summary>
    /// End waiting process when the game is over.
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    private IEnumerator WaitGameOver(float seconds)
    {
        StopSound(bgmObject); // BGM再生停止
        PlaySoundClip(Player, seGameOver); // 自機消滅SE音
        yield return new WaitForSeconds(seconds);
    }
    ///// <summary>
    ///// End waiting process when the game is over. (Unused)
    ///// </summary>
    ///// <returns></returns>
    //private IEnumerator PreprocessWaitGameOver()
    //{
    //    StopSound(bgmObject); // BGM再生停止
    //    PlaySoundClip(Player, seGameOver); // 自機消滅SE音
    //    yield return null;
    //}

    // ボールの制御用の関数(未使用)
    /// <summary>
    /// Function for controlling the ball.(unused)
    /// </summary>
    void CorrectBallSpeed()
    {
        // これで速度はわかる。あとは各自で工夫してください・・・
        Rigidbody rigidbody = ball.GetComponent<Rigidbody>();
        float vx = rigidbody.velocity.x;
        float vz = rigidbody.velocity.z;

        Debug.LogFormat("[CorrectBallSpeed()]: vx={0}", vx);
        Debug.LogFormat("[CorrectBallSpeed()]: vz={0}", vz);

    }

    // ブロックの整列
    /// <summary>
    /// Installation of aligned blocks(Alignment of blocks.)
    /// </summary>
    void SetBlocks()
    {
        GameObject block = redBlock;

        for (int i = 0; i < 7; i++)
        {
            if (i == 1)
                block = orangeBlock;
            if (i == 2)
                block = yellowBlock;
            if (i == 3)
                block = greenBlock;
            if (i == 4)
                block = blueBlock;
            if (i == 5)
                block = blueBlock2;
            if (i == 6)
                block = purpleBlock;

            Instantiate(block, new Vector3(-7, 1, 11 - i), Quaternion.identity);

            for (int j = -15; j <= 15; j += 2)
                blocks.Add(Instantiate(block, new Vector3(j, 1, 11 - i), Quaternion.identity));
        }

        // 配置したブロックはblocksのリスト内にいる（上記2重Forループで処理した）。生成していた各色ブロックはいらないので消す
        Destroy(redBlock);
        Destroy(orangeBlock);
        Destroy(yellowBlock);
        Destroy(greenBlock);
        Destroy(blueBlock);
        Destroy(blueBlock2);
        Destroy(purpleBlock);
    }

    // 自機の操作
    /// <summary>
    /// Operation of own machine.
    /// </summary>
    void MovePlayer()
    {
        // 枠の外へ移動しないようにする
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (Player.transform.position.x > -14)
                Player.transform.position += Vector3.left * this.playerSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (Player.transform.position.x < 14)
                Player.transform.position += Vector3.right * this.playerSpeed * Time.deltaTime;
        }

        // 上下に移動しすぎないように調整する
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (Player.transform.position.z < -10)
                Player.transform.position += Vector3.forward * this.playerSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (Player.transform.position.z > -14)
                Player.transform.position += Vector3.back * this.playerSpeed * Time.deltaTime;
        }
    }

    // 自機の処理
    /// <summary>
    /// Processing of own machine.
    /// </summary>
    void ProcessPlayer()
    {
        MovePlayer();

        // 発射準備状態判定
        if (isReadyToLanch)
        {
            ResetBallPosition();

            // スペースキーを押すとプッシュする
            if (Input.GetKey(KeyCode.Space))
            {
                Debug.Log("[ProcessPlayer()]:ボール　発射！");

                GetBaseTime();
                ball.BallStart();
                isReadyToLanch = false;
                Debug.LogFormat("[ProcessPlayer()]: this.playerSpeed={0}", this.playerSpeed);
                Debug.LogFormat("[ProcessPlayer()]: ball.Speed={0}", ball.Speed);
            }
        }
    }

    // 基準時間取得
    /// <summary>
    /// Get reference time.
    /// </summary>
    private void GetBaseTime()
    {
        // 初回発射タイミングで経過の基準時間とする
        if (IsFirstFire())
        {
            BaseTime = Time.realtimeSinceStartup;
        }
    }

    // 初回発射判定
    /// <summary>
    /// First launch judgment.
    /// </summary>
    private bool IsFirstFire()
    {
        return lifeMax == Life;
    }

    // ゲームシーンで表示している時間の更新
    /// <summary>
    /// Update the time displayed in the game scene.
    /// </summary>
    private void UpdateTimer()
    {
        if (BaseTime == -1f)
            return;

        float now = -1f;
        if (isGameOver || isGameClear )
        {
            now = StopTime;
        }
        else
        {
            now = Time.realtimeSinceStartup;
        }
        ElapsedTime = (float)Math.Round((now - BaseTime), 0, MidpointRounding.AwayFromZero);
    }

    // 経過時間の取得
    /// <summary>
    /// Get elapsed time.
    /// </summary>
    private void GetElapsedTime()
    {
        float elapsed_time = Time.realtimeSinceStartup - baseTime;
    }

    // 停止時の経過時間取得
    /// <summary>
    /// Get the elapsed time when stopped.
    /// </summary>
    private void GetStopElapsedTime()
    {
        if (StopTime == -1f) StopTime = Time.realtimeSinceStartup;
        ElapsedTime = StopTime - BaseTime;
        timeAttack = ElapsedTime;
    }

    // AudioSourceコンポーネントを入手する
    /// <summary>
    /// Get the AudioSource component.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    private static AudioSource GetAudioSourceComponent(GameObject obj)
    {
        // ゲームオブジェクトにオーディオソースコンポーネントがあるていで再生させる
        // 
        return obj == null ? null : ((AudioSource)obj.GetComponent<AudioSource>());
    }

    // 再生しているミュージッククリップを停止する。
    /// <summary>
    /// Stop playing music clip.
    /// </summary>
    /// <param name="obj"></param>
    private void StopSound(GameObject obj)
    {
        Debug.LogFormat("[StopSound(GameObject obj)]: obj={0}", obj);

        // オーディオソースコンポーネントが取得できる時は停止させる
        AudioSource audioSource = GetAudioSourceComponent(obj);
        if (audioSource != null)
        {
            // 再生音源を止める
            audioSource.Stop();
        }

    }

    // 【全消しTEST用】最後の１つを残して、ブロック全部消す
    /// <summary>
    /// [For the "All Erase TEST"] Erase all blocks, leaving the last one.
    /// </summary>
    private void SetTestBlocks()
    {
        GameObject goLast = null;
        foreach (GameObject go in blocks)
        {
            goLast = go;
            go.SetActive(false);
        }

        // 最後だけ
        if (null != goLast)
        {
            goLast.SetActive(true);
        }
    }

    #endregion
}
