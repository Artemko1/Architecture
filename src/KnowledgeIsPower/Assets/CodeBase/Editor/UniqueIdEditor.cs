using System;
using System.Linq;
using CodeBase.Logic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace CodeBase.Editor
{
    [CustomEditor(typeof(UniqueId))]
    public class UniqueIdEditor : UnityEditor.Editor
    {
        private void OnEnable()
        {
            var uniqueId = (UniqueId)target;

            if (IsPrefabAsset(uniqueId!.gameObject)) return;

            if (string.IsNullOrWhiteSpace(uniqueId.Id))
            {
                Generate(uniqueId);
            }
            else
            {
                UniqueId[] uniqueIds = FindObjectsOfType<UniqueId>();
                if (uniqueIds.Any(other => other != uniqueId && other.Id == uniqueId.Id))
                {
                    Generate(uniqueId);
                }
            }
        }

        private static bool IsPrefabAsset(GameObject uniqueId) =>
            IsInPrefabEditingWorkflow(out PrefabStage currentPrefabStage)
                ? currentPrefabStage.IsPartOfPrefabContents(uniqueId)
                : PrefabUtility.IsPartOfPrefabAsset(uniqueId);

        private static bool IsInPrefabEditingWorkflow(out PrefabStage currentPrefabStage)
        {
            currentPrefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            return currentPrefabStage != null;
        }

        private static void Generate(UniqueId uniqueId)
        {
            uniqueId.Id = $"{uniqueId.gameObject.scene.name}_{Guid.NewGuid().ToString()}";

            if (Application.isPlaying) return;

            EditorUtility.SetDirty(uniqueId);
            EditorSceneManager.MarkSceneDirty(uniqueId.gameObject.scene);
        }
    }
}