using UnityEngine;
using Bolt;

public class PlayerController : EntityBehaviour<IPhysicsState>
{
    private PlayerMotor _playerMotor;
    private PlayerWeapons _playerWeapons;
    // basic movement.
    private bool _forward;
    private bool _backward;
    private bool _left;
    private bool _right;
    private float _yaw;
    private float _pitch;
    private bool _jump;

    // shooting
    private bool _fire;
    private bool _aiming;
    private bool _reload;

    private bool _hasControl = false;

    private float _mouseSensitivity = 5f;

    public void Awake()
    {
        _playerMotor = GetComponent<PlayerMotor>();
        _playerWeapons = GetComponent<PlayerWeapons>();
    }

    public override void Attached()
    {
        if (entity.HasControl)
        {
            _hasControl = true;
            GUI_Controller.current.ShowHealth(true);
        }

        Init(entity.HasControl);
        _playerMotor.Init(entity.HasControl);
        _playerWeapons.Init();
    }

    public void Init(bool isMine)
    {
        if (isMine)
        {
            Cursor.lockState = CursorLockMode.Locked;
            FindObjectOfType<PlayerSetupController>().SceneCamera.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (_hasControl)
            PollKeys();
    }

    private void PollKeys()
    {
        _forward = Input.GetKey(KeyCode.W);
        _backward = Input.GetKey(KeyCode.S);
        _left = Input.GetKey(KeyCode.A);
        _right = Input.GetKey(KeyCode.D);
        _jump = Input.GetKeyDown(KeyCode.Space);        // getkey

        _fire = Input.GetMouseButton(0);

        _aiming = Input.GetMouseButton(1);
        _reload = Input.GetKeyDown(KeyCode.R);


        _yaw += Input.GetAxisRaw("Mouse X") * _mouseSensitivity;
        _yaw %= 360f;
        _pitch += -Input.GetAxisRaw("Mouse Y") * _mouseSensitivity;
        _pitch = Mathf.Clamp(_pitch, -85, 85);
    }

    public override void SimulateController()       // send input to server.
    {
        IPlayerCommandInput input = PlayerCommand.Create();
        input.Forward = _forward;
        input.Backward = _backward;
        input.Right = _right;
        input.Left = _left;
        input.Yaw = _yaw;
        input.Pitch = _pitch;
        input.Jump = _jump;

        input.Fire = _fire;
        input.Aiming = _aiming;
        input.Reload = _reload;

        entity.QueueInput(input);

        _playerMotor.ExecuteCommand(_forward, _backward, _left, _right, _jump, _yaw, _pitch);
        _playerWeapons.ExecuteCommand(_fire, _aiming, _reload, BoltNetwork.ServerFrame % 1024);
    }


    public override void ExecuteCommand(Command command, bool resetState)
    {
        PlayerCommand cmd = (PlayerCommand)command;

        if (resetState)
        {
            _playerMotor.SetState(cmd.Result.Position, cmd.Result.Rotation);
        }
        else
        {
            PlayerMotor.State motorState = new PlayerMotor.State();
            if (!entity.HasControl)
            {
                motorState = _playerMotor.ExecuteCommand(
                cmd.Input.Forward,
                cmd.Input.Backward,
                cmd.Input.Left,
                cmd.Input.Right,
                cmd.Input.Jump,
                cmd.Input.Yaw,
                cmd.Input.Pitch);

                _playerWeapons.ExecuteCommand(
                cmd.Input.Fire,
                cmd.Input.Aiming,
                cmd.Input.Reload,
                cmd.ServerFrame % 1024
                );
            }

            cmd.Result.Position = motorState.position;
            cmd.Result.Rotation = motorState.rotation;
        }
    }
}

