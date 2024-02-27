using UnityEngine;
using ViewModel;
using UniRx;

namespace Components
{
    public class CameraPlayDisplay : MonoBehaviour
    {
        public CharacterTable characterTable;
        public Animator mainCameraAnimator;

        void Start()
        {
            characterTable.OnRound
                .Subscribe(AnimateMainCamera)
                .AddTo(this);
        }

        public void AnimateMainCamera(bool isRound)
        {
            mainCameraAnimator.SetBool("Play", isRound);
        }
    }
}
