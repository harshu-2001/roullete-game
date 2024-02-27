using Commands;
using UnityEngine;
using ViewModel;

namespace Components
{
    public class GameUndoInput : MonoBehaviour
    {
        public CharacterTable characterTable;
        public GameCmdFactory gameCmdFactory;
        public void OnClick() 
        {
            gameCmdFactory.UndoTableTurn(characterTable).Execute();
        }
    }
}
