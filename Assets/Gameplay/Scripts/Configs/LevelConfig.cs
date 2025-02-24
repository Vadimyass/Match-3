using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Scripts.Configs
{
    [CreateAssetMenu(fileName = "NewLevelConfig", menuName = "Match3/LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private List<LevelData> _levelDatas;

        
        public List<LevelData> GetLevelDatas()
        {
            return _levelDatas;
        }
        
        public LevelData GetLevelData(int levelIndex)
        {
            return _levelDatas[levelIndex];
        }
        
    }
}