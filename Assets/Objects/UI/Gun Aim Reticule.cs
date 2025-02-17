using System;
using UnityEngine;

public class GunAimReticule : MonoBehaviour
{
    Transform aimReticulePos;

    private void Start()
    {
        aimReticulePos = GameObject.FindGameObjectWithTag("Aim Reticule").GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        aimReticulePos.position = Camera.main.WorldToScreenPoint(transform.position);
    }
}
