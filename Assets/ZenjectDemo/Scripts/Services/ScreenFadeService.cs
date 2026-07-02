using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ZenjectDemo
{
    public class ScreenFadeService : MonoBehaviour, IScreenFadeService
    {
        [SerializeField] private Image _panel;
        
        [SerializeField] private float _showDuration = 0.5f;
        [SerializeField] private float _hideDuration = 0.5f;
        
        private Coroutine _fadeCoroutine;
        
        private void Awake()
        {
            SetAlpha(0f);
        }
        
        public Task ShowAsync(CancellationToken cancellationToken) => FadeAsync(1f, _showDuration, cancellationToken);
        
        public Task HideAsync(CancellationToken cancellationToken) => FadeAsync(0f, _hideDuration, cancellationToken);
        
        private Task FadeAsync(float targetAlpha, float duration, CancellationToken cancellationToken)
        {
            if (_panel == null)
                return Task.CompletedTask;
            
            if (_fadeCoroutine != null)
                StopCoroutine(_fadeCoroutine);
            
            var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            
            if (cancellationToken.CanBeCanceled)
            {
                cancellationToken.Register(() =>
                {
                    if (_fadeCoroutine != null)
                    {
                        StopCoroutine(_fadeCoroutine);
                        _fadeCoroutine = null;
                    }
                    
                    tcs.TrySetCanceled(cancellationToken);
                });
            }
            
            _fadeCoroutine = StartCoroutine(FadeCoroutine(targetAlpha, duration, tcs, cancellationToken));
            return tcs.Task;
        }
        
        private IEnumerator FadeCoroutine(float targetAlpha, float duration, TaskCompletionSource<bool> tcs, CancellationToken cancellationToken)
        {
            var startAlpha = _panel.color.a;
            var elapsed = 0f;
            
            if (duration <= 0f)
            {
                SetAlpha(targetAlpha);
                _fadeCoroutine = null;
                tcs.TrySetResult(true);
                yield break;
            }
            
            while (elapsed < duration)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    _fadeCoroutine = null;
                    tcs.TrySetCanceled(cancellationToken);
                    yield break;
                }
                
                elapsed += Time.unscaledDeltaTime;
                var t = Mathf.Clamp01(elapsed / duration);
                
                SetAlpha(Mathf.Lerp(startAlpha, targetAlpha, t));
                yield return null;
            }
            
            SetAlpha(targetAlpha);
            _fadeCoroutine = null;
            tcs.TrySetResult(true);
        }
        
        private void SetAlpha(float alpha)
        {
            var color = _panel.color;
            color.a = alpha;
            _panel.color = color;
        }
    }
}