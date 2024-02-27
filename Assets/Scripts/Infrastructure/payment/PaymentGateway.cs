using System.Collections.Generic;
using UnityEngine;
using ViewModel;
using UniRx;
using Components;
using System.Linq;
using System;

namespace Infrastructure
{
    public class PaymnentGateway : IPayment
    {
        // Payment Controller System
        // He actived when the roullete is finished and finded the number winner
        // Calculate the bet with the number winner and the equation of payment
        private int _payment;
        private int _number;
        
        public int PaymentValue
        {
            get {return _payment;}
            set {_payment = value;}
        }

        public IObservable<Unit> PaymentSystem(CharacterTable characterTable)
        {
            Debug.Log($"Payment system is being executed in {characterTable.tableName}");
            _payment = 0;
            _number = characterTable.lastNumber;

            var _chipsWinner = characterTable.currentTable.Where(chip => chip.HasNumber(_number));
            var _chipsLosted = characterTable.currentTable.Where(chip => !chip.HasNumber(_number));

            int paymentWin = PaymentHandler.CalculateEarnedPayment(_chipsWinner.ToArray());
            int paymentLost = PaymentHandler.GetPaymentBack(_chipsLosted.ToArray());
            int paymentChipsReturn = PaymentHandler.GetPaymentBack(_chipsWinner.ToArray());

            _payment = paymentWin - paymentLost;
            _payment = _payment + paymentChipsReturn;

            if(paymentWin<=0){
                Dictionary<string,object> eventProperties = new Dictionary<string,object>();
                eventProperties.Add("coinsLost",paymentLost);
                eventProperties.Add("coins",GlobalConstants.CoinValue);
                GlobalConstants.CoinValue-=paymentLost;
                // eventProperties.Add("finalAmount",GlobalConstants.CoinValue);
                RudderStackHelper.SendTrackEvent("Lost",eventProperties);
                GlobalConstants.Dts = new DataToSave
                {
                    userName = GlobalConstants.Instance.DisplayName,
                    coins = GlobalConstants.CoinValue,
                    email = GlobalConstants.Instance.Email
                };
                // Debug.Log($"{GlobalConstants.Dts.userName} name {GlobalConstants.Dts.totalcoins} coins {GlobalConstants.Dts.email}");
                RudderStackHelper.SaveDataFn();

            }

            return Observable.Return(Unit.Default)
                .Do(_ => Debug.Log($"Win: {paymentWin}, Lost: {paymentLost}, Chips: {paymentChipsReturn}"))
                .Do(_ => Debug.Log($"The roullete pay: {_payment}"));
        }
    }

    public static class PaymentHandler
    {
        // Calculate and return the all values of payment.
        public static int CalculateEarnedPayment(ChipGame[] chips)
        {
            int earnedPayment = 0;

            IEnumerable<ChipGame> plenos =  chips.Where(chip => chip._chipRuntime.currentButton.isPleno);
            IEnumerable<ChipGame> middles =  chips.Where(chip => !chip._chipRuntime.currentButton.isPleno);

            int paymentPleno = GetPaymentChips(plenos.ToArray());
            int paymentMiddle = GetPaymentChips(middles.ToArray());

            earnedPayment = paymentPleno + paymentMiddle;
            return earnedPayment;
        }

        // Calculate returned payment of chips
        public static int GetPaymentBack(ChipGame[] chips)
        {
            int total = 0;

            foreach(ChipGame chip in chips)
            {
                int value = chip._chipRuntime.currentChipData.chipValue;
                total = total + value;
            }

            return total;
        }
    
        // Calculate the payment of middle or pleno with equation
        public static int GetPaymentChips(ChipGame[] chips)
        {
            int total = 0;
            foreach (ChipGame chip in chips)
            {
                int value = EquationRoullete.EquationPayment(chip._chipRuntime.currentButton.buttonValue.Count(), chip._chipRuntime.currentChipData.chipValue);
                // int n = chip._chipRuntime.currentButton.buttonValue.Count();
                // int multiply = (36 - n) / n;
                
                // // Payment
                // int payment = chip._chipRuntime.currentChipData.chipValue * multiply;
                // Debug.Log($"value : {value} chip quantity : {chip._chipRuntime.currentButton.buttonValue.Count()} chip value {chip._chipRuntime.currentChipData.chipValue} payment : {payment} multiply {multiply}");
                total += value;
            }
            return total;
        }
    }
}
