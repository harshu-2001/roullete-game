using Commands;
using UnityEngine;
using ViewModel;

namespace Components
{
    public class GamePlayInput : MonoBehaviour
    {
        public CharacterTable chartacterTable;
        public GameRoullete gameRoullete;
        public GameCmdFactory gameCmdFactory;
        
        public void OnClick()
        {
            gameCmdFactory.PlayTurn(chartacterTable, gameRoullete).Execute();
        }
    }
}
