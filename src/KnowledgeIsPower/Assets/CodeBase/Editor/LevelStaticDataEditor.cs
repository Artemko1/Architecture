﻿using System.Linq;
using CodeBase.Enemy.Spawner;
using CodeBase.Infrastructure.StaticData;
using CodeBase.Logic;
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

            if (GUILayout.Button("Collect"))
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

            levelData.LevelKey = SceneManager.GetActiveScene().name;
            EditorUtility.SetDirty(levelData);
        }
    }
}