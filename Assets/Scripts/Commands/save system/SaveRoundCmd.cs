using UnityEngine;
using UniRx;
using ViewModel;
using Infrastructure;

namespace Commands
{
    public class SaveRoundCmd : ICommand
    {
        private readonly CharacterTable characterTable;
        private ISaveRound saveRoundGateway;

        public SaveRoundCmd(CharacterTable characterTable, ISaveRound saveRoundGateway)
        {
            this.characterTable = characterTable;
            this.saveRoundGateway = saveRoundGateway;
        }

        public void Execute()
        {
            Round roundData = GetRoundData();

            saveRoundGateway.RoundSequentialSave(roundData)
                .Subscribe();

            
            // Debug.Log($"name");

            // DataSaver dataSaver = new DataSaver();
            GlobalConstants.Dts = new DataToSave
            {
                userName = GlobalConstants.Instance.DisplayName,
                coins = characterTable.characterMoney.characterMoney.Value + characterTable.characterMoney.characterBet.Value,
                email = GlobalConstants.Instance.Email
            };
            // Debug.Log($"{GlobalConstants.Dts.userName} name {GlobalConstants.Dts.totalCoins} coins {GlobalConstants.Dts.email}");
            RudderStackHelper.SaveDataFn();
            
        }

        private Round GetRoundData()
        {
            Table table = new Table()
            {
                TableChips = characterTable.currentTableInGame
            };

            string json = JsonUtility.ToJson(table);
            Round roundData = new Round()
            {
                idPlayer = GlobalConstants.Instance.Email,
                playerMoney = characterTable.characterMoney.characterMoney.Value + characterTable.characterMoney.characterBet.Value,
                playerTable = json
            };
            
            return roundData;
        }
    }
}
