using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class LevelSystem : MonoBehaviour
{
#pragma warning disable 0649
    [Range(0f, 1f)]
    [SerializeField] private float progress;
    [SerializeField] private Image fill;
    [SerializeField] private Image[] marks;
    [SerializeField] private Text[] labels;
    [SerializeField] private string[] ranks;
    [SerializeField] private RectTransform currentPositionMark;
    [Range(0f, 1f)]
    [SerializeField] private float currentPosition;
#pragma warning restore 0649

    private readonly Color ACTIVE       = new Color(.078f, .77f, .18f);
    private readonly Color DISACTIVE    = new Color(1f, 1f, 1f);
    public static LevelSystem Instance { get; private set; }

    #region Properties

    public float Progress
    {
        get => progress;
        set { progress = Mathf.Clamp(value, 0f, 1f); }
    }

    public float CurrentPosition
    {
        get => currentPosition;
        set { currentPosition = Mathf.Clamp(value, 0f, 1f); }
    }
    #endregion

    #region Private Fields

    private new Animation _animation;
    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        Instance = this;

        _animation = GetComponent<Animation>();
        _animation.Play("Show");
    }

    private void Update()
    {
        if (_animation.IsPlaying("Show")) return;

        if (fill != null) fill.fillAmount = Mathf.Lerp(fill.fillAmount, progress, Time.deltaTime * 10f);

        // Marks colorize
        float oneSectionLength = 1f / marks.Length;
        int activeCount = Mathf.FloorToInt(fill.fillAmount / oneSectionLength);
        for (int i = 0; i < marks.Length; ++i)
        {
            marks[i].color = (i < activeCount) ? ACTIVE : DISACTIVE;
            if (i < ranks.Length)
                labels[i].text = (i < activeCount) ? ranks[i] : "???";
        }

        // Current Position mark update
        float totalDistance = fill.rectTransform.rect.width;
        currentPositionMark.anchoredPosition = Vector2.Lerp(
            currentPositionMark.anchoredPosition,
            new Vector2(currentPosition * totalDistance, 0f),
            Time.deltaTime * 10f);
    }
    #endregion
}
