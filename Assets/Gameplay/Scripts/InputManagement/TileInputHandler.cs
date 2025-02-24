using System;
using Gameplay.Scripts.MatchEngine;
using Lean.Touch;
using MatchEngine;
using UnityEngine;

namespace Gameplay.Scripts.InputManagement
{
    public class TileInputHandler : MonoBehaviour
    {
        [SerializeField] private Match3Grid _match3Grid;
        private Tile _firstTile;
        private Tile _secondTile;
        
        private void Start()
        {
            LeanTouch.OnFingerDown += Touch;
            LeanTouch.OnFingerUp += LeanTouchOnOnFingerUp;
        }

        private void LeanTouchOnOnFingerUp(LeanFinger obj)
        {
            if (Physics.Raycast(obj.GetRay(Camera.main),out RaycastHit raycastHit))
            {
                if (raycastHit.collider.TryGetComponent(out Tile tile))
                {
                    _secondTile = tile;

                    _match3Grid.SwapTiles(_firstTile, _secondTile);
                    _firstTile = null;
                    _secondTile = null;
                }
            }
        }

        private void Touch(LeanFinger obj)
        {
            if (Physics.Raycast(obj.GetRay(Camera.main),out RaycastHit raycastHit))
            {
                if (raycastHit.collider.TryGetComponent(out Tile tile))
                {
                    _firstTile = tile;
                }
            }
        }
    }
}