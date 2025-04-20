using System.Text;
using UnityEngine.Networking;

namespace BitLevel.Core.Analytics
{
    public class HttpEventsSender : IEventsSender
    {
        public void GameLaunched(string saveId, int sessionIndex, int playtime, string[] tags = null)
        {
            var dict = new JsonDictionary()
                .Add(GameEvents.KEY_SAVE_ID, saveId)
                .Add(GameEvents.KEY_SESSION_INDEX, sessionIndex)
                .Add(GameEvents.KEY_PLAYTIME, playtime)
                .Add(GameEvents.KEY_EVENT_TYPE, GameEvents.EVENT_GAME_LAUNCHED);

            if (tags != null)
                dict.Add(GameEvents.KEY_TAGS, tags);

            Send(dict.ToJson());
        }

        public void NewGame(string saveId, int sessionIndex, int playtime, string[] tags = null)
        {
            var dict = new JsonDictionary()
                .Add(GameEvents.KEY_SAVE_ID, saveId)
                .Add(GameEvents.KEY_SESSION_INDEX, sessionIndex)
                .Add(GameEvents.KEY_PLAYTIME, playtime)
                .Add(GameEvents.KEY_EVENT_TYPE, GameEvents.EVENT_NEW_GAME);

            if (tags != null)
                dict.Add(GameEvents.KEY_TAGS, tags);

            Send(dict.ToJson());
        }

        public void ScoreUpdated(string saveId, int sessionIndex, int playtime, int score, string[] tags = null)
        {
            var dict = new JsonDictionary()
                .Add(GameEvents.KEY_SAVE_ID, saveId)
                .Add(GameEvents.KEY_SESSION_INDEX, sessionIndex)
                .Add(GameEvents.KEY_PLAYTIME, playtime)
                .Add(GameEvents.KEY_EVENT_TYPE, GameEvents.EVENT_SCORE_UPDATED)
                .Add(GameEvents.KEY_SCORE, score);

            if (tags != null)
                dict.Add(GameEvents.KEY_TAGS, tags);

            Send(dict.ToJson());
        }

        public void GameFailed(string saveId, int sessionIndex, int playtime, int score, string[] tags = null)
        {
            var dict = new JsonDictionary()
                .Add(GameEvents.KEY_SAVE_ID, saveId)
                .Add(GameEvents.KEY_SESSION_INDEX,  sessionIndex)
                .Add(GameEvents.KEY_PLAYTIME, playtime)
                .Add(GameEvents.KEY_EVENT_TYPE, GameEvents.EVENT_GAME_FAILED)
                .Add(GameEvents.KEY_SCORE, score);

            if (tags != null)
                dict.Add(GameEvents.KEY_TAGS, tags);

            Send(dict.ToJson());
        }

        public void DropRevealed(string saveId, int sessionIndex, int playtime, string dropName, string[] tags = null)
        {
            var dict = new JsonDictionary()
                .Add(GameEvents.KEY_SAVE_ID, saveId)
                .Add(GameEvents.KEY_SESSION_INDEX, sessionIndex)
                .Add(GameEvents.KEY_PLAYTIME, playtime)
                .Add(GameEvents.KEY_EVENT_TYPE, GameEvents.EVENT_DROP_REVEALED)
                .Add(GameEvents.KEY_DROP_NAME, dropName);

            if (tags != null)
                dict.Add(GameEvents.KEY_TAGS, tags);

            Send(dict.ToJson());
        }

        public void NoAdsPurchased(string saveId, int sessionIndex, int playtime, string[] tags = null)
        {
            var dict = new JsonDictionary()
                .Add(GameEvents.KEY_SAVE_ID, saveId)
                .Add(GameEvents.KEY_SESSION_INDEX, sessionIndex)
                .Add(GameEvents.KEY_PLAYTIME, playtime)
                .Add(GameEvents.KEY_EVENT_TYPE, GameEvents.EVENT_NO_ADS_PURCHASED);

            if (tags != null)
                dict.Add(GameEvents.KEY_TAGS, tags);

            Send(dict.ToJson());
        }

