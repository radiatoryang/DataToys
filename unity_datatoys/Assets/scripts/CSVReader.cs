/*
	CSVReader by Dock. (24/8/11)
	http://starfruitgames.com
 
	usage: 
	CSVReader.SplitCsvGrid(textString)
 
	returns a 2D string array. 
 
	Drag onto a gameobject for a demo of CSV parsing.
*/
 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq; 
using System.IO;
 
public class CSVReader : MonoBehaviour 
{
//	public TextAsset csvFile; 
//	public void Start()
//	{
//		string[,] grid = SplitCsvGrid(csvFile.text);
//		Debug.Log("size = " + (1+ grid.GetUpperBound(0)) + "," + (1 + grid.GetUpperBound(1))); 
// 
//		DebugOutputGrid(grid); 
//	}
// 
//	// outputs the content of a 2D array, useful for checking the importer
//	static public void DebugOutputGrid(string[,] grid)
//	{
//		string textOutput = ""; 
//		for (int y = 0; y < grid.GetUpperBound(1); y++) {	
//			for (int x = 0; x < grid.GetUpperBound(0); x++) {
// 
//				textOutput += grid[x,y]; 
//				textOutput += "|"; 
//			}
//			textOutput += "\n"; 
//		}
//		Debug.Log(textOutput);
//	}
 
	// splits a CSV file into a 2D string array
	static public string[,] SplitCsvGrid(string csvText)
	{
		StringReader reader = null;
		reader = new StringReader(csvText);   
		
		List<string> lines = new List<string>();
		string aLine;
		while(true) {
			aLine = reader.ReadLine();
			if (aLine != null)
				lines.Add(aLine);
			else
				break;
		}
		
//		string[] lines = csvText.Split("\n"[0]); 
 
		// finds the max width of row
		int width = 0; 
		for (int i = 0; i < lines.Count; i++)
		{
			string[] row = SplitCsvLine( lines[i] ); 
			width = Mathf.Max(width, row.Length); 
		}
 
		// creates new 2D string grid to output to
		string[,] outputGrid = new string[width + 1, lines.Count + 1]; 
		for (int y = 0; y < lines.Count; y++)
		{
			string[] row = SplitCsvLine( lines[y] ); 
			for (int x = 0; x < row.Length; x++) 
			{
				outputGrid[x,y] = row[x]; 
 
				// This line was to replace "" with " in my output. 
				// Include or edit it as you wish.
//				outputGrid[x,y] = outputGrid[x,y].Replace("\"\"", "\"");
			}
		}
 
		return outputGrid; 
	}
 
	// splits a CSV row 
	static public string[] SplitCsvLine(string line)
	{
		return (from System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(line,
		@"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)", 
		System.Text.RegularExpressions.RegexOptions.ExplicitCapture)
		select m.Groups[1].Value).ToArray();
	}
}