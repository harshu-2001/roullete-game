using System.Collections;
using UnityEngine;
using UniRx;
using ViewModel;
using Controllers;
using Infrastructure;
using System;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using Managers;

namespace Commands
{
    public class PlayTurnCmd : ICommand
    {
        private readonly MonoBehaviour monoBehaviour;
        private CharacterTable characterTable;
        private GameRoullete gameRoullete;
        private IRound roundGateway;
        private IPayment paymentGateway;

        static int counter =0 ;
        int winNumber ;

        int index;

        public PlayTurnCmd(MonoBehaviour monoBehaviour, CharacterTable characterTable, GameRoullete gameRoullete, IRound roundGateway, IPayment paymentGateway)
        {
            this.monoBehaviour = monoBehaviour;
            this.characterTable = characterTable;
            this.gameRoullete = gameRoullete;
            this.roundGateway = roundGateway;
            this.paymentGateway = paymentGateway;
        }

        public void Execute()
        {
            if(characterTable.currentTableCount <= 0)
                return;
            
            Debug.Log($"The game roullete is executing in {characterTable.tableName} with {characterTable.currentTableCount} chips in table!");

            PlayerSound.Instance.gameSound.OnSound.OnNext(PlayerSound.Instance.gameSound.audioReferences[7]);

            Dictionary<string,object> eventProperties = new Dictionary<string, object>();
            

            counter++;
            Debug.Log($"Counter : {counter}");
           if (counter == 3){
                
                counter = 0;
                Debug.Log($"{counter} value Execute");

                var variable =characterTable.currentTable[0]._chipRuntime.currentButton;

                 if((variable.name == "E1_Eightteen_3") || (variable.name == "E2_Red")||(variable.name == "E2_Black")||(variable.name == "E_Even")||(variable.name == "E_Odd") ||( variable.name == "E1_Eightteen_2" )|| (variable.name == "E1_Eightteen_1"))
                {  
                    index= Random.Range(0,18);
                    winNumber = variable.buttonValue[index];
                }
                else if(variable.name == "Number_0")
                {
                    winNumber = 0;
                }
                else if((variable.name == "Dozen_1") || (variable.name == "Dozen_2")||(variable.name == "Dozen_3")|| (variable.name == "Column_1") || (variable.name == "Column_2") || (variable.name == "Column_3"))
                {
                    index = Random.Range(0,12);
                    winNumber = variable.buttonValue[index];
                }
                else{
                    winNumber = variable.buttonValue[0];
                }
              
                roundGateway.PlayTurn()
                .Do(_ => monoBehaviour.StartCoroutine(RoulleteGame(winNumber,GameState.PLAY)))
                .Do(_ => characterTable.lastNumber = winNumber)
                .Subscribe(); 
           }
           else {

                // Debug.Log($"value : {roundGateway.randomNumber} and winnumber = {winNumber}");
                               
                var playturn = roundGateway.PlayTurn();
                winNumber=roundGateway.randomNumber;
                // Debug.Log($"value : {roundGateway.randomNumber} and winnumber = {winNumber}");

                playturn
                .Do(_ => monoBehaviour.StartCoroutine(RoulleteGame(winNumber,GameState.PLAY)))
                .Do(_ => characterTable.lastNumber = winNumber)
                .Subscribe();
            }

            eventProperties.Add("randomNumber", winNumber);
            eventProperties.Add("characterTableChips",characterTable.currentTable);
            // eventProperties.Add("characterTableNumbers",characterTable.currentNumbers);
            
            RudderStackHelper.SendTrackEvent("PlayGame",eventProperties);       
            }
        
        IEnumerator RoulleteGame(int num,GameState gameState)
        {
            characterTable.OnRound.OnNext(true); // Initialize round
            characterTable.currentTableActive.Value = false; // Desactivete table buttons
            gameRoullete.OnRotate.OnNext(true);

            yield return new WaitForSeconds(2.0f);
            gameRoullete.currentSpeed = 75f;
            yield return new WaitForSeconds(1.0f);
            gameRoullete.currentSpeed = 145f;
            PlayerSound.Instance.gameSound.OnSound.OnNext(PlayerSound.Instance.gameSound.audioReferences[9]);
            yield return new WaitForSeconds(0.5f);
            gameRoullete.currentSpeed = 240f;
            yield return new WaitForSeconds(1.2f);
            gameRoullete.currentSpeed = 245f;
            yield return new WaitForSeconds(2.0f);
            gameRoullete.currentSpeed = 265;
            yield return new WaitForSeconds(3.8f);
            gameRoullete.currentSpeed = 245;
            yield return new WaitForSeconds(1.5f);
            gameRoullete.currentSpeed = 240f;
            yield return new WaitForSeconds(1.5f);
            // Ball position
            gameRoullete.currentSpeed = 145;
            gameRoullete.OnNumber.OnNext(num);

            yield return new WaitForSeconds(1.8f);
            gameRoullete.currentSpeed = 75f;
   
            yield return new WaitForSeconds(5.0f);
            // Finish round
            gameRoullete.currentSpeed = gameRoullete.defaultSpeed;
            characterTable.OnRound.OnNext(false); 

            // Intialize the payment system and display the news values
            paymentGateway.PaymentSystem(characterTable)
                .Delay(TimeSpan.FromSeconds(3))
                .Do(_ => OnPayment(paymentGateway.PaymentValue,gameState))
                .Do(_ => characterTable.OnWinButton.OnNext(num))
                .Subscribe();
        }

        public void OnPayment(int value,GameState gameState)
        {
            characterTable.characterMoney.currentPayment.Value = value;
            characterTable.characterMoney.PaymentSystem(value,gameState);
        }
    }
}
