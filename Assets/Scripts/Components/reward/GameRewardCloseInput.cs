using Commands;
using Managers;
using UnityEngine;

namespace Components
{
    public class GameRewardCloseInput : MonoBehaviour
    {
        public GameCmdFactory gameCmdFactory;
        
        public void OnClick()
        {
            GameManager.Instance.LoadScene("Game");
            GameManager.Instance.ToggleGame();
        }
    }
}
