using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class LevelSystem : MonoBehaviour
{
#pragma warning disable 0649
    [Range(0f, 1f)]
    [SerializeField] float progress;
    [SerializeField] Image fill;
    [SerializeField] Image[] marks;
    [SerializeField] Text[] labels;
    [SerializeField] string[] ranks;
    [SerializeField] string[] limits;
    [SerializeField] RectTransform currentPositionMark;
    [Range(0f, 1f)]
    [SerializeField] float currentPosition;
    [SerializeField] Skyscraper.FloorColorSet[] ranksColorSets;
    [SerializeField] new ParticleSystem particleSystem;
    [SerializeField] Animation glowAnim;
    [SerializeField] GameObject noAds;
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

    private Animation _animation;
    private int _openRanksCount = -1;
    private bool _firstRankChanged = false;
    #endregion

    #region Events

    public delegate void MethodContainer();
    public event MethodContainer OnRankUp;
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
        if (fill != null) fill.fillAmount = progress;

        // Marks colorize
        float oneSectionLength = 1f / marks.Length;
        int activeCount = Mathf.FloorToInt(fill.fillAmount / oneSectionLength);
        for (int i = 0; i < marks.Length; ++i)
        {
            marks[i].color = (i < activeCount) ? ACTIVE : DISACTIVE;
            if (i < ranks.Length)
            {
                if (i < activeCount)
                {
                    labels[i].text = ranks[i];
                    labels[i].color = Color.white;
                }
                else if (i == activeCount)
                {
                    labels[i].text = limits[i];
                    labels[i].color = Color.white * .7f;
                }
                else
                {
                    labels[i].text = "???";
                    labels[i].color = Color.white;
                }
            }
        }

        // Current Position mark update
        float totalDistance = fill.rectTransform.rect.width;
        currentPositionMark.anchoredPosition = Vector2.Lerp(
            currentPositionMark.anchoredPosition,
            new Vector2(currentPosition * totalDistance, 0f),
            Time.deltaTime * 10f);

        if (activeCount != _openRanksCount)
        {
            _openRanksCount = activeCount;
            NewRank(_openRanksCount);
        }
    }
    #endregion

    #region Private Functions

    private void NewRank(int newRank)
    {
        if (Skyscraper.Instance != null)
            Skyscraper.Instance.currentColorSet = ranksColorSets[newRank];

        if (newRank == 4) // Last
            Ad.Block[Ad.Type.Interstitial] = true;

        if (_firstRankChanged)
        {
            ShowVFX();
            if (OnRankUp != null) OnRankUp();
        }
        _firstRankChanged = true;

        if (newRank > 0)
            labels[newRank - 1].GetComponent<Animation>().Play("LabelRankUp");
    }

    private void ShowVFX()
    {
        particleSystem.Play();
        glowAnim.Play("Glow");
    }
    #endregion
}
