using System.Collections;
using UnityEngine;

namespace Fading
{
    public abstract class Fader : MonoBehaviour
    {
        public Color Color { get; set; }
        public float Alpha { get; set; }

        public abstract void StartFadeInAnimation();
        public abstract void StartFadeOutAnimation();

        protected virtual IEnumerator FadeIn<T>(T objectWithColor)
        {
            for (float i = 0; i <= Alpha; i += Time.deltaTime)
            {
                var objectsColor = objectWithColor.GetType().GetProperty("color");
                objectsColor.SetValue(objectWithColor, new Color(Color.r, Color.g, Color.b, i));
                yield return null;
            }
        }

        protected virtual IEnumerator FadeOut<T>(T objectWithColor)
        {
            for (float i = Alpha; i >= 0; i -= Time.deltaTime)
            {
                var objectsColor = objectWithColor.GetType().GetProperty("color");
                objectsColor.SetValue(objectWithColor, new Color(Color.r, Color.g, Color.b, i));
                yield return null;
            }
        }
    }
}
