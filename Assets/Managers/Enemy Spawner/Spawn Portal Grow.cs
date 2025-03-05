using System;
using UnityEngine;

public class SpawnPortalGrow : MonoBehaviour
{
    [SerializeField] float growthRate;

    private void Update()
    {
        GrowPortal();
    }

    void GrowPortal()
    {
        transform.localScale = new Vector3(transform.localScale.x + (growthRate * Time.deltaTime), transform.localScale.y + (growthRate * Time.deltaTime), transform.localScale.z + (growthRate * Time.deltaTime));
    }
}
