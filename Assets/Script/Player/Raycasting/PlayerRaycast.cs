using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    [Header("Setups")]
    [SerializeField] private Transform _raycastPoint;
    [SerializeField] private PlayerBlocker _playerBlocker;
    [SerializeField] private LayerMask _raycastableLayer;
    private RaycastHit _lastRaycast;


    [Header("Settings")]
    [SerializeField] private float _raycastDistance;

    public event Action<RaycastHit> FoundRaycastable = delegate { };
    public event Action LostRaycastable = delegate { };

    private void Start()
    {

        InputManager.Instance.InputHandler.Player.LMB.started += (callback) => { RaycastContactable(); };
    }

    private void FixedUpdate()
    {
        Raycast();
        Debug.DrawRay(_raycastPoint.position, transform.forward * _raycastDistance, Color.green);

    }

    private void Raycast()
    {
        if (_playerBlocker.RaycastingBlocked)
        {
            return;
        }

        Physics.Raycast(_raycastPoint.position, transform.forward * _raycastDistance,
            out RaycastHit hit, _raycastDistance, _raycastableLayer);

        if (_lastRaycast.transform == null && hit.transform != null)
        {
            FoundRaycastable?.Invoke(hit);
        }
        else if (_lastRaycast.transform != null && hit.transform == null)
        {
            LostRaycastable?.Invoke();
        }

        _lastRaycast = hit;
    }

    private void RaycastContactable()
    {
        if (_playerBlocker.RaycastingBlocked)
        {
            return;
        }

        Physics.Raycast(_raycastPoint.position, transform.forward * _raycastDistance,
            out RaycastHit hit, _raycastDistance, _raycastableLayer);

        if (hit.transform != null && hit.transform.GetComponent<IContactable>() != null)
        {
            hit.transform.GetComponent<IContactable>().GetContacted();
        }
    }
}
