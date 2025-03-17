using System;
using UnityEngine;

public class GunAimReticule : MonoBehaviour
{
    Transform aimReticulePos;

    private void Awake()
    {
        aimReticulePos = GameObject.FindGameObjectWithTag("Aim Reticule").GetComponent<Transform>();
    }

    private void Start()
    {
        
    }

    private void LateUpdate()
    {
        aimReticulePos.position = Camera.main.WorldToScreenPoint(transform.position);
    }
}
