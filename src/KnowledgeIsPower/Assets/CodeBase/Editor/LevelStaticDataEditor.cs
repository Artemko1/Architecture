using System.Linq;
using CodeBase.Logic;
using CodeBase.Logic.Enemy.Spawner;
using CodeBase.StaticData;
using CodeBase.StaticData.Monsters;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Editor
{
    [CustomEditor(typeof(LevelStaticData))]
    public class LevelStaticDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Collect from current scene"))
            {
                Collect();
            }
        }

        private void Collect()
        {
            var levelData = (LevelStaticData)target;

            levelData.EnemySpawners =
                FindObjectsOfType<SpawnMarker>()
                    .Select(marker =>
                        new EnemySpawnerData(marker.GetComponent<UniqueId>().Id, marker.MonsterTypeId, marker.transform.position))
                    .ToList();

            string activeSceneName = SceneManager.GetActiveScene().name;
            levelData.LevelKey = activeSceneName;

            Vector3 initialPosition = GameObject.FindWithTag(Constants.Tags.InitialPoint).transform.position;
            levelData.InitialHeroPosition = initialPosition;

            EditorUtility.SetDirty(levelData);
        }
    }
}