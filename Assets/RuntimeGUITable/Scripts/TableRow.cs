using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System.Linq;

namespace UnityUITable
{

	[ExecuteInEditMode]
	public class TableRow : MonoBehaviour
	{

		Table _table;
		public Table table
		{
			get
			{
				if (_table == null)
					_table = GetComponentInParent<Table>();
				if (_table == null)
					_table = GetComponentInParent<ScrollTableContainer>().GetComponentInChildren<Table>();
				return _table;
			}
		}

		[SerializeField] [HideInInspector] protected LayoutElement layoutElement;

		[SerializeField] [HideInInspector] protected HorizontalOrVerticalLayoutGroup columnLayout;
		public int rowIndex;

		[SerializeField] [HideInInspector] protected List<CellContainer> cellContainers = new List<CellContainer>();
		public List<CellContainer> CellContainers { get { return cellContainers; } }

		public void Initialize(int rowIndex)
		{
			transform.DestroyChildrenImmediate();
			cellContainers.Clear();
			this.rowIndex = rowIndex;
			if (table.horizontal)
				columnLayout = gameObject.GetOrAddComponent<VerticalLayoutGroup>();
			else
				columnLayout = gameObject.GetOrAddComponent<HorizontalLayoutGroup>();
			columnLayout.childControlWidth = true;
			columnLayout.childControlHeight = true;
			columnLayout.childForceExpandWidth = true;
			columnLayout.childForceExpandHeight = true;
			columnLayout.spacing = -1f;
			layoutElement = gameObject.AddComponent<LayoutElement>();

			CreateCells();

			if (rowIndex >= 0)
				transform.SetSiblingIndex(rowIndex + ((table.hasTitles && !table.IsScrollable) ? 1 : 0));

			UpdateLayout();
		}

		protected virtual void CreateCells()
		{
			int columnIndex = 0;
			foreach(TableColumnInfo column in table.columns)
			{
				CreateCell(column, columnIndex);
				columnIndex++;
			}
			if (table.rowDeleteButtons)
			{
				CreateDeleteCell();
			}
		}

		protected virtual CellContainer CreateDeleteCell()
		{
			CellContainer cellContainer = GameObjectUtils.InstantiatePrefab(table.emptyCellContainerPrefab, transform);
			cellContainer.Initialize(rowIndex, table.columns.Count);
			cellContainers.Add(cellContainer);
			ButtonCell cell = (ButtonCell)cellContainer.CreateCellContent(table.deleteCellPrefab);
			cell.Initialize();
			return cellContainer;
		}

		protected virtual CellContainer CreateCell(TableColumnInfo column, int columnIndex)
		{
			TableCellContainer cellContainer = GameObjectUtils.InstantiatePrefab(table.cellContainerPrefab, transform);
			cellContainer.Initialize(rowIndex, columnIndex);
			cellContainers.Add(cellContainer);
			TableCell contentInstance = cellContainer.CreateCellContent(column.CellPrefab);
			contentInstance.Initialize();
			return cellContainer;
		}

		public void UpdateLayout()
		{
			if (table == null)
				return;
			if (table.horizontal)
				layoutElement.minWidth = table.GetHeight(rowIndex);
			else
				layoutElement.minHeight = table.GetHeight(rowIndex);
			columnLayout.spacing = table.spacing;
			foreach (CellContainer cc in cellContainers)
				cc.UpdateLayout();
		}

		public CellContainer GetCellAt(int index)
		{
			try
			{
				return cellContainers[index];
			}
			catch
			{
				return null;
			}
		}

		public int IndexOf(CellContainer cellContainer)
		{
			return cellContainers.IndexOf(cellContainer);
		}

		public void UpdateContent()
		{
			foreach (CellContainer cell in cellContainers)
				cell.UpdateContent();
		}

		public void UpdateStyle()
		{
			foreach (CellContainer cell in cellContainers)
				cell.UpdateStyle();
		}

	}

}
