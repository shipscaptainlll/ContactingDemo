using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastableObject : MonoBehaviour
{
    [SerializeField] private string _raycastInfo;

    public string RaycastInfo => _raycastInfo;
    
}
