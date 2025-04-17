var ysdk;
var player;
var save;
var rewardedAdCompleteFlag;
var rewardedAdCloseFlag;
var fullscreenAdCompleteFlag;
var rewardedAdOpenedFlag;
var fullscreenAdOpenedFlag;


YaGames.init().then(_ysdk => {
    ysdk = _ysdk;
    _ysdk.getPlayer({ scopes: true }).then(_player => {
        player = _player;
        player.getData(['save']).then(_save => {
            save = _save['save'];
        });
    });
});

function getPlayerData() {
    if (save == null) return "";
    return JSON.stringify(save);
}

function saveGameData(dataStr) {
    if (player == null) return;
    var data = JSON.parse(dataStr);
    data = {"save": data};
    player.setData(data);
}

function showFullscreenAdv() {
    ysdk.adv.showFullscreenAdv({
        callbacks: {
            onClose: function(wasShown) {
                if (wasShown)
                    fullscreenAdCompleteFlag = true;
            },
            onError: function(error) {
                console.log("Fullscreen adv error: " + error);
            },
            onOpen: function() {
                fullscreenAdOpenedFlag = true;
            }
        }
    });
}

function showRewardedVideo() {
    ysdk.adv.showRewardedVideo({
        callbacks: {
            onRewarded: () => {
                rewardedAdCompleteFlag = true;
            },
            onClose: () => {
                rewardedAdCloseFlag = true;
            },
            onOpen: () => {
                rewardedAdOpenedFlag = true;
            }
        }
    });
}

function getTime() {
    if (ysdk == null) return -1;
    return ysdk.serverTime();
}

function getLanguage() {
    if (ysdk == null) return "";
    return ysdk.environment.i18n.lang;
}

function gameReadyApi_ready() {
    if (ysdk == null) return;
    ysdk.features.LoadingAPI?.ready();
}

function getRewardedAdCompleteFlag() { return rewardedAdCompleteFlag; }
function getRewardedAdCloseFlag() { return rewardedAdCloseFlag; }
function getFullscreenAdCompleteFlag() { return fullscreenAdCompleteFlag; }
function getRewardedAdOpenedFlag() { return rewardedAdOpenedFlag; }
function getFullscreenAdOpenedFlag() { return fullscreenAdOpenedFlag; }

function resetRewardedAdCompleteFlag() { rewardedAdCompleteFlag = false; }
function resetRewardedAdCloseFlag() { rewardedAdCloseFlag = false; }
function resetFullscreenAdCompleteFlag() { fullscreenAdCompleteFlag = false; }
function resetRewardedAdOpenedFlag() { rewardedAdOpenedFlag = false; }
function resetFullscreenAdOpenedFlag() { fullscreenAdOpenedFlag = false; }