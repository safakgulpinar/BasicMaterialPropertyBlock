using System.Collections;
using UnityEngine;

namespace _BasicMPB.Scripts
{
    public class MPBController : MonoBehaviour
    {
        private Renderer _renderer = null;
        private MaterialPropertyBlock _materialPropertyBlock = null;
        private bool _isAnimating = false;
        private Color _originalColor;
        private IEnumerator _animateColorCoroutine;
        private IEnumerator _colorPulseCoroutine;
        
        //This is the ID of the color property in the shader
        private static readonly int Color1 = Shader.PropertyToID("_Color");

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _materialPropertyBlock = new MaterialPropertyBlock();
            //Or sharedMaterial if you want to change the color of the material directly
            _originalColor = _materialPropertyBlock.GetColor(Color1);
        }

        private void OnDisable()
        {
            if (!_isAnimating) return;
            StopAllCoroutines();
            ResetColor();
        }

        //Use this method is used to set the color of the material directly
        public void SetColor(Color color)
        {
            _renderer.GetPropertyBlock(_materialPropertyBlock);
            _materialPropertyBlock.SetColor(Color1, color);
            _renderer.SetPropertyBlock(_materialPropertyBlock);
        }
        //Use this method is used to animate the color of the material
        public void AnimateColor(Color targetColor, float duration)
        {
            if (_isAnimating) return;
            
            if(_animateColorCoroutine != null)
            {
                StopCoroutine(_animateColorCoroutine);
                _animateColorCoroutine = null;
            }
            StartCoroutine(_animateColorCoroutine = AnimateColorCoroutine(targetColor, duration));
        }
        public void StartColorPulse(Color colorA, Color colorB, float cycleDuration, int loops)
        {
            if (_isAnimating) return;
            
            if(_colorPulseCoroutine != null)
            {
                StopCoroutine(_colorPulseCoroutine);
                _colorPulseCoroutine = null;
            }
            StartCoroutine(_colorPulseCoroutine = ColorPulseCoroutine(colorA, colorB, cycleDuration, loops));
        }
        
        private IEnumerator AnimateColorCoroutine(Color targetColor, float duration)
        {
            _isAnimating = true;
            var initialColor = _materialPropertyBlock.GetColor(Color1);
            var timer = 0.0f;

            while (timer < duration)
            {
                var progress = timer / duration;
                var newColor = Color.Lerp(initialColor, targetColor, progress);
                SetColor(newColor);

                timer += Time.deltaTime;
                yield return null;
            }

            SetColor(targetColor);
            _isAnimating = false;
        }
        
        private IEnumerator ColorPulseCoroutine(Color colorA, Color colorB, float cycleDuration, int loops)
        {
            _isAnimating = true;
            var currentLoop = 0;

            while (currentLoop < loops)
            {
                yield return StartCoroutine(ColorTransition(colorA, colorB, cycleDuration / 2));
                yield return StartCoroutine(ColorTransition(colorB, colorA, cycleDuration / 2));
                currentLoop++;
            }

            SetColor(colorA); // Ensure final color is reset to original
            _isAnimating = false;
        }

        private IEnumerator ColorTransition(Color startColor, Color endColor, float duration)
        {
            var timer = 0.0f;
            while (timer < duration)
            {
                var progress = timer / duration;
                var newColor = Color.Lerp(startColor, endColor, progress);
                SetColor(newColor);
                timer += Time.deltaTime;
                yield return null;
            }
            SetColor(endColor);
        }
        
        private void ResetColor()
        {
            SetColor(_originalColor);
        }
    }
}
