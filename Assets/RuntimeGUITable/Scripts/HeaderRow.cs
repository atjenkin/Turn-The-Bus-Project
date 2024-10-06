using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityUITable
{

	public class HeaderRow : TableRow
	{

		protected override CellContainer CreateDeleteCell()
		{
			CellContainer cellContainer = GameObjectUtils.InstantiatePrefab(table.emptyCellContainerPrefab, transform);
			cellContainer.Initialize(rowIndex, table.columns.Count);
			cellContainers.Add(cellContainer);
			return cellContainer;
		}

		protected override CellContainer CreateCell(TableColumnInfo column, int columnIndex)
		{
			HeaderCellContainer cellContainer = GameObjectUtils.InstantiatePrefab(table.columnTitlePrefab, transform);
			cellContainer.Initialize(-1, columnIndex);
			cellContainers.Add(cellContainer);
			return cellContainer;
		}

	}

}
