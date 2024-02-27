using UnityEngine;
using UniRx;
using Managers;
using System.Collections.Generic;


namespace ViewModel
{
    [CreateAssetMenu(fileName = "New Characters", menuName = "Scriptable/Character Money")]
    public class CharacterMoney : ScriptableObject
    {
        public IntReactiveProperty characterBet = new IntReactiveProperty();
        public IntReactiveProperty characterMoney = new IntReactiveProperty();
        public IntReactiveProperty currentPayment = new IntReactiveProperty();

        private GameState gameState1;

        // Operations in player money
        void AddCash(int cashWinner,GameState gameState)
        {
            int aux = characterMoney.Value;
            characterMoney.Value += cashWinner;
            
            if(gameState1 == GameState.DELETE)
            {  
                //  Debug.Log($"character Global : {GlobalConstants.CoinValue} and cashwinner: {cashWinner} diff : {GlobalConstants.CoinValue + cashWinner}");
                GlobalConstants.CoinValue +=cashWinner; 
                return;
            }
            // Debug.Log($"character Game: {GlobalConstants.CoinValue}");
            GlobalConstants.CoinValue = characterMoney.Value;
            
            if(gameState == GameState.REWARD){
                Dictionary<string,object> eventProperties = new Dictionary<string,object>();
                eventProperties.Add("reward",cashWinner);
                eventProperties.Add("coins",aux);
                // eventProperties.Add("finalAmount",GlobalConstants.CoinValue);
                RudderStackHelper.SendTrackEvent("Spin wheel reward",eventProperties);
            }
            else if(gameState == GameState.PLAY){
                Dictionary<string,object> eventProperties = new()
                {
                    { "coinsWin", cashWinner },
                    { "coins", aux }
                };
                // eventProperties.Add("finalAmount",GlobalConstants.CoinValue);
                RudderStackHelper.SendTrackEvent("Won",eventProperties);
                GlobalConstants.Dts = new DataToSave
                {
                    userName = GlobalConstants.Instance.DisplayName,
                    coins = GlobalConstants.CoinValue,
                    email = GlobalConstants.Instance.Email
                };
                // Debug.Log($"{GlobalConstants.Dts.userName} name {GlobalConstants.Dts.totalCoins} coins {GlobalConstants.Dts.email}");
                RudderStackHelper.SaveDataFn();               


            }
            
            
        }
        
        void SubstractCash(int cashLost)
        {   int aux = characterMoney.Value;
            if(cashLost < 0) 
            {
                cashLost = cashLost * -1;
            }

            characterMoney.Value -= cashLost;

            if (characterMoney.Value < 0)
            {
                characterMoney.Value = 0;
            }            

        }

        // Operations in player bet
        void AddBet(int betSum)
        {
            characterBet.Value  += betSum;
        }
        void SubstractBet(int betRest)
        {  
            //  Debug.Log($"character Bet : {characterBet.Value} and bet rest : {betRest} and Difference : {characterBet.Value-betRest}  Global coin value : {GlobalConstants.CoinValue} and diff: {GlobalConstants.CoinValue-characterBet.Value}");  
            characterBet.Value  -= betRest;
            GlobalConstants.CoinValue -= betRest ;
        }

        // Public methods   
        public bool CheckBetValue(int valueFicha)
        {
            // Check if the bet is possible
            bool aux = true;
            if (valueFicha <= characterMoney.Value  && valueFicha != 0)
            {
                aux = true;
                SubstractCash(valueFicha);
                AddBet(valueFicha);
            }
            else
            {
                aux = false;
            }
            return aux;
        }
        public void DeleteChip(int valueFicha, GameState gameState)
        {   gameState1 = gameState;

            // Delete ficha of the table
            SubstractBet(valueFicha);
            AddCash(valueFicha,gameState);
        }
        public void PaymentSystem(int payment,GameState gameState)
        {    gameState1=gameState;
            characterBet.Value = 0;

            // If the player win when the round finish game will pay.
            // If not win it will stay with the same money without the bet.
            if(payment > 0)
                AddCash(payment,gameState);
                
            Debug.Log($"Character player money is now being refresh with payment {payment}!");
        }
    }
}
