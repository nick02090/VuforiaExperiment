using System.Collections.Generic;
using UnityEngine;

namespace Fading
{
    public class FaderController : MonoBehaviour
    {
        [SerializeField] private List<Fader> faders;

        public void AddFader(Fader fader) => faders.Add(fader);

        public void StartFadeInAnimation()
        {
            foreach (var fader in faders)
                if (fader.isActiveAndEnabled)
                    fader.StartFadeInAnimation();
        }

        public void StartFadeOutAnimation()
        {
            foreach (var fader in faders)
                if (fader.isActiveAndEnabled)
                    fader.StartFadeOutAnimation();
        }
    }
}