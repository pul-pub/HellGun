mergeInto(LibraryManager.library, {
  ShowAdv: function() {
        ysdk.adv.showFullscreenAdv({
        callbacks: {
            onOpen: function() {
               myGameInstance.SendMessage('Adv', 'OnOpen');
            },
            onClose: function(wasShown) {
               myGameInstance.SendMessage('Adv', 'OnClose');
            },
            onError: function(error) {
               myGameInstance.SendMessage('Adv', 'OnError');
            },
            onOffline: function(error) {
               myGameInstance.SendMessage('Adv', 'OnOffline');
            }
         }
      })
   },

   ShowReward: function() {
        ysdk.adv.showRewardedVideo({
        callbacks: {
            onOpen: () => {
               myGameInstance.SendMessage('Adv', 'OnOpenReward');
            },
            onRewarded: () => {
               myGameInstance.SendMessage('Adv', 'OnRewarded');
            },
            onClose: () => {
               myGameInstance.SendMessage('Adv', 'OnCloseReward');
            }, 
            onError: (e) => {
               myGameInstance.SendMessage('Adv', 'OnErrorReward');
            }
        }
     })
   }
});