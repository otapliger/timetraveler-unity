using UnityEditor;
using UnityEngine;
using UnityEditorInternal;

[CustomEditor(typeof(TimeMachine))]
public class TimeMachineInspector : Editor
{
	ReorderableList reorderableList;

	public void OnEnable()
	{
		var tagsToRewind = serializedObject.FindProperty("tagsToRewind");

		reorderableList = new ReorderableList(serializedObject, tagsToRewind, true, true, true, true);

		reorderableList.drawHeaderCallback += (rect) =>
		{
			EditorGUI.LabelField(rect, "Tags to Rewind");
		};

		reorderableList.drawElementCallback += (rect, index, isActive, isFocused) =>
		{
			rect.height *= 0.8f;
			rect.y += 1f;
			var tag = tagsToRewind.GetArrayElementAtIndex(index);
			EditorGUI.PropertyField(rect, tag, new GUIContent());
		};
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		EditorGUILayout.Space();
		EditorGUILayout.HelpBox("Rewind Time Limit: keep information from the last defined seconds.", MessageType.None);
		EditorGUILayout.Space();
		EditorGUILayout.IntSlider(serializedObject.FindProperty("rewindTimeLimit"), 0, 60);
		EditorGUILayout.Space();
		EditorGUILayout.HelpBox("Tags to Rewind: handle all game objects with those tags.", MessageType.None);
		EditorGUILayout.Space();
		reorderableList.DoLayoutList();
		EditorGUILayout.Space();

		serializedObject.ApplyModifiedProperties();
	}
}
