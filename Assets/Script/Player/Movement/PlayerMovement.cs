using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Setups")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private PlayerBlocker _playerBlocker;
    [SerializeField] private Transform _checkSphere;
    [SerializeField] private float _groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private Animator _animator;

    [Header("Settings")]
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _crouchSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _jumpHeight = 2f;

    private const float GRAVITY = -9.8f;

    private float _zInput;
    private float _xInput;
    private Vector3 _verticalVelocity;
    private Vector3 _move;

    public bool IsCrouching { get; private set; }
    private bool _isRunning;
    private bool _isGrounded;

    private List<AudioSource> _woodAudios = new List<AudioSource>();
    private List<AudioSource> _carpetAudios = new List<AudioSource>();
    private List<AudioSource> _concreteAudios = new List<AudioSource>();
    private List<AudioSource> _grassAudios = new List<AudioSource>();

    void Awake()
    {
        _verticalVelocity.y = GRAVITY;

        InputManager.Instance.InputHandler.Player.Crouch.started += (callback) => { StartCrouching(); };
        InputManager.Instance.InputHandler.Player.Crouch.canceled += (callback) => { StopCrouching(); };

        InputManager.Instance.InputHandler.Player.Run.started += (callback) => { StartRunning(); };
        InputManager.Instance.InputHandler.Player.Run.canceled += (callback) => { StopRunning(); };
    }

    private void Start()
    {
        _animator.speed = 1.3f;
    }

    void Update()
    {
        if (_playerBlocker.MovementBlocked)
        {
            _animator.Play("Walk", 0, 0);
            return;
        }

        ImpactMovementByInputs();
        CheckIfGrounded();
        ImpactByGravity();
    }

    private void StartCrouching()
    {
        if (!_isRunning)
        {
            IsCrouching = true;
            _animator.Play("Crouch", 0, 0);
            _movementSpeed = _crouchSpeed;
        }
    }

    private void StopCrouching()
    {
        if (IsCrouching)
        {
            IsCrouching = false;
            _animator.Play("Walk", 0, 0);
            _movementSpeed = _walkSpeed;
        }
    }

    private void StartRunning()
    {
        if (!IsCrouching)
        {
            _animator.speed = 2;
            _isRunning = true;
            _movementSpeed = _runSpeed;
        }
    }

    private void StopRunning()
    {
        if (_isRunning)
        {
            _animator.speed = 1.3f;
            _isRunning = false;
            _movementSpeed = _walkSpeed;
        }
    }

    private void ImpactByGravity()
    {
        _characterController.Move(_verticalVelocity * Time.deltaTime);
    }

    private void CheckIfGrounded()
    {
        if (Physics.CheckSphere(_checkSphere.position, _groundCheckRadius, _groundLayerMask))
        {
            _isGrounded = true;
            if (_verticalVelocity.y < GRAVITY)
            {
                _verticalVelocity.y = GRAVITY;
            }
        }
        else
        {
            _isGrounded = false;
            _verticalVelocity.y += 2f * GRAVITY * Time.deltaTime;
        }
    }

    private void ImpactMovementByInputs()
    {
        _zInput = InputManager.Instance.InputHandler.Player.Forward.IsPressed() ? 1 : 0;
        _zInput = InputManager.Instance.InputHandler.Player.Backward.IsPressed() ? _zInput - 1 : _zInput;

        _xInput = InputManager.Instance.InputHandler.Player.Right.IsPressed() ? 1 : 0;
        _xInput = InputManager.Instance.InputHandler.Player.Left.IsPressed() ? _xInput - 1 : _xInput;

        _move = transform.right * _xInput * _movementSpeed + transform.forward * _zInput * _movementSpeed;

        if (_zInput == 0 && _xInput == 0)
        {
            if (!IsCrouching)
            {
                _animator.Play("Walk", 0, 0);
            }
        }

        _characterController.Move(_move * Time.deltaTime);
    }

    public void CheckUnderGround()
    {
        RaycastHit hit;

        if (Physics.Raycast(_checkSphere.position, Vector3.down, out hit, 5, _groundLayerMask))
        {
            _isGrounded = true;
            string layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);

            switch (layerName)
            {
                case "Wood":
                    _woodAudios[Random.Range(0, _woodAudios.Count)].Play();
                    break;
                case "Carpet":
                    _carpetAudios[Random.Range(0, _carpetAudios.Count)].Play();
                    break;
                case "Concrete":
                    _concreteAudios[Random.Range(0, _concreteAudios.Count)].Play();
                    break;
                case "Grass":
                    _grassAudios[Random.Range(0, _grassAudios.Count)].Play();
                    break;
            }
        }
    }
}
