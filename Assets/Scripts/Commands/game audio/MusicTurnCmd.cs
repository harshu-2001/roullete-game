using ViewModel;

namespace Commands
{
    public class MusicTurnCmd : ICommand
    {
        private GameSound gameSound;
        private bool isOn;

        public MusicTurnCmd(GameSound gameSound, bool isOn)
        {
            this.isOn = isOn;
            this.gameSound = gameSound;
        }

        public void Execute()
        {
            gameSound.isMusicOn.Value = isOn;
            gameSound.isFxOn.Value = isOn;
        }
    }
}
