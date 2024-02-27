using UnityEngine;
using UniRx;
using ViewModel;
using Infrastructure;

namespace Commands
{
    public class LoadRoundCmd : ICommand
    {
        private CharacterTable characterTable;
        private ISaveRound loadRoundGateway;

        public LoadRoundCmd(CharacterTable characterTable, ISaveRound loadRoundGateway)
        {
            this.characterTable = characterTable;
            this.loadRoundGateway = loadRoundGateway;
        }

        public void Execute()
        {
            loadRoundGateway.RoundSequentialLoad()
                .Do(_ => characterTable.characterMoney.characterMoney.Value = loadRoundGateway.roundData.playerMoney)
                .Do(_ => LoadTable(loadRoundGateway.roundData))
                .Subscribe();
            // GlobalConstants.CoinValue =loadRoundGateway.roundData.playerMoney;
        }

        void LoadTable(Round roundData)
        {
            Table table = JsonUtility.FromJson<Table>(roundData.playerTable);
            Debug.Log($"Loading current player table {roundData.playerTable}");
            characterTable.OnRestoreTable.OnNext(table);
        }
    }
}
