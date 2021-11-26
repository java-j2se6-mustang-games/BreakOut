using UnityEngine;

/// <summary>
/// TitleManager：Title Scene Cotroller
/// </summary>
public class TitleManager : MonoBehaviour
{
    [Header("Title Control")]
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
    /// BGM Controller in the menu scene. 
    /// </summary>
    [Tooltip("BGM Controller in the menu scene.")]
    public GameObject menuBgmObject;

    /// <summary>
    /// Bgm to be sounded in the menu scene. 
    /// </summary>
    [Tooltip("Bgm to be sounded in the menu scene.")]
    public AudioClip soundMenuLoopBGM;
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
        //gm.LoadMenu();
        InitializeMenu();
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

    /// <summary>
    /// Start the game.
    /// </summary>
    public void StartGame()
    {
        Debug.Log("StartGame()");

        gm.LoadGame();
    }
    /// <summary>
    /// Quit the Game
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("QuitGame()");

        gm.QuitGame();
    }

    /// <summary>
    /// Initialize the title scene.
    /// </summary>
    private void InitializeMenu()
    {
        Debug.Log("InitializeMenu()");

        gm.PlaySoundClip(menuBgmObject, soundMenuLoopBGM); // タイトルシーンのループBGM設定
    }

}
