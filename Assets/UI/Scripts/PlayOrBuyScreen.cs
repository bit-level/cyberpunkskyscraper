using UnityEngine;
using UnityEngine.UI;

namespace CyberpunkSkyscraper
{
    public class PlayOrBuyScreen : MonoBehaviour
    {
        [SerializeField] Text description = null;

        private void Start()
        {
            description.text = string.Format(description.text, Purchaser.GetPrice("no_ads"));
        }

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
}
