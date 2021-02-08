using UnityEngine;
using UnityEngine.UI;

public class HintScreen : MonoBehaviour
{
    public void Show()
    {
        gameObject.SetActive(true);
        GetComponent<Animation>().Play("Show");
    }

    public void Hide(Button btn)
    {
        if (btn != null) btn.interactable = false;
        ActionList al = new ActionList(GetComponent<ActionSequencer>());
        Animation anim = GetComponent<Animation>();
        al.Add(() => anim.Play("Hide"), anim["Hide"].length);
        al.Add(() => gameObject.SetActive(false));
        al.Execute();
    }
}
