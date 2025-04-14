using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityUITable
{

	public class AddRow : TableRow
	{

		protected override CellContainer CreateCell(TableColumnInfo column, int columnIndex)
		{
			CellContainer cellContainer;
			if (columnIndex == 0)
			{
				cellContainer = GameObjectUtils.InstantiatePrefab(table.addButtonCellContainerPrefab, transform);
				cellContainer.Initialize(rowIndex, columnIndex);
				ButtonCell cell = (ButtonCell)cellContainer.CreateCellContent(table.addCellPrefab);
				cell.Initialize();
			}
			else
			{
				cellContainer = GameObjectUtils.InstantiatePrefab(table.emptyCellContainerPrefab, transform);
				cellContainer.Initialize(rowIndex, columnIndex);
			}
			cellContainers.Add(cellContainer);
			return cellContainer;
		}

		protected override CellContainer CreateDeleteCell()
		{
			CellContainer cellContainer = GameObjectUtils.InstantiatePrefab(table.emptyCellContainerPrefab, transform);
			cellContainer.Initialize(rowIndex, table.columns.Count);
			cellContainers.Add(cellContainer);
			return cellContainer;
		}

	}

}
