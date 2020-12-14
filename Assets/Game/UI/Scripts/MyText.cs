using UnityEngine;

public class MyText : MonoBehaviour
{
    protected bool isShowed { get; private set; } = true;

    protected void Start()
    {
        Show();
    }

    protected void Show()
    {
        GetComponent<Animation>().Play("Show");
        isShowed = true;
    }

    protected void Hide()
    {
        GetComponent<Animation>().Play("Hide");
        isShowed = false;
    }
}
