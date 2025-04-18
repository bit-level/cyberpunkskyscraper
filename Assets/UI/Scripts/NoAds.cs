using UnityEngine;
using PlayerPrefs = RedefineYG.PlayerPrefs;

public class NoAds : MonoBehaviour
{
    [SerializeField] GameObject noAdsImage = null;

    private const string PREFS_KEY = "NoAds";
    
    public static NoAds Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        if (PlayerPrefs.HasKey(PREFS_KEY)) Activate();
    }

    public static void Activate()
    {
        if (Instance == null) return;
        PlayerPrefs.SetInt(PREFS_KEY, 1);
        Saver.Instance.Save();
        Ad.Instance.Block[Ad.Type.Display] = true;
        Ad.Instance.Block[Ad.Type.Interstitial] = true;
        Destroy(Instance.gameObject);
        Destroy(Instance.noAdsImage);
    }
}
