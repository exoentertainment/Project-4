using UnityEngine;
using UnityEngine.UI;

public class UITargetIcons : MonoBehaviour
{
    [SerializeField] GameObject targetIconPrefab;
    [SerializeField] float targetIconDuration;

    public void SetTargetIcon(GameObject target)
    {
        GameObject targetIcon = Instantiate(targetIconPrefab, transform);
        targetIcon.transform.position = Camera.main.WorldToScreenPoint(target.transform.position);
        
        Destroy(targetIcon, targetIconDuration);
    }
}
