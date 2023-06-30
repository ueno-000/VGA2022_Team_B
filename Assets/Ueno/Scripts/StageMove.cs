using UnityEngine;
using UnityEngine.InputSystem;

public class StageMove : MonoBehaviour
{
    [Header("Pram")]
    [Tooltip("移動速度"),SerializeField] private float _moveSpeed = 5f;
    public float MoveSpeed  
        { get { return _moveSpeed; } set { _moveSpeed = value; } }

    [SerializeField] Obon _obon;

    /// <summary>停止する時にSpeedの値を取っておく</summary>
    [HideInInspector] public float KeepSpeed;

    private Material _targetMaterial;

    private StageTypeChange _stageTypeChange;

    /// <summary>UVスクロール速度が速いので調整の為</summary>
    [Tooltip("速度調整"), SerializeField] private float _speedRatio = 0.1f;

    public float SpeedRatio => _speedRatio;

    private Vector2 offset;

    private bool isSetMaterial;

    [SerializeField] float _gyroSpeed = 1.2f;
    void Start()
    {
        _stageTypeChange = this.gameObject.GetComponent<StageTypeChange>();
        KeepSpeed = MoveSpeed;
        MoveSpeed = 0;
        _obon = _obon.gameObject.GetComponent<Obon>();
        isSetMaterial = false;
    }

    void Update()
    {
        if (!isSetMaterial)
        {
            _targetMaterial = _stageTypeChange.CurrentMaterial;
            offset = _targetMaterial.mainTextureOffset;
            isSetMaterial = true;
        }


        if (!GameManager.IsAppearClearObj)
        {
            StickMove();

            offset.x += MoveSpeed * SpeedRatio * Time.deltaTime;
            //Debug.Log($"指定offset{offset.x}:Materialoffset{_targetMaterial.mainTextureOffset}");
            _targetMaterial.mainTextureOffset = offset;
        }

        else
        {
            MoveSpeed = 0;
        }

    }
    private void StickMove()
    {
        // 現在のゲームパッド情報
        var current = Gamepad.current;

        // ゲームパッド接続チェック
        if (current == null)
            return;

        // 左スティック入力取得
        var leftStickValue = current.leftStick.ReadValue();

        //左には動かない
        if (leftStickValue.x < 0)
        {
            return;
        }
        else if (leftStickValue.x > 0f)
        {
            GameManager.IsStop = false;
            //transform.Translate(-(leftStickValue.x * MoveSpeed * Time.deltaTime), 0, 0);
            MoveSpeed = KeepSpeed;

        }
        else if (leftStickValue.x == 0)
        {
            GameManager.IsStop = true;
            //AudioManager.Instance.CriAtomPlay(CueSheet.SE, "SE_player_footsteps1");
            MoveSpeed = 0;
        }

        //歩いたら傾いてる方に傾くようにした
        if(_obon.Movement >= 0)
        {
            _obon.MisalignmentOfSweetsCausedByMovement(leftStickValue.x * _gyroSpeed);
        }
        else if(_obon.Movement < 0)
        {
            _obon.MisalignmentOfSweetsCausedByMovement(-leftStickValue.x * _gyroSpeed);
        }
    }

    public void OnTapAdvance()
    {
        MoveSpeed = KeepSpeed;
    }
    public void ExitTapAdvance()
    {
        MoveSpeed = 0;
    }

}
