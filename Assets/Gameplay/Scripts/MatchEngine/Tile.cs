using Cysharp.Threading.Tasks;
using MatchEngine;
using MatchEngine.MatchAnimation;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Gameplay.Scripts.MatchEngine
{
    public class Tile : MonoBehaviour
    {
        [field: SerializeField] public TileType Type { get; private set; }
        public Vector2Int GridPosition { get; private set; }

        [SerializeField] private SpriteRenderer _spriteRenderer; 
        [SerializeField] private TileAnimator _tileAnimator;
        private float _fadeDuration = 0.1f;

        [Inject]
        public void Initialize(TileType type, Vector2Int position)
        {
            Type = type;
            GridPosition = position;
            switch (type)
            {
                case TileType.Blue:
                    _spriteRenderer.color = Color.blue;
                    break;
                case TileType.Green:
                    _spriteRenderer.color = Color.green;
                    break;
                case TileType.Red:
                    _spriteRenderer.color = Color.red;
                    break;
                case TileType.Yellow:
                    _spriteRenderer.color = Color.yellow;
                    break;
            }
        }
        
        
        public void SetAlpha(float alpha)
        {
            if (_spriteRenderer == null) return;
            Color color = _spriteRenderer.color;
            color.a = alpha;
            _spriteRenderer.color = color;
        }

        public async UniTask FadeInAnimation()
        {
            if (_spriteRenderer == null) return;

            float elapsedTime = 0f;
            while (elapsedTime < _fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(0f, 1f, elapsedTime / _fadeDuration);
                SetAlpha(alpha);
                await UniTask.Yield();
            }

            SetAlpha(1f);
        }

        public void SetGridPosition(Vector2Int newPosition)
        {
            GridPosition = newPosition;
        }

        public void ResetTile()
        {
            gameObject.SetActive(false);
        }

        public async UniTask RequestMove(Vector2Int targetPos)
        {
            await _tileAnimator.AnimateTile(this, targetPos);
        }

        public void RequestMatchAnimation()
        {
            _tileAnimator.PlayMatchAnimation(this);
        }
    }
}