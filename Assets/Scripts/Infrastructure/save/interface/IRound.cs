using System;
using UniRx;

namespace Infrastructure
{
    public interface IRound 
    {
        IObservable<Unit> PlayTurn();
        public int randomNumber {get; set;}

        // public int counter {get; set;}
    }
}
