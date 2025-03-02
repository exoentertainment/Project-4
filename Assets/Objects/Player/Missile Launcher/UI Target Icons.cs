using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UITargetIcons : MonoBehaviour
{
    [SerializeField] GameObject targetIconPrefab;
    [SerializeField] float targetIconDuration;

    //Create a target icon and pass it and the target to the coroutine to place the icon on screen
    public void SetTargetIcon(GameObject target)
    {
        GameObject targetIcon = Instantiate(targetIconPrefab, transform);
        StartCoroutine(UpdateTargetIconRoutine(targetIcon, target));
        
        Destroy(targetIcon, targetIconDuration);
    }

    //Update the on-screen position of the target icons
    IEnumerator UpdateTargetIconRoutine(GameObject targetIcon, GameObject target)
    {
        while (targetIcon != null)
        {
            targetIcon.transform.position = Camera.main.WorldToScreenPoint(target.transform.position);
            yield return new WaitForEndOfFrame();
        }
    }
}
