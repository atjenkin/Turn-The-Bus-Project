using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

namespace UnityUITable
{

	[ExecuteInEditMode]
	public class CellContainer : MonoBehaviour
	{

		TableRow _row;
		public TableRow row
		{
			get
			{
				if (_row == null)
					_row = GetComponentInParent<TableRow>();
				return _row;
			}
		}
		public Table table { get { return row.table; } }

		public TableColumnInfo info { get { return table.GetColumnInfoAt(columnIndex); } }

		public int columnIndex;
		public virtual TableColumnInfo columnInfo { get { return table.GetColumnInfoAt(columnIndex); } }
		public int rowIndex { get { return row.rowIndex; } }

		[SerializeField][HideInInspector] LayoutElement layoutElement;
		public Transform content;
		[SerializeField][HideInInspector] TableCell _cellInstance;
		public TableCell cellInstance { get { return _cellInstance; } private set { _cellInstance = value; } }

		public virtual float contentRequiredHeight
		{
			get
			{
				return cellInstance.contentRequiredHeight;
			}
		}

		 void Initialize()
		{
			layoutElement = gameObject.AddComponent<LayoutElement>();
			Update();
		}

		public void Initialize(int rowIndex, int columnIndex)
		{
			SetRowIndex(columnIndex);
			this.columnIndex = columnIndex;
			gameObject.name += columnIndex;
			Initialize();
		}

		public TableCell CreateCellContent(TableCell cellPrefab)
		{
			cellInstance = GameObjectUtils.InstantiatePrefab(cellPrefab, content);
			return cellInstance;
		}

		void SetRowIndex(int rowIndex)
		{
			transform.SetSiblingIndex(rowIndex);
		}

		protected virtual void Update()
		{
		}

		public void UpdateLayout()
		{
			if (table.horizontal)
				layoutElement.preferredHeight = info.AbsoluteWidth;
			else
				layoutElement.preferredWidth = info.AbsoluteWidth;

		}

		public virtual void UpdateContent()
		{
			if (cellInstance)
				cellInstance.UpdateContent();
		}

		public void UpdateStyle()
		{
			if (cellInstance)
			{
				if (!cellInstance.IsRightCellType)
				{
					TableCell rightCellPrefab = cellInstance.columnInfo.CellPrefab;
					int elmtIndex = cellInstance.elmtIndex;
					DestroyImmediate(cellInstance.gameObject);

					cellInstance = CreateCellContent(rightCellPrefab);
					cellInstance.Initialize();
				}
				cellInstance.UpdateStyle();
			}
		}

	}

}