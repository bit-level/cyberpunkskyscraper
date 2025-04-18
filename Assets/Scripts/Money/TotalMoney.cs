using PlayerPrefs = RedefineYG.PlayerPrefs;

public class TotalMoney : Money
{
    public static TotalMoney Instance { get; private set; }

    #region Events

    public delegate void MethodContainer(int arg);
    public event MethodContainer OnPutMoney;
    #endregion

    #region Constants

    private const string PREFSKEY = "wallet";
    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        Instance = this;
        amount = LoadWalletAmount();
        RenderAmount(amount);
    }
    #endregion

    #region Public Functions

    public void PutMoney(int amount)
    {
        base.PutMoney(amount);

        Save();
        OnPutMoney(amount);
    }

    public new void TakeMoney(int amount)
    {
        base.TakeMoney(amount);
        Save();
    }
    #endregion

    #region Private Functions

    private int LoadWalletAmount()
    {
        const string PREFSKEY = "wallet";
        int amount;

        if (PlayerPrefs.HasKey(PREFSKEY))
            amount = PlayerPrefs.GetInt(PREFSKEY);
        else
            PlayerPrefs.SetInt(PREFSKEY, amount = 0);

        return amount;
    }
   
    private void Save()
    {
        PlayerPrefs.SetInt(PREFSKEY, amount);
        Saver.Instance.Save();
    }
    #endregion
}
