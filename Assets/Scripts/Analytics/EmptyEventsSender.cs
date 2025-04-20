namespace BitLevel.Core.Analytics
{
    public class EmptyEventsSender : IEventsSender
    {
        public void DropRevealed(string saveId, int sessionIndex, int playtime, string dropName, string[] tags = null)
        {}

        public void GameFailed(string saveId, int sessionIndex, int playtime, int score, string[] tags = null)
        {}

        public void GameLaunched(string saveId, int sessionIndex, int playtime, string[] tags = null)
        {}

        public void NewGame(string saveId, int sessionIndex, int playtime, string[] tags = null)
        {}

        public void NoAdsPurchased(string saveId, int sessionIndex, int playtime, string[] tags = null)
        {}

        public void ScoreUpdated(string saveId, int sessionIndex, int playtime, int score, string[] tags = null)
        {}

        public void MergeBoosterUsed(string saveId, int sessionIndex, int playtime, string[] tags = null)
        {}

        public void BoosterTutorialShown(string saveId, int sessionIndex, int playtime, string[] tags = null)
        {}

        public void BoosterTutorialComplete(string saveId, int sessionIndex, int playtime, string[] tags = null)
        {}

        public void InterstitialShown(string saveId, int sessionIndex, int playtime, string placement, string[] tags = null)
        {}

        public void InterstitialClosed(string saveId, int sessionIndex, int playtime, string placement, int time, string[] tags = null)
        {}
    }
}