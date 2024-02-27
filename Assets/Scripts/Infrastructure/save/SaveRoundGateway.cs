using System;
using UniRx;
using UnityEngine;
using ViewModel;
using System.Collections;
using Managers;
using System.IO;

namespace Infrastructure
{
    public class SaveRoundGateway : ISaveRound
    {
        private static protected readonly string FILE_NAME = "players.json";
        public Round roundData {get; set;}

        public IObservable<Unit> RoundSequentialSave(Round roundData)
        {
            return Observable.FromCoroutine<Unit>(observer => SavePlayer(observer, roundData));
        }

        public IObservable<Unit> RoundSequentialLoad()
        {
            return Observable.FromCoroutine<Unit>(observer => LoadPlayer(observer));
        }

        IEnumerator SavePlayer(IObserver<Unit> observer, Round roundData)
        {
            string path = GameManager.Instance.UrlDataPath + FILE_NAME;
            string json = JsonUtility.ToJson(roundData);

            File.WriteAllText(path, json);
            Debug.Log($"Saved data JSON with the table {roundData.idPlayer} with {json}");

            yield return new WaitUntil(() => File.Exists(path));
            GlobalConstants.CoinValue = roundData.playerMoney;
            observer.OnNext(Unit.Default); // push Unit or all buffer result.
            observer.OnCompleted();
        }

        IEnumerator LoadPlayer(IObserver<Unit> observer) 
        {
            string path = GameManager.Instance.UrlDataPath + FILE_NAME;
            string json = File.ReadAllText(path);

            yield return new WaitUntil(() => json != null);
            
            roundData = JsonUtility.FromJson<Round>(json);
            Debug.Log($"Loaded data JSON with the table {roundData.idPlayer} with {json}");
            GlobalConstants.CoinValue = roundData.playerMoney;
            observer.OnNext(Unit.Default); // push Unit or all buffer result.
            observer.OnCompleted();
        }
    }
}

