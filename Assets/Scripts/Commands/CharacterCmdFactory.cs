using Infrastructure;
using Managers;
using UnityEngine;
using ViewModel;

namespace Commands
{
    [CreateAssetMenu(fileName = "New CharacterCmdFactory", menuName = "Factory/Character Command Factory")]
    public class CharacterCmdFactory : ScriptableObject
    {  
        // Player system      
        public LoadRoundCmd LoadPlayer(CharacterTable characterTable)
        {
            return new LoadRoundCmd(characterTable, new SaveRoundGateway());
        }        
        public SaveRoundCmd SavePlayer(CharacterTable characterTable)
        {   
            return new SaveRoundCmd(characterTable, new SaveRoundGateway());
        }        
        public SaveCashTurnCmd SaveCash(CharacterCmdFactory characterCmdFactory, CharacterTable characterTable, int payment,GameState gameState)
        {
            return new SaveCashTurnCmd(characterCmdFactory, characterTable, payment, new SaveRoundGateway(),gameState);
        }        
    }
}