        public void MergeBoosterUsed(string saveId, int sessionIndex, int playtime, string[] tags = null)
        {
            var dict = new JsonDictionary()
                .Add(GameEvents.KEY_SAVE_ID, saveId)
                .Add(GameEvents.KEY_SESSION_INDEX, sessionIndex)
                .Add(GameEvents.KEY_PLAYTIME, playtime)
                .Add(GameEvents.KEY_EVENT_TYPE, GameEvents.MERGE_BOOSTER_USED);

            if (tags != null)
                dict.Add(GameEvents.KEY_TAGS, tags);

            Send(dict.ToJson());
        }

        public void BoosterTutorialShown(string saveId, int sessionIndex, int playtime, string[] tags = null)
        {
            var dict = new JsonDictionary()
                .Add(GameEvents.KEY_SAVE_ID, saveId)
                .Add(GameEvents.KEY_SESSION_INDEX, sessionIndex)
                .Add(GameEvents.KEY_PLAYTIME, playtime)
                .Add(GameEvents.KEY_EVENT_TYPE, GameEvents.BOOSTER_TUTORIAL_SHOWN);

            if (tags != null)
                dict.Add(GameEvents.KEY_TAGS, tags);

            Send(dict.ToJson());
        }

        public void BoosterTutorialComplete(string saveId, int sessionIndex, int playtime, string[] tags = null)
        {
            var dict = new JsonDictionary()
                .Add(GameEvents.KEY_SAVE_ID, saveId)
                .Add(GameEvents.KEY_SESSION_INDEX, sessionIndex)
                .Add(GameEvents.KEY_PLAYTIME, playtime)
                .Add(GameEvents.KEY_EVENT_TYPE, GameEvents.BOOSTER_TUTORIAL_COMPLETE);

            if (tags != null)
                dict.Add(GameEvents.KEY_TAGS, tags);

            Send(dict.ToJson());
        }

        public void InterstitialShown(string saveId, int sessionIndex, int playtime, string placement, string[] tags = null)
        {
            var dict = new JsonDictionary()
                .Add(GameEvents.KEY_SAVE_ID, saveId)
                .Add(GameEvents.KEY_SESSION_INDEX, sessionIndex)
                .Add(GameEvents.KEY_PLAYTIME, playtime)
                .Add(GameEvents.KEY_EVENT_TYPE, GameEvents.INTERSTITIAL_SHOWN)
                .Add(GameEvents.KEY_PLACEMENT, placement);

            if (tags != null)
                dict.Add(GameEvents.KEY_TAGS, tags);

            Send(dict.ToJson());
        }

        public void InterstitialClosed(string saveId, int sessionIndex, int playtime, string placement, int time, string[] tags = null)
        {
            var dict = new JsonDictionary()
                .Add(GameEvents.KEY_SAVE_ID, saveId)
                .Add(GameEvents.KEY_SESSION_INDEX, sessionIndex)
                .Add(GameEvents.KEY_PLAYTIME, playtime)
                .Add(GameEvents.KEY_EVENT_TYPE, GameEvents.INTERSTITIAL_CLOSED)
                .Add(GameEvents.KEY_PLACEMENT, placement)
                .Add(GameEvents.KEY_TIME, time);

            if (tags != null)
                dict.Add(GameEvents.KEY_TAGS, tags);

            Send(dict.ToJson());
        }

        public void UpgradeBought(string saveId, int sessionIndex, int playtime, int level, string[] tags = null)
        {
            var dict = new JsonDictionary()
                .Add(GameEvents.KEY_SAVE_ID, saveId)
                .Add(GameEvents.KEY_SESSION_INDEX, sessionIndex)
                .Add(GameEvents.KEY_PLAYTIME, playtime)
                .Add(GameEvents.KEY_EVENT_TYPE, GameEvents.UPGRADE_BOUGHT)
                .Add(GameEvents.KEY_LEVEL, level);

            if (tags != null)
                dict.Add(GameEvents.KEY_TAGS, tags);

            Send(dict.ToJson());
        }

        private void Send(string json)
        {
            string url = "https://animals-farm.bit-level.com/api/cyberpunk-skyscraper/new-event";
            byte[] data = Encoding.UTF8.GetBytes(json);

            UnityWebRequest www = new UnityWebRequest(url, "POST");
            www.uploadHandler = new UploadHandlerRaw(data);
            www.SetRequestHeader("Content-Type", "application/json");
            www.SendWebRequest();
        }
    }
}