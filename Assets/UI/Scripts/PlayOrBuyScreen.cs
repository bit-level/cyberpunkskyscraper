using UnityEngine;

public class PlayOrBuyScreen : MonoBehaviour
{
    public void Show()
    {
        gameObject.SetActive(true);
        GetComponent<Animation>().Play("Show");
    }

    public void Hide()
    {
        ActionList al = new ActionList(GetComponent<ActionSequencer>());
        Animation anim = GetComponent<Animation>();
        al.Add(() => anim.Play("Hide"), anim["Hide"].length);
        al.Add(() => gameObject.SetActive(false));
        al.Execute();
    }

    public void Buy()
    {
        CyberpunkSkyscraper.Purchaser.Instance.BuyNoAds();
        Hide();
    }
}
