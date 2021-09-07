using System.Collections;
using UnityEngine;

namespace Fading
{
    public abstract class Fader : MonoBehaviour
    {
        public abstract void StartFadeInAnimation();
        public abstract void StartFadeOutAnimation();

        protected virtual IEnumerator FadeIn<T>(T objectWithColor, float alpha, Color color)
        {
            for (float i = 0; i <= alpha; i += Time.deltaTime)
            {
                var objectsColor = objectWithColor.GetType().GetProperty("color");
                objectsColor.SetValue(objectWithColor, new Color(color.r, color.g, color.b, i));
                yield return null;
            }
        }

        protected virtual IEnumerator FadeOut<T>(T objectWithColor, float alpha, Color color)
        {
            for (float i = alpha; i >= 0; i -= Time.deltaTime)
            {
                var objectsColor = objectWithColor.GetType().GetProperty("color");
                objectsColor.SetValue(objectWithColor, new Color(color.r, color.g, color.b, i));
                yield return null;
            }
        }
    }
}
