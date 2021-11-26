using UnityEngine;

/// <summary>
/// Ball controller for the game scene.
/// </summary>
public class Ball : MonoBehaviour
{
    /// <summary>
    /// Ballspeed apply in the game scene.
    /// </summary>
    [Tooltip("Ballspeed apply in the game scene.")]
    public float Speed = 1.0f;

    /// <summary>
    /// Ball speed control in the game scene.
    /// </summary>
    private Rigidbody myRigidbody;

    /// <summary>
    /// Game Manager in the game scene. 
    /// </summary>
    [Tooltip("Game Manager in the game scene. ")]
    public GameManager GameManager;

    // 前回接触判定保持用
    /// <summary>
    /// For holding the previous contact judgment
    /// </summary>
    private string targetBeforeRigiTag;

    /// <summary>
    /// For holding the previous contact judgment
    /// </summary>
    public string TargetBeforeRigitTag
    {
        get { return targetBeforeRigiTag; }
        set
        {
            targetBeforeRigiTag = value;
        }
    }

    // Start is called before the first frame update
    /// <summary>
    /// Executed when the object is started.
    /// </summary>
    void Start()
    {
    }

    // ボールを転がす(ボール発射時のアクション処理)
    /// <summary>
    /// Roll the ball (action processing when launching the ball).
    /// </summary>
    public void BallStart()
    {
        //gameObject.transform.position = new Vector3(2, 1, -4);
        gameObject.transform.position = GameManager.ball.transform.position;
        myRigidbody = this.GetComponent<Rigidbody>();
        myRigidbody.velocity = Vector3.zero;
        myRigidbody.AddForce((transform.forward + transform.right) * Speed, ForceMode.VelocityChange); // 右上方向に力を加える
    }

    // Update is called once per frame
    // フレーム毎に連続実行
    /// <summary>
    /// Continuous execution for each frame.
    /// </summary>
    void Update()
    {
    }

    // コリジョン接触イベント
    /// <summary>
    /// Collision contact event.
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        // 発射準備状態の判定(ボールが動いてないときは、接触判定取らなくていい)
        bool isReadyState = GameManager.IsReadyToLanch();
        if (isReadyState) 
            return;

        string tag = collision.gameObject.tag;

        // ボールがなにかに当たったらtagからなにに当たったかを調べて適切な処理をする
        // ケース：下枠（ミス）
        if (tag == "MISS")
        {
            Debug.LogFormat("[OnCollisionEnter(Collision collision)]: this.playerSpeed={0}", GameManager.playerSpeed);
            Debug.LogFormat("[OnCollisionEnter(Collision collision)]: ball.Speed={0}", Speed);

            GameManager.Miss(); // 後述
        }
        // ケース：上枠
        if (tag == "NOREFLECTIONSCORE")
        {
            Debug.LogFormat("[OnCollisionEnter(Collision collision)]: TargetBeforeRigitTag={0}", TargetBeforeRigitTag);
            // プレイヤー⇔上の枠のみの接触判定
            if ("PLAYER" == TargetBeforeRigitTag)
            {
                //  「上の枠と打ち返す」ループ状態ではペナルティとして10点減点していく
                GameManager.ProcessWallTopBack();
            }
        }
        // ケース：プレイヤー
        if (tag == "PLAYER")
        {
            GameManager.HitBack(); // ボールを跳ね返したときも10点追加する

            // だんだんボールの速度をアップする
            myRigidbody.AddForce(transform.forward * 0.5f, ForceMode.VelocityChange);


        }

        // ケース：ブロック
        bool b = (tag == "RED"
               || tag == "ORANGE"
               || tag == "YELLOW"
               || tag == "GREEN"
               || tag == "SKYBLUE"
               || tag == "BLUE"
               || tag == "PERPLE"); //スペル間違い

        if (b)
        {
            GameManager.BlockBreak(collision.gameObject); // ブロック破壊処理

            GameManager.ProcessGameClear();               // ゲームクリア判定
        }

        // 前回接触判定対象TAGの格納
        TargetBeforeRigitTag = tag;
    }
}
