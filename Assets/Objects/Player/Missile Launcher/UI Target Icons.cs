using UnityEngine;
using UnityEngine.UI;

public class UITargetIcons : MonoBehaviour
{
    [SerializeField] GameObject targetIconPrefab;
    [SerializeField] float targetIconDuration;

    public void SetTargets(GameObject target)
    {
        // foreach (Collider target in targets)
        // {
        //     if (CameraManager.Instance.ObjectInCameraView(target.transform))
        //     {
        //         GameObject targetIcon = Instantiate(targetIconPrefab, transform);
        //         targetIcon.transform.position = Camera.main.WorldToScreenPoint(target.transform.position);
        //         
        //         Destroy(targetIcon, targetIconDuration);
        //     }
        // }
        
        GameObject targetIcon = Instantiate(targetIconPrefab, transform);
        targetIcon.transform.position = Camera.main.WorldToScreenPoint(target.transform.position);
        
        Destroy(targetIcon, targetIconDuration);
    }
}
