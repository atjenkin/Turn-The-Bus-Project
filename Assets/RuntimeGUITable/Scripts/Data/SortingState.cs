using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityUITable
{

	[Serializable]
	public struct SortingState
	{
		public enum SortMode { None, Ascending, Descending }
		[SerializeField] public SortMode sortMode;
		[SerializeField] public int defaultSortingColumnIndex;

		public TableColumnInfo sortingColumn { get; private set; }

		public void Init(List<TableColumnInfo> columns)
		{
			if (columns == null || columns.Count == 0)
				sortingColumn = null;
			else
				sortingColumn = columns[defaultSortingColumnIndex];
		}

		public void ClickOnColumn(TableColumnInfo column)
		{
			if (column == sortingColumn)
				sortMode = (SortMode)((((int)sortMode) + 1) % Enum.GetValues(typeof(SortMode)).Length);
			else
			{
				sortingColumn = column;
				sortMode = SortMode.Ascending;
			}
		}

		object KeySelector(KeyValuePair<int, object> elmtKvp)
		{
			object elmt = elmtKvp.Value;
			PropertyOrFieldInfo property = new PropertyOrFieldInfo(elmt.GetType().GetMember(sortingColumn.fieldName)[0]);
			return property.GetValue(elmt);
		}

		public IEnumerable<object> GetSorted(IEnumerable<object> collection, ref List<int> indexMap)
		{
			List<int> indexMap2 = indexMap;
			var collectionWithIndexes = collection.Select((item, index) => new KeyValuePair<int, object>(indexMap2[index], item));
			if (sortMode == SortMode.Ascending)
				collectionWithIndexes = collectionWithIndexes.OrderBy(KeySelector);
			else if (sortMode == SortMode.Descending)
				collectionWithIndexes = collectionWithIndexes.OrderByDescending(KeySelector);

			indexMap = collectionWithIndexes.Select(ei => ei.Key).ToList();
			return collectionWithIndexes.Select(ei => ei.Value);
		}
	}

}
