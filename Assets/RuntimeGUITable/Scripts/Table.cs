using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace UnityUITable
{

    [ExecuteInEditMode]
    public class Table : MonoBehaviour
    {

        public enum State { UndefinedState, InvalidCollection, InvalidColumns, Valid }
        [SerializeField] [HideInInspector] State currentState = State.UndefinedState;

        bool isDirty;

        public void SetDirty()
        {
            isDirty = true;
        }

        public static bool IsCollection(System.Type type)
        {
            if (type == null || type == typeof(string) || type == typeof(Transform))
                return false;
            return typeof(IEnumerable).IsAssignableFrom(type) && type.IsGenericType;
        }

        static bool IsCollection(MemberInfo memberInfo)
        {
            return memberInfo.MemberType == MemberTypes.Field && IsCollection(((FieldInfo)memberInfo).FieldType)
                || memberInfo.MemberType == MemberTypes.Property && IsCollection(((PropertyInfo)memberInfo).PropertyType);
        }

        public static readonly Color DARK_BLUE = new Color(0f, 0f, 0.5f);
        public static readonly Color DARK_GRAY = new Color(0.4f, 0.4f, 0.4f);
        public static readonly Color SELECTION_BLUE = new Color(0.2f, 0.4f, 0.5f);

        public TableCellContainer cellContainerPrefab;
        public CellContainer emptyCellContainerPrefab;
        public CellContainer addButtonCellContainerPrefab;

        public HeaderCellContainer columnTitlePrefab;

        public List<TableColumnInfo> columns = new List<TableColumnInfo>();

        public bool rowDeleteButtons;

        public float deleteColumnWidth = 40f;

        public ButtonCell deleteCellPrefab;

        public ButtonCellStyle deleteCellStyle;
        public ButtonCellStyle deleteCellStyleTemplate;
        public ButtonCellStyle DeleteCellStyle { get { return (deleteCellStyle != null) ? deleteCellStyle : deleteCellStyleTemplate; } }
        public DeleteColumnInfo deleteColumnInfo { get { return new DeleteColumnInfo(this); } }

        public bool rowAddButton;

        public ButtonCell addCellPrefab;

        public ButtonCellStyle addCellStyle;
        public ButtonCellStyle addCellStyleTemplate;
        public ButtonCellStyle AddCellStyle { get { return (addCellStyle != null) ? addCellStyle : addCellStyleTemplate; } }
        public AddCellInfo addCellInfo { get { return new AddCellInfo(this); } }

        public enum RowsColorMode { Plain, Striped }
        public RowsColorMode rowsColorMode;
        public Color bgColor = Color.gray;
        public Color altBgColor = DARK_GRAY;

        public bool selectableRows;
        public Color selectedBgColor = SELECTION_BLUE;
        public enum MultiSelectionType { Never, Always, HoldingCtrlOrAlt }
        public MultiSelectionType multiSelection = MultiSelectionType.HoldingCtrlOrAlt;

        public Color lineColor = Color.white;
        public bool hasTitles = true;
        public Color titleBGColor = DARK_BLUE;
        public Color titleFontColor = Color.white;
        public int titleFontSize = 14;
        public Font titleFont;
        public FontStyle titleFontStyle;
        public enum TitleHeightMode { Default, Override }
        public TitleHeightMode titleHeightMode = TitleHeightMode.Default;
        public float titleHeight = 32f;

        public float rowHeight = 32f;
        public float spacing = -1f;

        public Sprite outlineSprite;

        public bool updateCellStyleAtRuntime;
        public bool updateCellContentAtRuntime;

        public bool limitRowsInEditMode = true;
        public int nbRowsInEditMode = 10;
        public bool preinstantiateRowsOverLimit = false;

        public bool horizontal = false;

        public UnityMemberInfo targetCollection = new UnityMemberInfo(IsCollection);

        public SortingState sortingState;
        public SortingState secondarySortingState;

        List<float> heights = new List<float>();
        public float GetHeight(int rowIndex)
        {
            // Headers row
            if (rowIndex == -1)
                return titleHeightMode == TitleHeightMode.Override ? titleHeight : rowHeight;
            // Data rows
            if (rowIndex >= 0 && rowIndex < heights.Count)
                return heights[rowIndex];
            // Add button row + safety default case
            return rowHeight;
        }

        List<int> selectedRows = new List<int>();

        bool IsRowSelected(int index) { return selectedRows.Contains(index); }

        public int SelectedCount { get { return selectedRows.Count; } }

        public IEnumerable<int> SelectedElements { get { return selectedRows.Select(r => _sortedElementsMap[r]); } }

        #region CachedElements

        List<TableColumnInfo> validColumns = new List<TableColumnInfo>();

        bool UpdateValidColumns()
        {
            int oldCount = validColumns.Count;
            validColumns = columns.Where(c => c.IsDefined).ToList();
            return validColumns.Count != oldCount;
        }

        [SerializeField] [HideInInspector] HeaderRow headerRow;

        [SerializeField] [HideInInspector] List<TableRow> tableRows = new List<TableRow>();

        [SerializeField] [HideInInspector] AddRow addRow;

        [SerializeField] [HideInInspector] bool hasRowDeleteButtons;

        [SerializeField] [HideInInspector] bool hasRowAddButton;

        #endregion

        RectTransform _rectTransform;
        RectTransform rectTransform
        {
            get
            {
                if (_rectTransform == null)
                    _rectTransform = GetComponent<RectTransform>();
                return _rectTransform;
            }
        }

        public IEnumerable<object> GetCollectionElements()
        {
            if (!Application.isPlaying && limitRowsInEditMode && !preinstantiateRowsOverLimit && targetCollection.Collection.Count > nbRowsInEditMode)
                return targetCollection.Collection.Cast<object>().Take(nbRowsInEditMode).ToList();
            return targetCollection.Collection.Cast<object>().ToList();
        }

        public int ElementCount
        {
            get
            {
                return GetSortedElements().Count;
            }
        }

        public int ActualElementCount
        {
            get
            {
                if (targetCollection.Collection == null) return 0;
                if (!Application.isPlaying && limitRowsInEditMode && !preinstantiateRowsOverLimit)
                    return Mathf.Min(targetCollection.Collection.Count, nbRowsInEditMode);
                return targetCollection.Collection.Count;
            }
        }

        public System.Type ElementType
        {
            get
            {
                if (!targetCollection.IsDefined)
                    return null;
                return targetCollection.ElementType;
            }
        }

        public float GetAvailableWidth(int nbColumns)
        {
            Rect rect = rectTransform.rect;
            float total = horizontal ? rect.height : rect.width;
            return total - (rowDeleteButtons ? deleteColumnWidth : 0f) - spacing * (nbColumns - 1 + (rowDeleteButtons ? 1 : 0));
        }

        public float GetAvailableWidth()
        {
            return GetAvailableWidth(validColumns.Count);
        }

        public TableColumnInfo GetColumnInfoAt(int index)
        {
            if (index == validColumns.Count && rowDeleteButtons)
                return deleteColumnInfo;
            else if (index < 0)
                return addCellInfo;
            else
                return validColumns[index];
        }

        HorizontalOrVerticalLayoutGroup _hGroup;
        HorizontalOrVerticalLayoutGroup hGroup
        {
            get
            {
                if (_hGroup == null) _hGroup = GetComponent<HorizontalOrVerticalLayoutGroup>();
                return _hGroup;
            }
        }

        ContentSizeFitter _fitter;
        ContentSizeFitter fitter
        {
            get
            {
                if (_fitter == null) _fitter = GetComponent<ContentSizeFitter>();
                return _fitter;
            }
        }

        public bool IsMultiSelecting
        {
            get
            {
                if (multiSelection == MultiSelectionType.Always)
                    return true;
                if (multiSelection == MultiSelectionType.Never)
                    return false;
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
                return Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand);
#else
				return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
#endif
            }
        }
        public bool IsBatchSelecting
        {
            get
            {
                if (multiSelection == MultiSelectionType.Always)
                    return false;
                if (multiSelection == MultiSelectionType.Never)
                    return false;
                if (IsMultiSelecting)
                    return false;
                return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            }
        }

        public Color GetRowBGColor(int rowIndex)
        {
            if (selectableRows && IsRowSelected(rowIndex))
                return selectedBgColor;
            if (rowsColorMode == RowsColorMode.Plain)
                return bgColor;
            else
                return rowIndex % 2 == 0 ? altBgColor : bgColor;
        }

        private void Start()
        {
            UpdateValidColumns();
            sortingState.Init(columns);
            secondarySortingState.Init(columns);
            UpdateContent();
            UpdateStyle();
        }

        void HandleInputs()
        {
            if (currentState != State.Valid)
                return;
#if ENABLE_INPUT_SYSTEM
			if (Keyboard.current.tabKey.wasPressedThisFrame)
				HandleTabKey(Keyboard.current.shiftKey.isPressed);
#elif ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(KeyCode.Tab))
                HandleTabKey(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
            if (selectedRows.Count > 0)
            {
                bool selectionChange = false;
                int nextRow = -1;
                if (horizontal && Input.GetKeyDown(KeyCode.RightArrow) || !horizontal && Input.GetKeyDown(KeyCode.DownArrow))
                {
                    selectionChange = true;
                    nextRow = selectedRows.Max() + 1;
                }
                else if (horizontal && Input.GetKeyDown(KeyCode.LeftArrow) || !horizontal && Input.GetKeyDown(KeyCode.UpArrow))
                {
                    selectionChange = true;
                    nextRow = selectedRows.Min() - 1;
                }
                if (selectionChange)
                {
                    if (!IsBatchSelecting)
                        nextRow = Mathf.Clamp(nextRow, 0, ActualElementCount - 1);
                    OnRowClicked(nextRow);
                }
            }
#endif
        }

        enum TabDirection { Forward, Backward }

        private void HandleTabKey(bool isNavigateBackward)
        {
            GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
            if (selectedObject != null && selectedObject.activeInHierarchy)
            {
                Selectable currentSelection = selectedObject.GetComponent<Selectable>();
                if (currentSelection != null)
                {
                    if (currentSelection.GetComponentInParent<Table>() != this)
                        return;
                    Selectable nextSelection = FindNextSelectable(currentSelection, isNavigateBackward ? TabDirection.Backward : TabDirection.Forward);
                    if (nextSelection != null)
                    {
                        nextSelection.Select();
                    }
                }
            }
        }

        Selectable FindNextSelectable(Selectable selectable, TabDirection direction)
        {
            int columnIndex = -1, rowIndex = -1;
            CellContainer cellContainer = selectable.GetComponentInParent<CellContainer>();
            for (int i = 0; i < tableRows.Count; i++)
            {
                int row = tableRows[i].IndexOf(cellContainer);
                if (row >= 0)
                {
                    columnIndex = i;
                    rowIndex = row;
                    break;
                }
            }
            if (columnIndex < 0 || rowIndex < 0)
                return null;
            if (direction == TabDirection.Forward)
            {
                for (int i = rowIndex; i < ElementCount; i++)
                {
                    int startIndex = (i == rowIndex) ? columnIndex + 1 : 0;
                    for (int j = startIndex; j < tableRows.Count; j++)
                    {
                        Selectable s = GetSelectableFromCellAt(j, i);
                        if (s != null)
                            return s;
                    }
                }
            }
            else
            {
                for (int i = rowIndex; i >= 0; i--)
                {
                    int startIndex = (i == rowIndex) ? columnIndex - 1 : tableRows.Count - 1;
                    for (int j = startIndex; j >= 0; j--)
                    {
                        Selectable s = GetSelectableFromCellAt(j, i);
                        if (s != null)
                            return s;
                    }
                }
            }
            return null;
        }

        Selectable GetSelectableFromCellAt(int columnIndex, int rowIndex)
        {
            TableRow column = tableRows[rowIndex];
            CellContainer cc = column.GetCellAt(columnIndex);
            return (cc == null) ? null : cc.GetComponentsInChildren<Selectable>().FirstOrDefault(s => s.gameObject != cc.gameObject);
        }

        void UpdateColumnWidths()
        {
            float totalWidth = GetAvailableWidth(validColumns.Count);
            System.Func<TableColumnInfo, float> GetAbsoluteWidth = (c) => c.useRelativeWidth ? c.width * totalWidth : c.width;
            foreach (TableColumnInfo column in validColumns)
            {
                if (column.autoWidth)
                {
                    float newWidth = (totalWidth - validColumns.Where(c => !c.autoWidth).Sum(c => GetAbsoluteWidth(c))) / validColumns.Count(c => c.autoWidth);
                    if (column.useRelativeWidth)
                        newWidth = newWidth / totalWidth;
                    if (!Mathf.Approximately(newWidth, column.width))
                    {
                        column.width = newWidth;
                    }
                }
                column.AbsoluteWidth = column.useRelativeWidth ? column.width * totalWidth : column.width;
            }
        }

        void UpdateRowHeights()
        {
            if (heights.Count > ElementCount)
                heights.RemoveRange(0, heights.Count - ElementCount);
            else if (heights.Count < ElementCount)
                heights.AddRange(Enumerable.Repeat<float>(0, ElementCount - heights.Count));
            if (validColumns.All(vc => !vc.expandableHeight))
            {
                for (int i = 0; i < heights.Count; i++)
                    heights[i] = rowHeight;
            }
            else
            {
                for (int i = 0; i < heights.Count; i++)
                    //heights[i] = Mathf.Max(rowHeight, tableRows[i + (hasTitles ? 1 : 0)].CellContainers.Max(c =>
                    heights[i] = Mathf.Max(rowHeight, tableRows[0].CellContainers.Max(c =>
                    {
                        if (c != null) return c.contentRequiredHeight;
                        return 0f;
                    }));
            }
        }

        private void Update()
        {
            Canvas.ForceUpdateCanvases();

            if (UpdateValidColumns())
                SetDirty();

            if (currentState == State.Valid && (hasRowDeleteButtons != rowDeleteButtons || hasRowAddButton != rowAddButton || (headerRow != null) != hasTitles))
                SetDirty();

            if (rectTransform.sizeDelta.x == 0f && rectTransform.anchorMin.x == rectTransform.anchorMax.x)
                rectTransform.sizeDelta = new Vector2(200f, 100f);
            HandleInputs();

            bool unconstrained = (validColumns.Count == 0 || validColumns.Any(c => c.autoWidth || c.useRelativeWidth));
            ContentSizeFitter.FitMode fitMode = unconstrained ? ContentSizeFitter.FitMode.Unconstrained : ContentSizeFitter.FitMode.PreferredSize;

            if (horizontal)
            {
                fitter.verticalFit = fitMode;
                fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            }
            else
            {
                fitter.horizontalFit = fitMode;
                fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            }

            UpdateState();
            if (currentState == State.Valid && !isDirty)
            {

                UpdateColumnWidths();
                UpdateRowHeights();

                if (horizontal != (hGroup is HorizontalLayoutGroup))
                {
                    SetDirty();
                    DestroyImmediate(hGroup);
                    gameObject.AddComponent(horizontal ? typeof(HorizontalLayoutGroup) : typeof(VerticalLayoutGroup));
                    hGroup.childControlWidth = hGroup.childControlHeight = hGroup.childForceExpandWidth = hGroup.childForceExpandHeight = true;
                    hGroup.spacing = -1f;
                }
            }

            if (isDirty)
            {
                Debug.Log("Reinitialize");
                Initialize();
            }

            if (updateCellContentAtRuntime || !Application.isPlaying)
                UpdateContent();
            if (updateCellStyleAtRuntime || !Application.isPlaying)
                UpdateStyle();

            hGroup.spacing = spacing;

            UpdateLayout();

        }

        [ContextMenu("Initialize")]
        public void Initialize()
        {
            Debug.Log("Initialize");
            while (transform.childCount > 0)
                DestroyImmediate(transform.GetChild(0).gameObject);
            if (IsScrollable)
            {
                Transform headerContainer = GetComponentInParent<ScrollTableContainer>().headerContainer;
                while (headerContainer.childCount > 0)
                    DestroyImmediate(headerContainer.GetChild(0).gameObject);
                rectTransform.sizeDelta = Vector2.zero;
            }
            tableRows.Clear();
            if (!targetCollection.IsDefined)
            {
                GameObject content = transform.CreateChildGameObject("Text");
                Text text = content.AddComponent<Text>();
                text.text = "Table - Target not defined.";
                text.alignment = TextAnchor.MiddleCenter;
                isDirty = false;
                return;
            }
            foreach (TableColumnInfo column in columns)
            {
                column.table = this;
            }
            UpdateValidColumns();
            if (validColumns.Count == 0)
            {
                GameObject content = transform.CreateChildGameObject("Text");
                Text text = content.AddComponent<Text>();
                text.text = "Table - No valid columns.";
                text.alignment = TextAnchor.MiddleCenter;
                isDirty = false;
                return;
            }
            if (hasTitles)
            {
                headerRow = CreateHeaderRow();
                headerRow.Initialize(-1);
                tableRows.Add(headerRow);
            }
            UpdateSortedElements();
            for (int i = 0; i < ElementCount; i++)
            {
                TableRow tableRow = CreateRow("Row_" + i);
                tableRow.Initialize(i);
                tableRows.Add(tableRow);
            }
            if (rowAddButton)
            {
                addRow = CreateAddRow();
                addRow.Initialize(-2);
            }
            hasRowDeleteButtons = rowDeleteButtons;
            hasRowAddButton = rowAddButton;
            isDirty = false;

            Update();

        }

        void UpdateLayout()
        {
            foreach (var row in tableRows)
                row.UpdateLayout();
            if (addRow != null)
                addRow.UpdateLayout();
        }

        AddRow CreateAddRow()
        {
            GameObject columnGO = transform.CreateChildGameObject("AddRow");
            columnGO.transform.parent = transform;
            AddRow tableRow = columnGO.AddComponent<AddRow>();
            return tableRow;
        }

        HeaderRow CreateHeaderRow()
        {
            GameObject columnGO = transform.CreateChildGameObject("HeaderRow");
            if (IsScrollable)
            {
                Transform headerContainer = GetComponentInParent<ScrollTableContainer>().headerContainer;
                while (headerContainer.childCount > 0)
                    DestroyImmediate(headerContainer.GetChild(0).gameObject);
                columnGO.transform.parent = headerContainer;
                RectTransform rt = columnGO.AddComponent<RectTransform>();
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.anchoredPosition = rt.sizeDelta = Vector2.zero;
            }
            else
                columnGO.transform.parent = transform;
            HeaderRow tableRow = columnGO.AddComponent<HeaderRow>();
            return tableRow;
        }

        TableRow CreateRow(string goName)
        {
            GameObject columnGO = transform.CreateChildGameObject(goName);
            columnGO.transform.parent = transform;
            TableRow tableRow = columnGO.AddComponent<TableRow>();
            return tableRow;
        }

        List<object> _sortedElements;
        List<int> _sortedElementsMap;
        public List<object> GetSortedElements()
        {
            if (_sortedElements == null)
                UpdateSortedElements();
            return _sortedElements;
        }
        public void UpdateSortedElements()
        {
            if (!Application.isPlaying)
            {
                sortingState.Init(columns);
                secondarySortingState.Init(columns);
            }
            _sortedElementsMap = GetCollectionElements().Select((item, index) => index).ToList();
            _sortedElements = sortingState.GetSorted(secondarySortingState.GetSorted(GetCollectionElements(), ref _sortedElementsMap), ref _sortedElementsMap).ToList();
        }

        public void ColumnTitleClicked(TableColumnInfo column)
        {
            sortingState.ClickOnColumn(column);
            UpdateSortedElements();
            UpdateContent();
        }

        public void SetSelected(int index, bool value)
        {
            if (value && !selectedRows.Contains(index))
                selectedRows.Add(index);
            else if (!value && selectedRows.Contains(index))
                selectedRows.Remove(index);
        }

        public void ToggleSelected(int index)
        {
            if (!selectedRows.Contains(index))
                selectedRows.Add(index);
            else
                selectedRows.Remove(index);
        }

        public void ClearSelection()
        {
            selectedRows.Clear();
        }

        public void OnRowClicked(int index)
        {
            if (index < 0 || index >= tableRows.Count - (hasTitles ? 1 : 0))
                return;
            if (IsMultiSelecting)
                ToggleSelected(index);
            else if (IsBatchSelecting)
            {
                int closest = selectedRows.OrderBy(i => Mathf.Abs(index - i)).First();
                for (int i = Mathf.Min(closest, index); i <= Mathf.Max(closest, index); i++)
                    SetSelected(i, true);
            }
            else
            {
                ClearSelection();
                SetSelected(index, true);
            }
        }

        public void OnRowDeselected(int index)
        {
            if (!IsMultiSelecting && !IsBatchSelecting)
                ClearSelection();
        }

        public void UpdateContent()
        {
            if (currentState != State.Valid)
                return;
            targetCollection.UpdateCache();
            int titleRows = (hasTitles ? 1 : 0);
            int expectedNbRows = ActualElementCount;
            int actualNbRows = tableRows.Count - titleRows;
            if (actualNbRows != expectedNbRows)
            {
                UpdateSortedElements();
                UpdateRows(expectedNbRows, actualNbRows, titleRows);
            }

            foreach (TableRow row in tableRows)
                row.UpdateContent();
        }

        void UpdateRows(int expectedNbRows, int actualNbRows, int titleRows)
        {
            if (actualNbRows < expectedNbRows)
            {
                int nbToAdd = expectedNbRows - actualNbRows;
                for (int i = 0; i < nbToAdd; i++)
                {
                    int rowIndex = actualNbRows + i;
                    TableRow tableRow = CreateRow("Row_" + rowIndex);
                    tableRow.Initialize(rowIndex);
                    tableRows.Add(tableRow);
                }
            }
            else if (actualNbRows > expectedNbRows)
            {
                int nbToRemove = actualNbRows - expectedNbRows;
                int removalIndex = expectedNbRows + titleRows;
                for (int i = 0; i < nbToRemove; i++)
                {
                    DestroyImmediate(tableRows[removalIndex].gameObject);
                    tableRows.RemoveAt(removalIndex);
                }
            }
        }


        public void UpdateStyle()
        {
            foreach (TableRow row in tableRows)
                row.UpdateStyle();
            if (addRow != null)
                addRow.UpdateStyle();
        }

        void UpdateState()
        {
            State newState = GetState();
            if (newState != currentState)
            {
                SetDirty();
                currentState = newState;
            }
        }

        State GetState()
        {
            if (!targetCollection.IsDefined)
                return State.InvalidCollection;
            if (validColumns.Count == 0)
                return State.InvalidColumns;
            return State.Valid;
        }

        public bool IsScrollable
        {
            get
            {
                return transform.parent != null && transform.parent.parent != null && transform.parent.parent.GetComponent<ScrollRect>() != null;
            }
        }

#if UNITY_EDITOR

		public ScrollTableContainer scrollContainerPrefab;

		public void MakeScrollable(bool scrollable)
		{
			if (scrollable)
			{
				if (IsScrollable)
					return;
				fitter.enabled = false;
				ScrollTableContainer container = (ScrollTableContainer)GameObjectUtils.InstantiatePrefab(scrollContainerPrefab);
				ScrollRect scrollView = container.scrollView;
				container.transform.SetParent(this.transform.parent, false);
				RectTransform tableRT = GetComponent<RectTransform>();
				RectTransform containerRT = container.GetComponent<RectTransform>();
				UnityEditorInternal.ComponentUtility.CopyComponent(tableRT);
				UnityEditorInternal.ComponentUtility.PasteComponentValues(containerRT);
				tableRT.SetParent(scrollView.viewport, false);
				tableRT.sizeDelta = tableRT.anchoredPosition = Vector2.zero;
				tableRT.anchorMin = Vector2.zero;
				tableRT.anchorMax = Vector2.one;
				tableRT.pivot = new Vector2(0f, 1f);
				scrollView.content = tableRT;
				fitter.enabled = true;
				EditorGUIUtility.PingObject(gameObject);
			}
			else
			{
				if (!IsScrollable)
					return;
				fitter.enabled = false;
				ScrollTableContainer container = this.transform.parent.parent.parent.GetComponent<ScrollTableContainer>();
				RectTransform tableRT = GetComponent<RectTransform>();
				RectTransform containerRT = container.GetComponent<RectTransform>();
				tableRT.parent = containerRT.parent;
				UnityEditorInternal.ComponentUtility.CopyComponent(containerRT);
				UnityEditorInternal.ComponentUtility.PasteComponentValues(tableRT);
				DestroyImmediate(containerRT.gameObject);
				fitter.enabled = true;
				EditorGUIUtility.PingObject(gameObject);
			}
			SetDirty();
		}

#endif

    }

}