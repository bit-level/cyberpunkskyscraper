namespace BitLevel.Core.Analytics
{
    public interface IEventsSender
    {
        void GameLaunched(string saveId, int sessionIndex, int playtime, string[] tags = null);
        void NewGame(string saveId, int sessionIndex, int playtime, string[] tags = null);
        void GameFailed(string saveId, int sessionIndex, int playtime, int score, string[] tags = null);
        void ScoreUpdated(string saveId, int sessionIndex, int playtime, int score, string[] tags = null);
        void DropRevealed(string saveId, int sessionIndex, int playtime, string dropName, string[] tags = null);
        void NoAdsPurchased(string saveId, int sessionIndex, int playtime, string[] tags = null);
        void MergeBoosterUsed(string saveId, int sessionIndex, int playtime, string[] tags = null);
        void BoosterTutorialShown(string saveId, int sessionIndex, int playtime, string[] tags = null);
        void BoosterTutorialComplete(string saveId, int sessionIndex, int playtime, string[] tags = null);
        void InterstitialShown(string saveId, int sessionIndex, int playtime, string placement, string[] tags = null);
        void InterstitialClosed(string saveId, int sessionIndex, int playtime, string placement, int time, string[] tags = null);
        void UpgradeBought(string saveId, int sessionIndex, int playtime, int level, string[] tags = null);
    }
}