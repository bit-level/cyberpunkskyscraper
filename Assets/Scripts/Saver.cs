using UnityEngine;
using PlayerPrefs = RedefineYG.PlayerPrefs;

public class Saver : MonoBehaviour
{
    private float timer;

    public static Saver Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.unscaledDeltaTime;

            if (timer <= 0f)
            {
                PlayerPrefs.Save();
                timer = 0f;
            }
        }
    }

    public void Save()
    {
        timer = 1f;
    }
}