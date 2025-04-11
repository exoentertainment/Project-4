using MoreMountains.Tools;
using UnityEngine;

public class RotaryTurret : BaseTurret
{
    [SerializeField] private MMAutoRotate[] rotatingBarrels;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}
