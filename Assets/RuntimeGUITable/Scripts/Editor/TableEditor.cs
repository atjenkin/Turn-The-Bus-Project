using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
using System.Linq;

namespace UnityUITable
{

	[CustomEditor(typeof(Table), true)]
	public class TableEditor : Editor
	{

		static readonly Color WARNING_COLOR = new Color(1f, 0.9f, 0.2f);

		SerializedProperty columns;
		SerializedProperty rowsColorMode;
		SerializedProperty bgColor;
		SerializedProperty altBgColor;
		SerializedProperty lineColor;
		SerializedProperty hasTitles;
		SerializedProperty titleBGColor;
		SerializedProperty titleFontColor;
		SerializedProperty titleFontSize;
		SerializedProperty titleFont;
		SerializedProperty titleFontStyle;
		SerializedProperty rowHeight;
		SerializedProperty titleHeightMode;
		SerializedProperty titleHeight;
		SerializedProperty spacing;
		SerializedProperty targetCollection;
		SerializedProperty rowDeleteButtons;
		SerializedProperty deleteColumnWidth;
		SerializedProperty deleteCellStyle;
		SerializedProperty rowAddButton;
		SerializedProperty addCellStyle;
		SerializedProperty selectableRows;
		SerializedProperty multiSelection;
		SerializedProperty selectedBgColor;
		SerializedProperty updateCellStyleAtRuntime;
		SerializedProperty updateCellContentAtRuntime;
		SerializedProperty limitRowsInEditMode;
		SerializedProperty nbRowsInEditMode;
		SerializedProperty preinstantiateRowsOverLimit;
		SerializedProperty horizontal;
		SerializedProperty sortingState;
		SerializedProperty secondarySortingState;
		SerializedProperty sortingStateIndex;
		SerializedProperty secondarySortingStateIndex;
		SerializedProperty sortingStateMode;
		SerializedProperty secondarySortingStateMode;

		bool defaultSortingEnabled;

		Table table;
		ReorderableList reorderableList;

		GUIStyle warningstyle;

		public class OrientedWordsContainer
		{
			public string Width;
			public string Height;
			public string Column;
			public string Row;
			public string Columns;
			public string Rows;
		}

		public static readonly OrientedWordsContainer VERTICAL_WORDS = new OrientedWordsContainer 
		{ 
			Width= "Width",
			Height = "Height",
			Column = "Column",
			Row = "Row",
			Columns = "Columns",
			Rows = "Rows",
		};

		public static readonly OrientedWordsContainer HORIZONTAL_WORDS = new OrientedWordsContainer
		{
			Width = "Height",
			Height = "Width",
			Column = "Row",
			Row = "Column",
			Columns = "Rows",
			Rows = "Columns",
		};

		public static OrientedWordsContainer GetOrientedWords(bool horizontal)
		{
			return horizontal ? HORIZONTAL_WORDS : VERTICAL_WORDS;
		}

