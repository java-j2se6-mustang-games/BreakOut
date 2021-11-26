using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// ResultManager：Result Scene Cotroller
/// </summary>
public class ResultManager : MonoBehaviour
{
    [Header("Result Control")]
    //-----------------------------------
    // GameUIControl fields
    //-----------------------------------
    #region GameUIControl fields
    /// <summary>
    /// Game Manager in the game scene. 
    /// </summary>
    [Tooltip("Game Manager in the game scene. ")]
    public GameManager gm;

    /// <summary>
    /// Game Over Canvas in the result scene. 
    /// </summary>
    [Tooltip("Game Over Canvas in the result scene.")]
    public GameObject gameOverCanvas;

    /// <summary>
    /// Result Scene BGM Controller
    /// </summary>
    [Tooltip("BGM Controller in the result scene.")]
    public GameObject resultBgmObject;

    /// <summary>
    /// Result Scene BGM Audio Clip
    /// </summary>
    [Tooltip("Bgm to be sounded in the result scene.")]
    public AudioClip soundResultLoopBGM;
    #endregion

    //-----------------------------------
    // Control fields
    //-----------------------------------
    #region Control fields
    /// <summary>
    /// Text to be displayed in the result scene.
    /// </summary>
    string result = "";
    #endregion

    //-----------------------------------
    // Properties fields
    //-----------------------------------
    #region Properties

    /// <summary>
    /// Result Scene Display Text Properties
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
        InitializeResult();
    }

    // Update is called once per frame
    // フレーム毎に連続実行
    /// <summary>
    /// Continuous execution for each frame.
    /// </summary>
    void Update()
    {
        
    }
    #endregion


    //-----------------------------------
    // Public Methods
    //-----------------------------------
    #region Public Methods

    /// <summary>
    /// Restart Game
    /// </summary>
    public void Retry()
    {
        Debug.Log("Retry()");

        gm.Retry();
    }
    /// <summary>
    /// Back to menu
    /// </summary>
    public void Exit()
    {
        Debug.Log("Exit()");

        gm.LoadMenu();
    }
    #endregion

    //-----------------------------------
    // Private Methods
    //-----------------------------------
    #region Private Methods
    /// <summary>
    /// Initialize result scene
    /// </summary>
    private void InitializeResult()
    {
        Debug.Log("Loading result");

        TimeSpan resultTimeScore = gm.GetResultTimeSpan(); // 終了時の経過時間（秒）取得

        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(GameManager.score, 0);

        LoadPlaySoundClip();
    }

    private void LoadPlaySoundClip()
    {
        gm.PlaySoundClip(resultBgmObject, soundResultLoopBGM); // リザルトシーンのループBGM設定
    }

    #endregion
    }
