using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Infrastructure
{
    public class PlayRoundGateway : IRound
    {
        public int randomNumber { get; set; }
        public IObservable<Unit> PlayTurn()
        {   
            randomNumber = Random.Range(0, 37);
            
            return Observable.Return(Unit.Default)
                    .Do(_ => Debug.Log($"Generating number {randomNumber} for the roullete game round!"));
        } 
    }
}

