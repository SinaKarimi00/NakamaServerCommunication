using NakamaServerCommunication.RunTime.Main.Models;
using NakamaServerCommunication.RunTime.Main.Models.Confgis;
using UnityEditor;
using UnityEngine;

namespace NakamaServerCommunication.Editor
{
#if UNITY_EDITOR
    [CustomEditor(typeof(NakamaConfigModel))]
    public class NakamaConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var myComponent = (NakamaConfigModel)target;

            if (GUILayout.Button("Set local Values"))
            {
                myComponent.SetLocalValues();
            }

            if (GUILayout.Button("Set Stage Values"))
            {
                myComponent.SetStageValues();
            }

            if (GUILayout.Button("Set Product Values"))
            {
                myComponent.SetProductValues();
            }
        }
    }
#endif
}