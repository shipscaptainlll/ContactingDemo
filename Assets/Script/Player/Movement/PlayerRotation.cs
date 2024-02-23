using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    [Header("Setups")]
    [SerializeField] private Transform _bodyTransform;
    [SerializeField] private PlayerBlocker _playerBlocker;

    [Header("Settings")]
    [SerializeField] private float _mouseSensitivity;
    [SerializeField] private float _maxVerticalAngle;
    [SerializeField] private float _minVerticalAngle;
    private float _yRotation;
    private float _xRotation;

    private bool _isSitting;
    private bool _isSleeping;
    private float _sitPivot;

    private void Start()
    {

    }

    void Update()
    {
        if (_playerBlocker.MovementBlocked)
        {
            return;
        }

        RotateHead();
    }

    public void PivosSittings()
    {
        _sitPivot = _xRotation;
        _isSitting = true;
        PlayerSatRotation();
    }

    public void UnpivotSitting()
    {
        _isSitting = false;
    }

    public void PlayerSatRotation()
    {
        _yRotation = 0;
        transform.localRotation = Quaternion.Euler(_yRotation, 0, 0f);
    }

    private void RotateHead()
    {
        float xRot = InputManager.Instance.InputHandler.Player.MouseX.ReadValue<float>() 
            * _mouseSensitivity * 1f;
        float yRot = InputManager.Instance.InputHandler.Player.MouseY.ReadValue<float>() 
            * _mouseSensitivity * 5f;

        _yRotation -= yRot * 0.0006f;
        _xRotation -= xRot * 0.0005f;

        _yRotation = Mathf.Clamp(_yRotation, _maxVerticalAngle, _minVerticalAngle);

        transform.localRotation = Quaternion.Euler(_yRotation, 0, 0f);

        if ((!_isSitting && !_isSleeping) || (_isSitting && Mathf.Abs(_xRotation - _sitPivot) < 11.5f)
            || (_isSleeping && Mathf.Abs(_xRotation - _sitPivot) < 10))
        {
            _bodyTransform.Rotate(Vector3.up * xRot * 0.0055f);
        }
        else if ((_isSitting && Mathf.Abs(_xRotation - _sitPivot) > 11.5f))
        {
            if ((_xRotation - _sitPivot) < -11.5f)
            {
                _xRotation = _sitPivot - 11.5f;
            }
            else if ((_xRotation - _sitPivot) > 11.5f)
            {
                _xRotation = _sitPivot + 11.5f;
            }
        }
        else if ((_isSleeping && Mathf.Abs(_xRotation - _sitPivot) > 10))
        {
            if ((_xRotation - _sitPivot) < -10)
            {
                _xRotation = _sitPivot - 10;
            }
            else if ((_xRotation - _sitPivot) > 10)
            {
                _xRotation = _sitPivot + 10;
            }
        }
    }
}
