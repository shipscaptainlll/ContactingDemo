using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtConversation : MonoBehaviour
{
    [Header("Setups")]
    [SerializeField] private Camera _camera;

    [Header("Settings")]
    [SerializeField] private float _lookAtDuration = 1f;
    [SerializeField] private float _zoomInFOV = 30f;
    [SerializeField] private float _zoomOutFOV = 50f;
    [SerializeField] private ConversationManager _conversationManager;
    private Coroutine _zoomingCoroutine;

    public Transform bodyTransform;
    public Transform cameraTransform;


    private Coroutine _rotationCoroutine;


    public void StartRotation()
    {
        if (_conversationManager.CurrentLookAtCharacter != null)
        {
            if (_rotationCoroutine != null)
            {
                StopCoroutine(_rotationCoroutine);
            }
            _rotationCoroutine = StartCoroutine(RotateTowardsTarget());
        }
    }

    public void StartRotation(Transform target)
    {
        if (_conversationManager.CurrentLookAtCharacter != null)
        {
            if (_rotationCoroutine != null)
            {
                StopCoroutine(_rotationCoroutine);
            }
            _rotationCoroutine = StartCoroutine(RotateTowardsTarget(target));
        }
    }

    private IEnumerator RotateTowardsTarget()
    {
        Quaternion originalBodyRotation = bodyTransform.rotation;
        Quaternion targetBodyRotation = Quaternion.LookRotation(_conversationManager.CurrentLookAtCharacter.position - bodyTransform.position);

        Vector3 cameraDirection = _conversationManager.CurrentLookAtCharacter.position - cameraTransform.position;
        Quaternion targetCameraRotation = Quaternion.LookRotation(cameraDirection);
        Quaternion originalCameraRotation = cameraTransform.rotation;

        float elapsed = 0;

        while (elapsed < _lookAtDuration)
        {
            elapsed += Time.deltaTime;
            float fraction = elapsed / _lookAtDuration;

            Quaternion currentBodyRotation = Quaternion.Slerp(originalBodyRotation, targetBodyRotation, fraction);
            bodyTransform.rotation = Quaternion.Euler(0, currentBodyRotation.eulerAngles.y, 0);

            cameraTransform.rotation = Quaternion.Slerp(originalCameraRotation, targetCameraRotation, fraction);

            yield return null;
        }

        bodyTransform.rotation = Quaternion.Euler(0, targetBodyRotation.eulerAngles.y, 0);
        cameraTransform.rotation = targetCameraRotation;

        _rotationCoroutine = null;
    }

    private IEnumerator RotateTowardsTarget(Transform target)
    {
        Quaternion originalBodyRotation = bodyTransform.rotation;
        Quaternion targetBodyRotation = Quaternion.LookRotation(target.position - bodyTransform.position);

        Vector3 cameraDirection = target.position - cameraTransform.position;
        Quaternion targetCameraRotation = Quaternion.LookRotation(cameraDirection);
        Quaternion originalCameraRotation = cameraTransform.rotation;

        float elapsed = 0;

        while (elapsed < _lookAtDuration)
        {
            elapsed += Time.deltaTime;
            float fraction = elapsed / _lookAtDuration;

            Quaternion currentBodyRotation = Quaternion.Slerp(originalBodyRotation, targetBodyRotation, fraction);
            bodyTransform.rotation = Quaternion.Euler(0, currentBodyRotation.eulerAngles.y, 0);

            cameraTransform.rotation = Quaternion.Slerp(originalCameraRotation, targetCameraRotation, fraction);

            yield return null;
        }

        bodyTransform.rotation = Quaternion.Euler(0, targetBodyRotation.eulerAngles.y, 0);
        cameraTransform.rotation = targetCameraRotation;

        _rotationCoroutine = null;
    }

    public void ZoomIn()
    {
        StartRotation();
        if (_zoomingCoroutine != null)
        {
            StopCoroutine(_zoomingCoroutine);
            _zoomingCoroutine = null;
        }

        _zoomingCoroutine = StartCoroutine(Zoom(_zoomInFOV));
    }

    public void ZoomOut()
    {
        if (_rotationCoroutine != null)
        {
            StopCoroutine(_rotationCoroutine);
            _rotationCoroutine = null;
        }

        if (_zoomingCoroutine != null)
        {
            StopCoroutine(_zoomingCoroutine);
            _zoomingCoroutine = null;
        }

        _zoomingCoroutine = StartCoroutine(Zoom(_zoomOutFOV));

    }

    private IEnumerator Zoom(float targetFOV)
    {
        float elapsed = 0f;

        float startFOV = _camera.fieldOfView;

        float zoomNormalizedDuration = _lookAtDuration *
                                       (Mathf.Abs(targetFOV - _camera.fieldOfView) / (_zoomOutFOV - _zoomInFOV));

        while (elapsed < zoomNormalizedDuration)
        {
            elapsed += Time.deltaTime;

            float currentFOV = Mathf.Lerp(startFOV, targetFOV, elapsed / zoomNormalizedDuration);
            _camera.fieldOfView = currentFOV;

            yield return null;
        }

        _camera.fieldOfView = targetFOV;
    }
}
