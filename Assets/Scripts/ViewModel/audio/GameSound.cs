using UnityEngine;
using UniRx;

namespace ViewModel
{
    [CreateAssetMenu(fileName = "New Game Sound", menuName = "Scriptable/Game Sound")]
    public class GameSound : ScriptableObject
    {
        public BoolReactiveProperty isFxOn;
        public BoolReactiveProperty isMusicOn;
        public SimpleAudio[] audioReferences;
        public ISubject<SimpleAudio> OnSound = new Subject<SimpleAudio>();
        public ISubject<SimpleAudio> OnMusic = new Subject<SimpleAudio>();
    }
}