		void OnEnable()
		{
			table = (Table)target;
			columns = 						serializedObject.FindProperty("columns");
			rowsColorMode = 				serializedObject.FindProperty("rowsColorMode");
			bgColor = 						serializedObject.FindProperty("bgColor");
			altBgColor = 					serializedObject.FindProperty("altBgColor");
			lineColor = 					serializedObject.FindProperty("lineColor");
			hasTitles = 					serializedObject.FindProperty("hasTitles");
			titleBGColor = 					serializedObject.FindProperty("titleBGColor");
			titleFontColor = 				serializedObject.FindProperty("titleFontColor");
			titleFontSize = 				serializedObject.FindProperty("titleFontSize");
			titleFont = 					serializedObject.FindProperty("titleFont");
			titleFontStyle = 				serializedObject.FindProperty("titleFontStyle");
			titleHeightMode =				serializedObject.FindProperty("titleHeightMode");
			titleHeight = 					serializedObject.FindProperty("titleHeight");
			rowHeight = 					serializedObject.FindProperty("rowHeight");
			spacing = 						serializedObject.FindProperty("spacing");
			targetCollection = 				serializedObject.FindProperty("targetCollection");
			rowDeleteButtons = 				serializedObject.FindProperty("rowDeleteButtons");
			deleteColumnWidth = 			serializedObject.FindProperty("deleteColumnWidth");
			deleteCellStyle = 				serializedObject.FindProperty("deleteCellStyle");
			rowAddButton = 					serializedObject.FindProperty("rowAddButton");
			addCellStyle = 					serializedObject.FindProperty("addCellStyle");
			selectableRows = 				serializedObject.FindProperty("selectableRows");
			multiSelection = 				serializedObject.FindProperty("multiSelection");
			selectedBgColor = 				serializedObject.FindProperty("selectedBgColor");
			updateCellStyleAtRuntime = 		serializedObject.FindProperty("updateCellStyleAtRuntime");
			updateCellContentAtRuntime = 	serializedObject.FindProperty("updateCellContentAtRuntime");
			limitRowsInEditMode = 			serializedObject.FindProperty("limitRowsInEditMode");
			nbRowsInEditMode = 				serializedObject.FindProperty("nbRowsInEditMode");
			preinstantiateRowsOverLimit = 	serializedObject.FindProperty("preinstantiateRowsOverLimit");
			horizontal = 					serializedObject.FindProperty("horizontal");
			sortingState =					serializedObject.FindProperty("sortingState");
			secondarySortingState =			serializedObject.FindProperty("secondarySortingState");
			sortingStateIndex =				sortingState.FindPropertyRelative("defaultSortingColumnIndex");
			secondarySortingStateIndex = 	secondarySortingState.FindPropertyRelative("defaultSortingColumnIndex");
			sortingStateMode =				sortingState.FindPropertyRelative("sortMode");
			secondarySortingStateMode = 	secondarySortingState.FindPropertyRelative("sortMode");

			defaultSortingEnabled = sortingState.FindPropertyRelative("sortMode").enumValueIndex > 0;

			reorderableList = new ReorderableList(serializedObject, columns);
			reorderableList.drawHeaderCallback = rect =>
			{
				bool changed = GUI.changed;
				columns.isExpanded = reorderableList.draggable = EditorGUI.Foldout(new Rect(rect.x + 10f, rect.y, rect.width, rect.height), columns.isExpanded, string.Format("{0}:", GetOrientedWords(horizontal.boolValue).Columns));
				GUI.changed = changed;
			};

			reorderableList.drawElementCallback =
				(Rect rect, int index, bool isActive, bool isFocused) =>
				{
					if (!columns.isExpanded)
					{
						GUI.enabled = false;
						return;
					}
					SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
					EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
				};
			reorderableList.elementHeightCallback = (index) => (!columns.isExpanded) ? 0.0001f : EditorGUI.GetPropertyHeight(reorderableList.serializedProperty.GetArrayElementAtIndex(index));
			reorderableList.onAddCallback = (list) =>
			{
				list.serializedProperty.arraySize++;
				InitNewColumnInfo(list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1), table);
				EditorUtility.SetDirty(target);
			};
			reorderableList.onReorderCallback = (list) => table.SetDirty();
		}

		void Awake()
		{
			warningstyle = new GUIStyle(EditorStyles.label);
			warningstyle.normal.textColor = WARNING_COLOR;
		}
		 
		static void InitNewColumnInfo(SerializedProperty columnInfoSP, Table table)
		{
			columnInfoSP.FindPropertyRelative("fieldName").stringValue = "";
			columnInfoSP.FindPropertyRelative("width").floatValue = 0f;
			columnInfoSP.FindPropertyRelative("useRelativeWidth").boolValue = true;
			columnInfoSP.FindPropertyRelative("autoWidth").boolValue = true;
			columnInfoSP.FindPropertyRelative("autoColumnTitle").boolValue = true;
			columnInfoSP.FindPropertyRelative("table").objectReferenceValue = table;
			columnInfoSP.FindPropertyRelative("cellPrefab").objectReferenceValue = null;
			columnInfoSP.FindPropertyRelative("cellStyle").objectReferenceValue = null;
		}

