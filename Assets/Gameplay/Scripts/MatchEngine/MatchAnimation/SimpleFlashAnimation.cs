using DG.Tweening;
using Gameplay.Scripts.MatchEngine;

namespace MatchEngine.MatchAnimation
{
    public class SimpleFlashAnimation : IMatchAnimation
    {
        public void PlayAnimation(Tile tile)
        {
            tile.transform.DOShakeScale(0.3f, 0.2f);
        }
    }
}