﻿using CodeBase.Data;
using CodeBase.StaticData;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Editor
{
    [CustomEditor(typeof(PlayerProgressStaticData))]
    public class PlayerProgressStaticDataEditor : UnityEditor.Editor
    {
        private const string InitialPointTag = "InitialPoint"; // todo вынести в общие константы

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
            var playerProgressStaticData = (PlayerProgressStaticData)target;

            playerProgressStaticData.PositionOnLevel.LevelName = SceneManager.GetActiveScene().name;

            Vector3 initialPosition = GameObject.FindWithTag(InitialPointTag).transform.position;
            playerProgressStaticData.PositionOnLevel.Position = initialPosition.AsVector3Data();

            EditorUtility.SetDirty(playerProgressStaticData);
        }
    }
}