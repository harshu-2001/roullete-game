using UnityEngine;
using Controllers;
using Managers;
using System.Collections.Generic;

namespace Commands
{
    public class RewardSceneOpenCmd : ICommand
    {
        private string rewardScene;

        public RewardSceneOpenCmd(string rewardScene)
        {
            this.rewardScene = rewardScene;
        }

        public void Execute()
        {
            PlayerSound.Instance.gameSound.OnSound.OnNext(PlayerSound.Instance.gameSound.audioReferences[5]);
            Debug.Log($"Opening game reward to get more money!");
            GameManager.Instance.ToggleRewardSystem();
            GameManager.Instance.LoadScene(rewardScene);
            
            // Dictionary<string,object> screenprperties = new Dictionary<string, object>();
            // screenprperties.Add("email", GlobalConstants.Instance.Email);
            // RudderStackHelper.SendScreenEvent("Spin Wheel",screenprperties);

        }
    }
}
