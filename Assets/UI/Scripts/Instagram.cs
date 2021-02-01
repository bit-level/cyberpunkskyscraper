using UnityEngine;

public class Instagram : MonoBehaviour
{
    public void PlayClickAnimation()
    {
        GetComponent<Animation>().Play("Click");
    }
}
