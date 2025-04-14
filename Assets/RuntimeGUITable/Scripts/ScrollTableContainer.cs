using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityUITable
{

	[ExecuteInEditMode]
	public class ScrollTableContainer : MonoBehaviour
	{

		public ScrollRect scrollView;

		public Transform headerContainer;

		Table _table;
		Table table
		{
			get
			{
				if (_table == null)
					_table = GetComponentInChildren<Table>();
				return _table;
			}
		}

		void Update()
		{
			float headerRowHeight = table.GetHeight(-1);
			if (table.horizontal)
			{
				((RectTransform)headerContainer.transform).sizeDelta = new Vector2(headerRowHeight, table.GetComponent<RectTransform>().rect.height);
				((RectTransform)scrollView.transform).anchoredPosition = new Vector2(headerRowHeight - 1, 0f);
				((RectTransform)scrollView.transform).sizeDelta = new Vector2(-headerRowHeight, 0f);
				scrollView.horizontal = true;
				scrollView.vertical = false;
			}
			else
			{
				((RectTransform)headerContainer.transform).sizeDelta = new Vector2(table.GetComponent<RectTransform>().rect.width, headerRowHeight);
				((RectTransform)scrollView.transform).anchoredPosition = new Vector2(0f, -headerRowHeight + 1);
				((RectTransform)scrollView.transform).sizeDelta = new Vector2(0f, -headerRowHeight);
				scrollView.horizontal = false;
				scrollView.vertical = true;
			}
		}

	}

}
