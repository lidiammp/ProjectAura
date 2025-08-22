using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectAfterAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string animationName;
    [SerializeField] private Button buttonToSelect;

    private void Start()
    {
        StartCoroutine(WaitForAnimation());
    }

    private System.Collections.IEnumerator WaitForAnimation()
    {
        // Wait until the animation finishes
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Select the button
        EventSystem.current.SetSelectedGameObject(buttonToSelect.gameObject);
    }
}
