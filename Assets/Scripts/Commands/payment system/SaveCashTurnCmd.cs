using UnityEngine;
using UniRx;
using ViewModel;
using Infrastructure;
using Managers;

namespace Commands
{
    public class SaveCashTurnCmd : ICommand
    {
        private CharacterCmdFactory characterCmdFactory;
        private CharacterTable characterTable;
        private int payment;
        private ISaveRound saveRoundGateway;
        private GameState _GameState;

        public SaveCashTurnCmd(CharacterCmdFactory characterCmdFactory, CharacterTable characterTable, int payment, ISaveRound saveRoundGateway, GameState gameState)
        {
            this.characterCmdFactory = characterCmdFactory;
            this.characterTable = characterTable;
            this.payment = payment;
            this.saveRoundGateway = saveRoundGateway;
            this._GameState = gameState;
        }

        public void Execute()
        {
            // First Load Player Table
            saveRoundGateway.RoundSequentialLoad()
                .Do(_ => characterTable.characterMoney.characterMoney.Value = saveRoundGateway.roundData.playerMoney)
                .Do(_ => UpdateTable(payment))
                .Do(_ => characterCmdFactory.SavePlayer(characterTable).Execute())
                .Do(_ => characterTable.currentTableInGame.Clear())
                .Subscribe();
            // GlobalConstants.CoinValue = saveRoundGateway.roundData.playerMoney;
        }

        private void UpdateTable(int payment)
        {
            Table tableLoaded = JsonUtility.FromJson<Table>(saveRoundGateway.roundData.playerTable);
            characterTable.currentTableInGame = tableLoaded.TableChips;
            characterTable.characterMoney.PaymentSystem(payment,_GameState);
        }
    }
}
