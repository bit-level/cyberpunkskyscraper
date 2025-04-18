using System;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class Ad : MonoBehaviour
{
    public enum Type { Interstitial, Rewarded, Display }

    public static Ad Instance { get; private set; }

    public Dictionary<Type, bool> Block = new Dictionary<Type, bool>
    {
        { Type.Interstitial, false },
        { Type.Rewarded, false },
        { Type.Display, false }
    };

    public event Action RewardedWatched;

    private void Awake()
    {
        Instance = this;
    }

    public bool IsReady(Type type) => true;

    public void Show(Type type)
    {
        if (Block[type]) return;
        Debug.Log("Show: " + type);
        if (type == Type.Rewarded)
            RewardedWatched?.Invoke();
    }

    public bool ShowIfReady(Type type)
    {
        bool isReady = IsReady(type);
        if (isReady) Show(type);
        return isReady;
    }

    public void ShowRewarded(string id, Action callback = null)
    {
        YG2.RewardedAdvShow(id, callback);
    }
}
