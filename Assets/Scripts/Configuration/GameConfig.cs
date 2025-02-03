using UnityEngine;

[CreateAssetMenu(fileName = nameof(GameConfig), menuName = "Config/" + nameof(GameConfig))]
public class GameConfig : ScriptableObject
{
    [SerializeField] private bool disableRateUsPrompt;

    public bool DisableRateUsPrompt => disableRateUsPrompt;
}

