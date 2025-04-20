using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BitLevel.Core.Analytics
{
    public class DebugEventsSender : IEventsSender
    {
        private StringBuilder stringBuilder = new();

        public void GameLaunched(string saveId, int sessionIndex, int playtime, string[] tags = null)
        {
            Send(saveId, sessionIndex, playtime, GameEvents.EVENT_GAME_LAUNCHED, tags);
        }

        public void NewGame(string saveId, int sessionIndex, int playtime, string[] tags = null)
        {
            Send(saveId, sessionIndex, playtime, GameEvents.EVENT_NEW_GAME, tags);
        }
        
        public void ScoreUpdated(string saveId, int sessionIndex, int playtime, int score, string[] tags = null)
        {
            Send(saveId, sessionIndex, playtime, GameEvents.EVENT_SCORE_UPDATED, tags, new Dictionary<string, object>
            {
                { GameEvents.KEY_SCORE, score }
            });
        }

        public void GameFailed(string saveId, int sessionIndex, int playtime, int score, string[] tags = null)
        {
            Send(saveId, sessionIndex, playtime, GameEvents.EVENT_GAME_FAILED, tags, new Dictionary<string, object>
            {
                { GameEvents.KEY_SCORE, score }
            });
        }

        public void DropRevealed(string saveId, int sessionIndex, int playtime, string dropName, string[] tags = null)
        {
            Send(saveId, sessionIndex, playtime, GameEvents.EVENT_DROP_REVEALED, tags, new Dictionary<string, object>
            {
                { GameEvents.KEY_DROP_NAME, dropName }
            });
        }

        public void NoAdsPurchased(string saveId, int sessionIndex, int playtime, string[] tags = null)
        {
            Send(saveId, sessionIndex, playtime, GameEvents.EVENT_NO_ADS_PURCHASED, tags);
        }

        public void MergeBoosterUsed(string saveId, int sessionIndex, int playtime, string[] tags = null)
        {
            Send(saveId, sessionIndex, playtime, GameEvents.MERGE_BOOSTER_USED, tags);
        }

        public void BoosterTutorialShown(string saveId, int sessionIndex, int playtime, string[] tags = null)
        {
            Send(saveId, sessionIndex, playtime, GameEvents.BOOSTER_TUTORIAL_SHOWN, tags);
        }

        public void BoosterTutorialComplete(string saveId, int sessionIndex, int playtime, string[] tags = null)
        {
            Send(saveId, sessionIndex, playtime, GameEvents.BOOSTER_TUTORIAL_COMPLETE, tags);
        }

        public void InterstitialShown(string saveId, int sessionIndex, int playtime, string placement, string[] tags = null)
        {
            Send(saveId, sessionIndex, playtime, GameEvents.INTERSTITIAL_SHOWN, tags, new Dictionary<string, object>
            {
                { GameEvents.KEY_PLACEMENT, placement }
            });
        }

        public void InterstitialClosed(string saveId, int sessionIndex, int playtime, string placement, int time, string[] tags = null)
        {
            Send(saveId, sessionIndex, playtime, GameEvents.INTERSTITIAL_CLOSED, tags, new Dictionary<string, object>
            {
                { GameEvents.KEY_PLACEMENT, placement },
                { GameEvents.KEY_TIME, time }
            });
        }

        public void UpgradeBought(string saveId, int sessionIndex, int playtime, int level, string[] tags = null)
        {
            Send(saveId, sessionIndex, playtime, GameEvents.UPGRADE_BOUGHT, tags, new Dictionary<string, object>
            {
                { GameEvents.KEY_LEVEL, level }
            });
        }

        private void Send(string userId, int sessionIndex, int playtime, string eventType, string[] tags = null, Dictionary<string, object> extras = null)
        {
            stringBuilder.Append(string.Format("{0}\t{1}\t{2}\t{3}",
                userId,
                sessionIndex,
                playtime,
                eventType));

            if (tags != null)
                stringBuilder.Append($"\ttags: {string.Join(", ", tags)}");
            
            if (extras != null) stringBuilder.Append($"\t{DictionaryToJson(extras)}");

            Debug.Log(stringBuilder.ToString());
            stringBuilder.Clear();
        }

        private string DictionaryToJson(Dictionary<string, object> dictionary)
        {
            StringBuilder sb = new();
            sb.Append("{");

            foreach (KeyValuePair<string, object> kvp in dictionary)
                sb.Append(string.Format("\"{0}\": \"{1}\",", kvp.Key, kvp.Value));
            
            if (dictionary.Count > 0)
                sb.Remove(sb.Length - 1, 1);

            sb.Append("}");
            return sb.ToString();
        }
    }
}