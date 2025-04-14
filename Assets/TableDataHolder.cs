using System.Collections.Generic;
using UnityEngine;

public class TableDataHolder : MonoBehaviour
{
    public List<TableRowData> tableData = new List<TableRowData>();

    public void AddRow(float u, float v, float cu, float cv, float iu, float iv, float f, float df)
    {
        if (tableData != null)
        {
            // Round all values to two decimal digit precision
            u = Mathf.Round(u * 100) / 100f;
            v = Mathf.Round(v * 100) / 100f;
            cu = Mathf.Round(cu * 100) / 100f;
            cv = Mathf.Round(cv * 100) / 100f;
            iu = Mathf.Round(iu * 100) / 100f;
            iv = Mathf.Round(iv * 100) / 100f;
            f = Mathf.Round(f * 100) / 100f;
            df = Mathf.Round(df * 100) / 100f;

            TableRowData newRow = new TableRowData
            {
                a = 0,
                b = u,
                c = v,
                u = u,
                v = v,
                cu = cu,
                cv = cv,
                iu = iu,
                iv = iv,
                f = f,
                df = df
            };

            tableData.Add(newRow);
            Debug.Log("Added new row: a=" + 0 + ", b=" + 0 + ", c=" + 0 + ", u=" + u + ", v=" + v + ", cu=" + cu + ", cv=" + cv + ", iu=" + iu + ", iv=" + iv + ", f=" + f + ", df=" + df);
        }
        else
        {
            Debug.LogError("tableData component not set in scene.");
        }
    }

}

[System.Serializable]
public class TableRowData
{
    public float a;
    public float b;
    public float c;
    public float u;
    public float v;
    public float cu;
    public float cv;
    public float iu;
    public float iv;
    public float f;
    public float df;
}


