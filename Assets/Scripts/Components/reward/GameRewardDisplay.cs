using UnityEngine;
using UnityEngine.UI;
using ViewModel;
using UniRx;

namespace Components
{
    public class GameRewardDisplay : MonoBehaviour
    {
        public RewardFortune rewardFortune;
        public Text timerLabel;
        public Button rewardButton;
        
        void Start()
        {
            rewardFortune.isRewardPossible
                .Subscribe(OnReward)
                .AddTo(this);

            rewardFortune.rewardTimer
                .Subscribe(OnTimer)
                .AddTo(this);
        }

        private void OnTimer(string time)
        {
            timerLabel.text = time;
        }

        private void OnReward(bool isReward)
        {
            rewardButton.interactable = isReward;
        }
    }
}
