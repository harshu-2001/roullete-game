using ViewModel;

namespace Commands
{
    public class FortunePlayCmd : ICommand
    {
        private RewardFortune rewardFortune;

        public FortunePlayCmd(RewardFortune rewardFortune)
        {
            this.rewardFortune = rewardFortune;
        }

        public void Execute()
        {
            rewardFortune.OnFortune.OnNext(true);
            rewardFortune.OpenReward();
        }
    }
}
