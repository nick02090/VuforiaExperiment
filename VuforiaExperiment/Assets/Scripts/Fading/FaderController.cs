using UnityEngine;

namespace Fading
{
    public class FaderController : MonoBehaviour
    {
        [SerializeField] private Fader[] faders;

        public void StartFadeInAnimation()
        {
            foreach (var fader in faders)
            {
                fader.StartFadeInAnimation();
            }
        }

        public void StartFadeOutAnimation()
        {
            foreach (var fader in faders)
                fader.StartFadeOutAnimation();
        }
    }
}