using System;
using System.IO;
using UnityEngine;
using ViewModel;
using System.Threading.Tasks;

namespace Components
{ 
    public static class Player
    {
        public static async Task CreatePlayer(CharacterTable characterTable, string tableName, string playerPath,int value)
        {   
            characterTable.tableName = tableName;
            characterTable.characterMoney.characterMoney.Value = value;
            
            GlobalConstants.CoinValue =value;
            if(!GlobalConstants.PlayerExists)
            {  
                Debug.Log($"Start save fn {value}");

                GlobalConstants.Dts = new DataToSave
                {
                    userName = GlobalConstants.Instance.DisplayName,
                    coins = GlobalConstants.CoinValue,
                    email = GlobalConstants.Instance.Email
                };
                
                RudderStackHelper.SaveDataFn();
            }

            
            if(!File.Exists(playerPath))
            {
                characterTable.OnSaveGame.OnNext(true);
                PlayerPrefs.SetString("LastRewardOpen", DateTime.Now.Ticks.ToString());
                PlayerPrefs.SetFloat("SecondsToWaitReward", 120);
            }

            await Task.Run(() => File.Exists(playerPath)); 
        }
    }
}
