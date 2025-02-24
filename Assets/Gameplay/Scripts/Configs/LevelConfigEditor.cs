using MatchEngine;
using UnityEditor;
using UnityEngine;

namespace Gameplay.Scripts.Configs
{
    [CustomEditor(typeof(LevelConfig))]
    public class LevelConfigEditor : Editor
    {
        private LevelConfig _levelConfig;

        private void OnEnable()
        {
            _levelConfig = (LevelConfig)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            SerializedProperty levelDatas = serializedObject.FindProperty("_levelDatas");
            EditorGUILayout.PropertyField(levelDatas, true);
            
            if (GUILayout.Button("Add New Level"))
            {
                AddNewLevel();
            }
            
            for (int i = 0; i < _levelConfig.GetLevelDatas().Count; i++)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField($"Level {i}", EditorStyles.boldLabel);

                LevelData levelData = _levelConfig.GetLevelDatas()[i];
                
                levelData.width = EditorGUILayout.IntField("Width", levelData.width);
                levelData.height = EditorGUILayout.IntField("Height", levelData.height);
                
                if (GUILayout.Button("Initialize Grid"))
                {
                    levelData.InitializeGrid();
                }
                
                if (levelData.initialLayout != null && levelData.initialLayout.Length == levelData.width * levelData.height)
                {
                    EditorGUILayout.LabelField("Tile Grid");
                    for (int y = 0; y < levelData.height; y++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        for (int x = 0; x < levelData.width; x++)
                        {
                            int index = y * levelData.width + x;
                            levelData.initialLayout[index] = (TileType)EditorGUILayout.EnumPopup(
                                levelData.initialLayout[index]
                            );
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void AddNewLevel()
        {
            LevelData newLevel = new LevelData();
            newLevel.InitializeGrid();
            _levelConfig.GetLevelDatas().Add(newLevel);
        }
    }
}