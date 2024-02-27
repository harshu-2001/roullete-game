using Commands;
using UnityEngine;

namespace Components
{
    public class GameRewardInput : MonoBehaviour
    {
        public GameCmdFactory gameCmdFactory;
        
        public void OnClick(string scene)
        {
            gameCmdFactory.RewardTurn(scene).Execute();
        }
    }
}
