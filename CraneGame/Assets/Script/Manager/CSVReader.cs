using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

public class CSVReader : MonoBehaviour
{
    public static CSVReader instance;
    public List<string> targetAreaString;
    public List<float> targetAreaIndex;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
            return;
        }

    }

    public int ColumnNameToIndex(string column)
    {
        int columnIndex = 0;
        int length = column.Length;

        for (int i = 0; i < length; i++)
        {
            char c = column[i];
            columnIndex *= 26;
            columnIndex += (c - 'A' + 1);  // 计算当前字符的列索引
        }

        //Debug.Log($"{columnIndex - 1}");  // 输出指定单元格的数据

        return columnIndex - 1;  // 减 1 因为列是从 0 开始的
    }
    public string IndexToColumnName(int index)
    {
        if (index < 0)
        {
            throw new ArgumentException("Index must be a non-negative integer.");
        }

        string columnName = "";
        index++;  // Because your original function subtracts 1 at the end

        while (index > 0)
        {
            int remainder = (index - 1) % 26;
            columnName = (char)('A' + remainder) + columnName;
            index = (index - 1) / 26;
        }

        return columnName;
    }

    public float ReadTargetCellIndex(string csvPath, string rowLabel, int col)
    {
        return TargetCellIndex(csvPath, rowLabel, col);
    }
    float TargetCellIndex(string csvPath, string rowLabel, int col)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(csvPath);
        // 1. 检查文件是否存在
        if (csvFile == null)
        {
            Debug.LogError("CSV文件未找到：" + csvPath);
            return 0;
        }
        // 2. 读取CSV所有行（忽略空行）
        string[] lines = csvFile.text.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        // 3. 将行标签（如"AB"）转换为行索引
        int rowIndex = ColumnNameToIndex(rowLabel);
        int colIndex = col - 1;

        string[] values = lines[colIndex].Split(',');
        //Debug.Log($"values({values.Length}");
        //Debug.Log($"Lines ({lines.Length}");

        if (rowIndex > values.Length)
        {
            Debug.LogError("行索引超出范围！");
            return 0;
        }
        if (colIndex > lines.Length)
        {
            Debug.LogError("列索引超出范围！");
            return 0;
        }


        // 5. 输出目标单元格数据
        string cellData = values[rowIndex].Trim();
        //Debug.Log($"单元格 ({rowLabel}{col}): {cellData}");

        float result;
        if (float.TryParse(cellData, out result))
        {
            //Debug.Log("转换成功：" + result);
            return result;
        }
        else
        {
            Debug.LogError("转换失败！原始数据：" + cellData);
            return 0;
        }
    }

    public void ReadTargetAreaIndex(string csvPath, List<float> targetList, string rowLabel, int startCol, int endCol)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(csvPath);
        // 1. 检查文件是否存在
        if (csvFile == null)
        {
            Debug.LogError("CSV文件未找到：" + csvPath);
        }
        // 2. 读取CSV所有行（忽略空行）
        string[] lines = csvFile.text.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        int rowIndex = ColumnNameToIndex(rowLabel);
        int startColIndex = startCol - 1;
        int endColIndex = endCol - 1;

        if (startCol >= endCol)
        {
            Debug.LogError("列索引超出范围！");
            return;
        }

        string[] values;
        string cellData;
        float result;


        for (int i = startCol - 1; i <= endCol; i++)
        {
            values = lines[i].Split(',');
            cellData = values[rowIndex].Trim();

            if (float.TryParse(cellData, out result))
            {
                targetList.Add(result);
            }
            else
            {
                Debug.LogError("转换失败！原始数据：" + cellData);
                return;
            }
        }
    }
    public void ReadTargetAreaIndex(string csvPath, List<float> targetList, string stratRowLabel, string endRowLabel, int col)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(csvPath);
        // 1. 检查文件是否存在
        if (csvFile == null)
        {
            Debug.LogError("CSV文件未找到：" + csvPath);
        }
        // 2. 读取CSV所有行（忽略空行）
        string[] lines = csvFile.text.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        int startRowIndex = ColumnNameToIndex(stratRowLabel);
        int endRowIndex = ColumnNameToIndex(endRowLabel);
        int colIndex = col - 1;

        if (startRowIndex >= endRowIndex)
        {
            Debug.LogError("行索引超出范围！");
            return;
        }

        string[] values;
        string cellData;
        float result;

        for (int i = startRowIndex; i <= endRowIndex; i++)
        {
            values = lines[colIndex].Split(',');
            cellData = values[i].Trim();

            if (float.TryParse(cellData, out result))
            {
                targetList.Add(result);
            }
            else
            {
                Debug.LogError("转换失败！原始数据：" + cellData);
                return;
            }
        }
    }
    public void ReadTargetAreaIndex(string csvPath, List<float> targetList, string stratRowLabel, int startCol, string endRowLabel, int endCol)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(csvPath);
        // 1. 检查文件是否存在
        if (csvFile == null)
        {
            Debug.LogError("CSV文件未找到：" + csvPath);
        }
        // 2. 读取CSV所有行（忽略空行）
        string[] lines = csvFile.text.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        int startRowIndex = ColumnNameToIndex(stratRowLabel);
        int endRowIndex = ColumnNameToIndex(endRowLabel);

        int startColIndex = startCol - 1;
        int endColIndex = endCol - 1;

        string[] values;
        string cellData;
        float result;


        for (int i = startRowIndex; i <= endRowIndex; i++)
        {
            for (int j = startCol - 1; j < endCol; j++)
            {
                values = lines[j].Split(',');
                cellData = values[i].Trim();

                if (float.TryParse(cellData, out result))
                {
                    targetList.Add(result);
                }
                else
                {
                    Debug.LogError("转换失败！原始数据：" + cellData);
                    return;
                }

            }
        }
    }

    public string ReadTargetCellString(string csvPath, string rowLabel, int col)
    {
        return TargetCellString(csvPath, rowLabel, col);
    }
    string TargetCellString(string csvPath, string rowLabel, int col)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(csvPath);
        // 1. 检查文件是否存在
        if (csvFile == null)
        {
            Debug.LogError("CSV文件未找到：" + csvPath);
            return null;
        }
        // 2. 读取CSV所有行（忽略空行）
        string[] lines = csvFile.text.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        // 3. 将行标签（如"AB"）转换为行索引
        int rowIndex = ColumnNameToIndex(rowLabel);
        int colIndex = col - 1;

        string[] values = lines[colIndex].Split(',');
        //Debug.Log($"values({values.Length}");
        //Debug.Log($"Lines ({lines.Length}");

        if (rowIndex > values.Length)
        {
            Debug.LogError("行索引超出范围！");
            return null;
        }
        if (colIndex > lines.Length)
        {
            Debug.LogError("列索引超出范围！");
            return null;
        }


        // 5. 输出目标单元格数据
        string cellData = values[rowIndex].Trim();

        return cellData;
        //Debug.Log($"单元格 ({rowLabel}{col}): {cellData}");

    }
    public void ReadTargetAreaString(string csvPath, List<string> targetList, string rowLabel, int startCol, int endCol)
    {

        TextAsset csvFile = Resources.Load<TextAsset>(csvPath);
        // 1. 检查文件是否存在
        if (csvFile == null)
        {
            Debug.LogError("CSV文件未找到：" + csvPath);
        }
        // 2. 读取CSV所有行（忽略空行）
        string[] lines = csvFile.text.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        int rowIndex = ColumnNameToIndex(rowLabel);
        int startColIndex = startCol - 1;
        int endColIndex = endCol - 1;

        if (startCol >= endCol)
        {
            Debug.LogError("列索引超出范围！");
            return;
        }

        string[] values;
        string cellData;
        for (int i = startCol - 1; i <= endCol; i++)
        {
            values = lines[i].Split(',');
            cellData = values[rowIndex].Trim();
            targetList.Add(cellData);
        }

    }
    public void ReadTargetAreaString(string csvPath, List<string> targetList, string stratRowLabel, string endRowLabel, int col)
    {

        TextAsset csvFile = Resources.Load<TextAsset>(csvPath);
        // 1. 检查文件是否存在
        if (csvFile == null)
        {
            Debug.LogError("CSV文件未找到：" + csvPath);
        }
        // 2. 读取CSV所有行（忽略空行）
        string[] lines = csvFile.text.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        int startRowIndex = ColumnNameToIndex(stratRowLabel);
        int endRowIndex = ColumnNameToIndex(endRowLabel);
        int colIndex = col - 1;

        if (startRowIndex >= endRowIndex)
        {
            Debug.LogError("行索引超出范围！");
            return;
        }

        string[] values;
        string cellData;
        for (int i = startRowIndex; i <= endRowIndex; i++)
        {
            values = lines[colIndex].Split(',');
            cellData = values[i].Trim();
            targetList.Add(cellData);
        }

    }
    public void ReadTargetAreaString(string csvPath, List<string> targetList, string stratRowLabel, int startCol, string endRowLabel, int endCol)
    {

        TextAsset csvFile = Resources.Load<TextAsset>(csvPath);
        // 1. 检查文件是否存在
        if (csvFile == null)
        {
            Debug.LogError("CSV文件未找到：" + csvPath);
        }
        // 2. 读取CSV所有行（忽略空行）
        string[] lines = csvFile.text.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        int startRowIndex = ColumnNameToIndex(stratRowLabel);
        int endRowIndex = ColumnNameToIndex(endRowLabel);

        int startColIndex = startCol - 1;
        int endColIndex = endCol - 1;

        string[] values;
        string cellData;

        for (int i = startRowIndex; i <= endRowIndex; i++)
        {
            for (int j = startCol - 1; j < endCol; j++)
            {
                values = lines[j].Split(',');
                cellData = values[i].Trim();
                targetList.Add(cellData);
                Debug.Log($"单元格: {cellData}");
            }
        }

    }


}