		void DrawWarning(string message)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(EditorGUIUtility.IconContent("Collab.Warning"), GUILayout.Width(20f));
			EditorGUILayout.LabelField(new GUIContent(message), warningstyle);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();
		}

		public override void OnInspectorGUI()
		{
			OrientedWordsContainer orientedWords = GetOrientedWords(horizontal.boolValue);

			bool sortHasChanged = false;

			EditorGUILayout.LabelField("Data", EditorStyles.boldLabel);
			EditorGUILayout.LabelField("Collection:");
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(targetCollection);
			if (UnityMemberInfoDrawer.changed)
			{
				if (columns.arraySize > 0 && EditorUtility.DisplayDialog("You changed the target collection", string.Format ("Your {0} will likely become invalid.\nClear the selected {1}?", orientedWords.Columns, orientedWords.Columns.ToLower()), "Clear", "Do not clear"))
					columns.ClearArray();
				UnityMemberInfoDrawer.changed = false;
			}
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			if (table.targetCollection.IsDefined)
			{
				reorderableList.DoLayoutList();
				GUI.enabled = true;
			}

			List<TableColumnInfo> tableColumnInfos = (List<TableColumnInfo>)columns.GetTargetObjectOfProperty();
			if (tableColumnInfos.Count > 0 && tableColumnInfos.All(tci => !tci.autoWidth))
			{
				DrawWarning(string.Format("No auto-width {0}.", orientedWords.Column.ToLower()));
			}
			if (tableColumnInfos.Count > 0 && tableColumnInfos.Sum(tci => tci.autoWidth ? 0f : tci.AbsoluteWidth) > table.GetAvailableWidth())
			{
				DrawWarning(string.Format("{0} don't fit in table's RectTransform.", orientedWords.Columns));
			}

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(rowDeleteButtons, new GUIContent("\"Delete\" Buttons"));
			if (rowDeleteButtons.boolValue)
			{
				EditorGUIUtility.labelWidth = EditorGUIUtility.fieldWidth = 40f;
				EditorGUILayout.PropertyField(deleteColumnWidth, new GUIContent(orientedWords.Width), GUILayout.Width(80f));
				EditorGUIUtility.labelWidth = EditorGUIUtility.fieldWidth = -1f;
				TableColumnInfoDrawer.DrawCellStyleButton(
					EditorGUILayout.GetControlRect(false, GUILayout.Width(20f)),
					deleteCellStyle,
					table.deleteCellPrefab,
					string.Format("{0}_Delete_Button_Style", serializedObject.targetObject.name),
					table,
					table.deleteCellStyleTemplate);
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(rowAddButton, new GUIContent("\"Add\" Button"));
			if (rowAddButton.boolValue)
			{
				TableColumnInfoDrawer.DrawCellStyleButton(
					EditorGUILayout.GetControlRect(false, GUILayout.Width(20f)),
					addCellStyle,
					table.addCellPrefab,
					string.Format("{0}_Add_Button_Style", serializedObject.targetObject.name),
					table,
					table.addCellStyleTemplate);
			}
			EditorGUILayout.EndHorizontal();

			sortingState.isExpanded = EditorGUILayout.Foldout(sortingState.isExpanded, "Default Sorting");
			if (sortingState.isExpanded)
			{
				EditorGUI.indentLevel++;
				string[] columnNames = table.columns.Select(c => c.fieldName).ToArray();
				sortHasChanged |= DrawSortingState("Primary", sortingStateMode, sortingStateIndex, columnNames);
				sortHasChanged |= DrawSortingState("Secondary", secondarySortingStateMode, secondarySortingStateIndex, columnNames);
				EditorGUI.indentLevel--;
			}

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Appearance", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(rowsColorMode, new GUIContent(string.Format("{0} Colors", orientedWords.Row)));
			EditorGUI.indentLevel++;
			if (table.rowsColorMode == Table.RowsColorMode.Plain)
				EditorGUILayout.PropertyField(bgColor, new GUIContent("Color"));
			else
			{
				EditorGUILayout.PropertyField(bgColor, new GUIContent(string.Format("Odd {0}", orientedWords.Rows)));
				EditorGUILayout.PropertyField(altBgColor, new GUIContent(string.Format("Even {0}", orientedWords.Rows)));
			}
			EditorGUI.indentLevel--;
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(selectableRows, new GUIContent("Selectable Lines"));
			if (selectableRows.boolValue)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(multiSelection, new GUIContent("Multi Selection"));
				EditorGUILayout.PropertyField(selectedBgColor, new GUIContent("Selected Color"));
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.Space();
			bool oldOutline = lineColor.colorValue != Color.clear;
			bool newOutline = EditorGUILayout.Toggle(new GUIContent("Outline"), oldOutline);
			if (oldOutline && !newOutline)
			{
				lineColor.colorValue = Color.clear;
				spacing.floatValue = 0f;
			}
			else if (!oldOutline && newOutline)
			{
				lineColor.colorValue = Color.black;
				spacing.floatValue = -1f;
			}
			if (newOutline)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(lineColor, new GUIContent("Color"));
				EditorGUILayout.PropertyField(spacing);
				EditorGUI.indentLevel--;
				EditorGUILayout.Space();
			}
			EditorGUILayout.Space();

			EditorGUILayout.PropertyField(hasTitles, new GUIContent("Headers"));
			if (hasTitles.boolValue)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(titleBGColor, new GUIContent("BG Color"));
				EditorGUILayout.PropertyField(titleFont, new GUIContent("Font"));
				EditorGUILayout.PropertyField(titleFontStyle, new GUIContent("Font Style"));
				EditorGUILayout.PropertyField(titleFontSize, new GUIContent("Font Size"));
				EditorGUILayout.PropertyField(titleFontColor, new GUIContent("Font Color"));
				GUILayout.BeginHorizontal();
				int oldTitleHeightMode = titleHeightMode.enumValueIndex;
				EditorGUILayout.PropertyField(titleHeightMode, new GUIContent(orientedWords.Height), GUILayout.Width(70f + EditorGUIUtility.labelWidth));
				if (oldTitleHeightMode != titleHeightMode.enumValueIndex)
					titleHeight.floatValue = rowHeight.floatValue;
				if (titleHeightMode.enumValueIndex == 1)
					EditorGUILayout.PropertyField(titleHeight, new GUIContent(""));
				GUILayout.EndHorizontal();
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.Space();

			EditorGUILayout.PropertyField(rowHeight, new GUIContent(string.Format("Default {0} {1}", orientedWords.Row, orientedWords.Height)));

			bool isScrollable = table.IsScrollable;
			if (EditorGUILayout.Toggle("Scrollable", isScrollable) != isScrollable)
			{
				table.MakeScrollable(!isScrollable);
			}

			EditorGUILayout.PropertyField(horizontal, new GUIContent("Horizontal"));

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Performance", EditorStyles.boldLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("At Runtime:");
			EditorGUI.indentLevel++;

			EditorGUILayout.LabelField("Update");
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(updateCellStyleAtRuntime, new GUIContent("Cell Style", "Updates the cells style at runtime. Enable if you plan to modify these styles at runtime."));
			EditorGUILayout.PropertyField(updateCellContentAtRuntime, new GUIContent("Cell Content", "Updates the cells content at runtime from the target collection. Enable if the collection's elements might be modified outside of the table."));
			EditorGUI.indentLevel--;

			EditorGUILayout.Space();
			EditorGUI.indentLevel--;
			EditorGUILayout.LabelField("In Edit Mode:");
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(limitRowsInEditMode, new GUIContent("Limit Rows", "Limit the number of rows to display in edit mode for previewing the table. This is important to avoid slowing down the editor when a large table is displayed."));
			if (limitRowsInEditMode.boolValue)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(nbRowsInEditMode, new GUIContent("Limit", "The maximum number of rows to display in edit mode for previewing the table."));
				EditorGUILayout.PropertyField(preinstantiateRowsOverLimit, new GUIContent("Preinstantiate all", "If true, all rows will be instantiated in edit mode but inactive. This will make the scene bigger but will be faster to start at runtime. If false, only rows below limit will be pre-instantiated."));
				EditorGUI.indentLevel--;
			}
			EditorGUI.indentLevel--;
			EditorGUI.indentLevel--;
			serializedObject.ApplyModifiedProperties();
			if (sortHasChanged)
				table.UpdateSortedElements();
		}

		bool DrawSortingState(string label, SerializedProperty sortMode, SerializedProperty defaultIndex, string[] columnNames)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(sortMode, new GUIContent(label));
			EditorGUILayout.LabelField("Column:", GUILayout.Width(65f));
			defaultIndex.intValue = EditorGUILayout.Popup("", defaultIndex.intValue, columnNames, GUILayout.Width(78f));
			EditorGUILayout.EndHorizontal();
			return EditorGUI.EndChangeCheck();
		}

	}

}
