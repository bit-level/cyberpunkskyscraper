using UnityEngine;

public class ScreenPositionChecker : MonoBehaviour
{
    private bool _checked = false;

    private void Update()
    {
        if (!_checked && IsObjectVisible())
        {
            print(name);
            GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
            _checked = true;
        }
    }

    private bool IsObjectVisible()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        screenPos.x -= Screen.width / 2f;
        screenPos.y -= Screen.height / 2f;

        float shift = 90f;

        bool xCorrect = Mathf.Abs(screenPos.x) - shift < Screen.width / 2f;
        bool yCorrect = Mathf.Abs(screenPos.y) - shift < Screen.height / 2f;

        bool visible = xCorrect && yCorrect;

        return visible;
    }
}
