using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Scripts.MatchEngine;
using UnityEngine;

namespace MatchEngine.MatchAnimation
{
    public class TileAnimator : MonoBehaviour
    {
        private readonly float animationSpeed = 0.3f;
        private readonly Match3Grid grid;
        private Dictionary<TileType, IMatchAnimation> matchAnimations;

        public void Start()
        {
            matchAnimations = new Dictionary<TileType, IMatchAnimation>
            {
                { TileType.Red, new SimpleFlashAnimation() },
                { TileType.Blue, new ExplosiveEffectAnimation() },
                { TileType.Green, new SimpleFlashAnimation() },
                { TileType.Yellow, new ExplosiveEffectAnimation() }
            };
        }

        public async UniTask AnimateTile(Tile tile, Vector2Int targetPos)
        {
            Debug.Log($"Animating tile from {tile.GridPosition} to {targetPos}");
            Vector3 targetWorldPos = new Vector3(targetPos.x, targetPos.y, 0);
            await tile.transform.DOMove(targetWorldPos, animationSpeed)
                .SetEase(Ease.OutBack).AsyncWaitForCompletion();

            tile.SetGridPosition(targetPos);
            Debug.Log($"Tile moved to {targetPos}");
        }

        public void PlayMatchAnimation(Tile tile)
        {
            
            if (matchAnimations.TryGetValue(tile.Type, out var animation))
            {
                animation.PlayAnimation(tile);
            }
        }
    }
}