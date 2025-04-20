using System.Collections.Generic;
using System.Linq;
using BitLevel.Tools;
using UnityEngine;

namespace BitLevel.Core.Analytics
{
    public static class GameEvents
    { 
        public const string KEY_SAVE_ID = "save_id";
        public const string KEY_SESSION_INDEX = "session_index";
        public const string KEY_PLAYTIME = "playtime";
        public const string KEY_EVENT_TYPE = "event_type";
        public const string KEY_TAGS = "tags";
        public const string KEY_SCORE = "score";
        public const string KEY_DROP_NAME = "drop_name";
        public const string KEY_PLACEMENT = "placement";
        public const string KEY_TIME = "time";

        public const string EVENT_GAME_LAUNCHED = "game_launched";
        public const string EVENT_NEW_GAME = "new_game";
        public const string EVENT_SCORE_UPDATED = "score_updated";
        public const string EVENT_GAME_FAILED = "game_failed";
        public const string EVENT_REVIVED = "revived";
        public const string EVENT_DROP_REVEALED = "drop_revealed";
        public const string EVENT_NO_ADS_PURCHASED = "no_ads_purchased";
        public const string MERGE_BOOSTER_USED = "merge_booster_used";
        public const string BOOSTER_TUTORIAL_SHOWN = "booster_tutorial_shown";
        public const string BOOSTER_TUTORIAL_COMPLETE = "booster_tutorial_complete";
        public const string INTERSTITIAL_SHOWN = "interstitial_shown";
        public const string INTERSTITIAL_CLOSED = "interstitial_closed";

        private static string saveId;
        private static int sessionIndex;
        private static IReadOnlyList<IEventsSender> senders;
        private static string[] tags = new string[0];

        public static void Init(string saveId, int sessionIndex, IReadOnlyList<IEventsSender> senders)
        {
            GameEvents.saveId = saveId;
            GameEvents.sessionIndex = sessionIndex;
            GameEvents.senders = senders;
        }

        public static void AddTags(params string[] tags)
        {
            GameEvents.tags = GameEvents.tags.Concat(tags).ToArray();
        }

        public static void GameLaunched()
        {
            for (int i = 0; i < senders.Count; i++)
                senders[i].GameLaunched(saveId, sessionIndex, GetPlaytime(), tags);
        }

        public static void NewGame()
        {
            for (int i = 0; i < senders.Count; i++)
                senders[i].NewGame(saveId, sessionIndex, GetPlaytime(), tags);
        }

        public static void ScoreUpdated(int score)
        {
            for (int i = 0; i < senders.Count; i++)
                senders[i].ScoreUpdated(saveId, sessionIndex, GetPlaytime(), score, tags);
        }

        public static void GameFailed(int score)
        {
            for (int i = 0; i < senders.Count; i++)
                senders[i].GameFailed(saveId, sessionIndex, GetPlaytime(), score, tags);
        }

        public static void DropRevealed(string dropName)
        {
            for (int i = 0; i < senders.Count; i++)
                senders[i].DropRevealed(saveId, sessionIndex, GetPlaytime(), dropName, tags);
        }

        public static void NoAdsPurchased()
        {
            for (int i = 0; i < senders.Count; i++)
                senders[i].NoAdsPurchased(saveId, sessionIndex, GetPlaytime(), tags);
        }

        public static void MergeBoosterUsed()
        {
            for (int i = 0; i < senders.Count; i++)
                senders[i].MergeBoosterUsed(saveId, sessionIndex, GetPlaytime(), tags);
        }

        public static void BoosterTutorialShown()
        {
            for (int i = 0; i < senders.Count; i++)
                senders[i].BoosterTutorialShown(saveId, sessionIndex, GetPlaytime(), tags);
        }

        public static void BoosterTutorialComplete()
        {
            for (int i = 0; i < senders.Count; i++)
                senders[i].BoosterTutorialComplete(saveId, sessionIndex, GetPlaytime(), tags);
        }

        public static void InterstitialShown(string placement)
        {
            for (int i = 0; i < senders.Count; i++)
                senders[i].InterstitialShown(saveId, sessionIndex, GetPlaytime(), placement, tags);
        }

        public static void InterstitialClosed(string placement, int time)
        {
            for (int i = 0; i < senders.Count; i++)
                senders[i].InterstitialClosed(saveId, sessionIndex, GetPlaytime(), placement, time, tags);
        }

        public static int GetPlaytime() => Mathf.FloorToInt(Playtime.Instance.Get());
    }
}