using System;
using UniRx;
using ViewModel;

namespace Infrastructure
{
    public interface ISaveRound 
    {
        IObservable<Unit> RoundSequentialSave(Round roundData);
        IObservable<Unit> RoundSequentialLoad();
        public Round roundData {get; set;}    
    }
}
