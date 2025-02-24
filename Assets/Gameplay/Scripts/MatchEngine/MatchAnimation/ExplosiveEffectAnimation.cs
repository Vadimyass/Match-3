using DG.Tweening;
using Gameplay.Scripts.MatchEngine;
using UnityEngine;

namespace MatchEngine.MatchAnimation
{
    public class ExplosiveEffectAnimation : IMatchAnimation
    {
        public void PlayAnimation(Tile tile)
        {
            tile.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() => tile.ResetTile());
        }
    }
}