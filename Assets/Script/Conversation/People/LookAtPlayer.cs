using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    [SerializeField] private Transform _target; // The target to look at

    [SerializeField] private float _rotationDuration;
    [SerializeField] private Animator _animator;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Vector3 _modelRotationOffset;
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private float _xOffset;
    [SerializeField] private float _yOffset;
    [SerializeField] private float _zOffset;
    private Quaternion _currentRotation;
    private Coroutine _rotationCoroutine;
    private Quaternion _startRotation;
    public bool IsActive { get; private set; }

    void Start()
    {
        _startRotation = transform.rotation;
        _mainCamera = Camera.main;
    }

    public void ChangeTarget(Transform nextTarget)
    {
        _target = nextTarget;
    }

    public void UpdateRotation()
    {
        _startRotation = transform.rotation;
    }

    void LateUpdate()
    {
        if (IsActive)
        {
            _animator.speed = 0f;
            transform.rotation = _currentRotation;
        }
    }

    public void StopRotation()
    {
        if (_rotationCoroutine != null)
        {
            StopCoroutine(_rotationCoroutine);
            _rotationCoroutine = null;
        }
        _rotationCoroutine = StartCoroutine(RotateDefaultCoroutine());
    }

    public void RotateTowardsTarget()
    {

        if (_rotationCoroutine != null)
        {
            StopCoroutine(_rotationCoroutine);
            _rotationCoroutine = null;
        }

        _rotationCoroutine = StartCoroutine(RotateTowardsCoroutine());
    }

    public void RotateTowardsTargetInstant()
    {

        Vector3 directionToTarget = _target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);

        lookRotation.eulerAngles = new Vector3(
            lookRotation.eulerAngles.x,
            lookRotation.eulerAngles.y + _modelRotationOffset.y,
            lookRotation.z + _modelRotationOffset.z);

        transform.rotation = lookRotation;
    }

    private IEnumerator RotateTowardsCoroutine()
    {
        float elapsed = 0;


        Vector3 directionToTarget = _target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);

        lookRotation.eulerAngles = new Vector3(
            lookRotation.eulerAngles.x,
            lookRotation.eulerAngles.y + _modelRotationOffset.y,
            lookRotation.z + _modelRotationOffset.z);

        _currentRotation = Quaternion.Lerp(transform.rotation, lookRotation, _animationCurve.Evaluate(elapsed / _rotationDuration));
        IsActive = true;

        while (elapsed < _rotationDuration)
        {
            elapsed += Time.deltaTime;
            _currentRotation = Quaternion.Lerp(transform.rotation, lookRotation, elapsed / _rotationDuration);

            if (elapsed >= 1.49f)
            {
                _currentRotation = lookRotation;

                StopCoroutine(_rotationCoroutine);
                _rotationCoroutine = null;
            }
            yield return null;
        }
        _currentRotation = lookRotation;

        _rotationCoroutine = null;
    }

    public void LookOnCharacterConstant()
    {
        StartCoroutine(LookAtPlayerConstant());
    }

    public void StopLookingOnCharacter()
    {
        IsActive = false;
        StopAllCoroutines();
    }

    private IEnumerator LookAtPlayerConstant()
    {
        IsActive = true;
        while (true)
        {
            Vector3 directionToTarget = _target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);

            lookRotation.eulerAngles = new Vector3(
            lookRotation.eulerAngles.x,
            lookRotation.eulerAngles.y + _modelRotationOffset.y,
            lookRotation.z + _modelRotationOffset.z);
            _currentRotation = lookRotation;
            transform.rotation = _currentRotation;

            yield return new WaitForSeconds(0.03f);
        }

    }


    private IEnumerator RotateDefaultCoroutine()
    {
        float elapsed = 0;

        while (elapsed < _rotationDuration)
        {
            elapsed += Time.deltaTime;
            _currentRotation = Quaternion.Lerp(transform.rotation, _startRotation, _animationCurve.Evaluate(elapsed / _rotationDuration));
            if (elapsed >= 1.49f)
            {
                _currentRotation = _startRotation;
                IsActive = false;
                _animator.speed = 1f;
                StopCoroutine(_rotationCoroutine);
                _rotationCoroutine = null;
            }
            yield return null;
        }
        _currentRotation = _startRotation;

        _rotationCoroutine = null;

        IsActive = false;

    }
}
