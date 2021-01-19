using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class Ad : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] bool testMode = true;
#pragma warning restore 0649

    private const string GAMEID_IOS     = "3979422";
    private const string GAMEID_ANDROID = "3979423";

    public enum Type { Interstitial, Rewarded }

    public static Ad Instance { get; private set; }

    #region Private Fields

    private Dictionary<Type, string> _placementIds = new Dictionary<Type, string>()
    {
        { Type.Interstitial,  "video" },
        { Type.Rewarded,      "rewardedVideo" }
    };
    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        string gameID = (Application.platform == RuntimePlatform.Android) ? GAMEID_ANDROID : GAMEID_IOS;
        Advertisement.Initialize(gameID, testMode);
    }
    #endregion

    #region Public Functions

    public bool IsReady(Type type) => Advertisement.IsReady(_placementIds[type]);

    public void Show(Type type) { Advertisement.Show(_placementIds[type]); }

    public void ShowIfReady(Type type) { if (IsReady(type)) Show(type); }
    #endregion
}
