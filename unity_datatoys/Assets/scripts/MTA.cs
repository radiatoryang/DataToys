using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class StationTurn {
	public int date;
	public int time;
	public int entry;
	public int exit;
	
	public StationTurn (int date, int time, int entry, int exit) {
		this.date = date;
		this.time = time;
		this.entry = entry;
		this.exit = exit;
	}
}

[System.Serializable]
public class StationEntrance {
	public int latitude;
	public int longitude;
	public Vector3 position;
	
//	public bool ada = false;
	
	public StationEntrance(int latitude, int longitude) {
		this.latitude = latitude;
		this.longitude = longitude;
//		this.ada = ada;
	}
}

[System.Serializable]
public class Station {
	public string nameRemote = "station";
	public List<string> id = new List<string>(); // a station can have multiple remote ids
//	public string id = -1;
	public string nameOfficial = "-1";
	public string division = "division";
	public string lines = "lines";
	public List<StationEntrance> entrances = new List<StationEntrance>();
	public List<StationTurn> turnstile = new List<StationTurn>();
	
	public int avgLatitude;
	public int avgLongitude;
	public Vector3 avgPosition = Vector3.zero;
	public Vector3 screenPosition = Vector3.zero;
	
	public Station (string id, string nameRemote, string division, string lines) {
		this.id.Add(id);
		this.nameRemote = nameRemote;
		this.division = division;
		this.lines = lines;
	}
}

public class MTA : MonoBehaviour {
	
	public TextAsset turnstileData; // TODO: array
	public TextAsset stationKey;
	public TextAsset coordinateKey;
	
	public List<Station> stations = new List<Station>();
	public Dictionary<string, Station> stationIDIndex = new Dictionary<string, Station>();
	public Dictionary<string, Station> stationRemoteNameIndex = new Dictionary<string, Station>();
	public Dictionary<string, Station> stationOfficialIndex = new Dictionary<string, Station>();
	
	const int loopsPerFrame = 20;
	
	const int nameMatchMaxAccuracy = 8;
	const int nameMatchMinAccuracy = 5;
	
	const int gpsScalar = 10000;
	Vector3 gpsCenter = new Vector3(-4060, 0, -7400);
	
	public int currentTime = 1;
	
	// Use this for initialization
	void Start () {
		StartCoroutine(ParseStations());
	}
	
	
	IEnumerator ParseStations () {
		
		// 1) GET STATION NAMES AND REMOTE NAMES, MERGE STATIONS WITH SAME NAME
		string[,] stationRemoteNames = CSVReader.SplitCsvGrid(stationKey.text);
		
		for(int i=1; i<stationRemoteNames.GetLength(1)-1; i++) {
			string id = stationRemoteNames[0,i];
			if(!stationIDIndex.ContainsKey(id)) {
				Station thisStation;
				
				if (!stationRemoteNameIndex.ContainsKey(stationRemoteNames[2,i])) {
					thisStation = new Station(id, stationRemoteNames[2,i], stationRemoteNames[4,i], stationRemoteNames[3,i]);
					stations.Add(thisStation);
					stationRemoteNameIndex.Add(thisStation.nameRemote, thisStation); // add name > station to index
				} else {
					thisStation = stationRemoteNameIndex[stationRemoteNames[2,i]];
					thisStation.id.Add(id); // add remote to station
				}
				stationIDIndex.Add(id, thisStation); // add remote > station to index
			}
		}
		
		yield return 0;
		Debug.Log("done parsing / merging station names");
		
		
		// 2) GUESS STATION'S REMOTE-NAME TO OFFICIAL-NAME
		string[,] coords = CSVReader.SplitCsvGrid(coordinateKey.text);
		
		int currentLoops = 0;
		for (int y=1; y<coords.GetLength(1)-1; y++) {
			bool foundMatch = false;
			string debug = "";
			if (stationOfficialIndex.ContainsKey(coords[2,y])) { // this official name is already in database, skip it
//				Debug.Log("already matched " + coords[2,y] + " to " + stationOfficialIndex[coords[2,y]].nameRemote);
				foundMatch = true;
			}
			
			if (!foundMatch) {
				string line = ""; // make a subway line string
				for (int x=3; x<14; x++) {
					if (coords[x,y] != null)
						line += coords[x,y];
					else
						break;
				}
				
				string lineNumbersFlipped = ""; // sometimes numbers are at beginning of subway line string
				int numberStart = -1;
				int numberEnd = -1;
				for (int ch=0; ch<line.Length; ch++) { // get number substring
					if (System.Char.IsNumber(line[ch])) {
						if (numberStart == -1) {
							numberStart = ch;
							numberEnd = ch+1;
						} else {
							numberEnd = ch+1;
						}
					}
				}
				
				if (numberStart != -1 && numberEnd != -1) { // make sure there are numbers at all
					string numbers = line.Substring(numberStart, numberEnd - numberStart ); // extract numbers and letters substrings, switch order
					if (numberStart == 0) { // numbers are at beginning
						string letters = line.Substring(numberEnd, line.Length-numberEnd);
						lineNumbersFlipped = letters + numbers;
					} else { // numbers are at end
						string letters = line.Substring(0, numberStart);
						lineNumbersFlipped = numbers + letters;
					}
				}
				
				string division = coords[0,y];
				string name = coords[2, y];		// try to format official name to remote name as similarly as possible
	//			name = name.Replace(" -", "-"); // only for GTFS data, not needed here
	//			name = name.Replace("- ", "-");
				name = name.ToUpper();
				
				foreach (Station test in stations) { // compare...
//					if (lineNumbersFlipped.StartsWith("456L"))
//						Debug.Log(name + " vs. " + test.nameRemote);
					
					if (test.entrances.Count > 0) { // already has an official name
//							Debug.Log(test.nameRemote + " already has entrances");
						continue;
					}
					
					if (test.division != division) {
//						if (lineNumbersFlipped.StartsWith("456L"))
//							Debug.Log("fail! " + test.division + " != " + division);
						continue;
					}
					
					if (test.lines != line) { // are they the same line?
	//					Debug.Log("fail! " + test.lines + " != " + line);
						if (numberStart != -1 && numberEnd != -1 && test.lines != lineNumbersFlipped) { // nope! then try the flipped version too?
//							if (lineNumbersFlipped.StartsWith("456L"))
//								Debug.Log("fail! (flip) " + test.lines + test.nameRemote + " != " + lineNumbersFlipped);
							continue;
						}
					} 
					
					int maxAccuracy = nameMatchMaxAccuracy;
					if (test.nameRemote.Length < maxAccuracy)
						maxAccuracy = test.nameRemote.Length;
					if (name.Length < maxAccuracy)
						maxAccuracy = name.Length;
					
					for (int i=maxAccuracy; i > nameMatchMinAccuracy; i--) { // try to match gradually smaller substrings of names
//						if (lineNumbersFlipped.StartsWith("456L"))
//							Debug.Log("name match... " + test.nameRemote.Substring(0,i) + " == " + name.Substring(0,i) + "?");
						if (test.nameRemote.Substring(0,i) == name.Substring(0,i)) { // IT'S A MATCH!
							stationOfficialIndex.Add(coords[2,y], test);
							test.nameOfficial = coords[2,y];
							foundMatch = true;
							Debug.Log("MATCH SUCCESS! " + test.nameRemote + "==" + test.nameOfficial);
							break;
						} 
					}
					
					if (foundMatch)
						break;
				}
				
				if (!foundMatch)
					debug = " > " + name + "," + division + "," + line + "/" + lineNumbersFlipped;
			}
			
			if (foundMatch) {
//				Debug.Log(coords[24,y] + "," + coords[25,y]);
				stationOfficialIndex[coords[2,y]].entrances.Add(new StationEntrance (int.Parse(coords[24,y]), int.Parse(coords[25,y]) ) );
			}
			
			currentLoops++;
			if (currentLoops >= loopsPerFrame) {
				if (!foundMatch)
					Debug.LogWarning("MATCH FAIL... couldn't match " + coords[2,y] + debug);
				currentLoops = 0;
				yield return 0;
			}
		}
		
		Debug.Log("done assigning official names / entrances");
		yield return 0;
		
		// 3) CONVERT STATION GPS COORDS INTO VECTOR3
		foreach (Station station in stations) {
			if (station.entrances.Count > 0) {
				int avgLatitude = 0;
				int avgLongitude = 0;
				foreach (StationEntrance entry in station.entrances) {
					avgLatitude += entry.latitude;
					avgLongitude += entry.longitude;
				}
				avgLatitude /= Mathf.RoundToInt(station.entrances.Count);
				avgLongitude /= Mathf.RoundToInt(station.entrances.Count);
				
				station.avgLatitude = avgLatitude;
				station.avgLongitude = avgLongitude;
				
				station.avgPosition = new Vector3 (-(float)avgLatitude / (float)gpsScalar, 0f, (float)avgLongitude / (float)gpsScalar) - gpsCenter;
				station.avgPosition *= 2f;
			}
		}
		
		Debug.Log("done interpolating station locations");
		yield return 0;
		
		StartCoroutine(ParseTurnstiles());
	}
	
	IEnumerator ParseTurnstiles () {
		// 4) DUMP TURNSTILE DATA INTO MEMORY (takes a while, freezes...)
		float startTime = Time.realtimeSinceStartup;
		string[,] turnstile = CSVReader.SplitCsvGrid(turnstileData.text);
		Debug.Log("done reading turnstile entries (" + (Time.realtimeSinceStartup - startTime).ToString() + " s.)");
		yield return 0;
		
		// 5) PUT TURNSTILE DATA INTO STATION CONTAINERS (takes a while)
		int currentLoops = 0;
//		const int limit = turnstile.GetLength(1)-1;
		const int limit = 1000;
		for(int x=1; x<limit; x++) { // 
			Station thisStation = stationIDIndex[turnstile[1,x]];
			for (int y=3; y<39; y+=5) {
				Debug.Log("row" + x + "col" + y + "=" + turnstile[y,x]);
				
				// CHECK IF STRING IS NULL
				if (turnstile[y,x] != null) {
					string check = turnstile[y,x].Replace(" ", "");
					if ( check == "" || check == null) // if empty or null string, break
						break;
				} else {
					break;
				}
				
				string[] date = turnstile[y,x].Split("-".ToCharArray());
				int dateNumber = int.Parse(date[2]) * 10000 + int.Parse(date[0]) * 100 + int.Parse(date[1]); // year, month, day
				
				string time = turnstile[y+1,x].Substring(0, 2); // get the hour
				int timeNumber = int.Parse(time);
				
				int entries = int.Parse(turnstile[y+3,x]);
				
				string exitsClean = turnstile[y+4,x].Replace(" ", "");
				int exits = int.Parse(exitsClean);
				
				thisStation.turnstile.Add(new StationTurn(dateNumber, timeNumber, entries, exits));
			}
			
			currentLoops++;
			if (currentLoops >= loopsPerFrame) {
				Debug.Log(x.ToString() + ") stationID[" + turnstile[1,x] + "].id = " + thisStation.id);
				currentLoops = 0;
				yield return 0;
			}
		}
		Debug.Log("done putting turnstile entries into stations");
		
		
		// 6) SORT TURNSTILE DATA BY DATE / TIME
		currentLoops = 0;
		foreach (Station station in stations) {
			var sortedTurn = from element in station.turnstile
		     				 orderby element.date, element.time
		      				 select element;
			station.turnstile = sortedTurn.ToList();
				
			currentLoops++;
			if (currentLoops > loopsPerFrame) {
				Debug.Log(station.nameRemote + " >> turn count: " + station.turnstile.Count);
				currentLoops = 0;
				yield return 0;
			}
		}
		Debug.Log("done sorting turnstile data");
		
	}
	
	void OnDrawGizmos () {
		const float gizmoScale = 0.0000001f;
		foreach (Station station in stations) {
			if (station.avgPosition != Vector3.zero) {
				Gizmos.color = Color.white;
				Gizmos.DrawWireSphere(station.avgPosition, 0.1f);
				
				int entryDelta = 0;
				int exitDelta = 0;
				if (currentTime != 0 && station.turnstile.Count > currentTime) {
					entryDelta = station.turnstile[currentTime].entry - station.turnstile[currentTime-1].entry;
					exitDelta = station.turnstile[currentTime].exit - station.turnstile[currentTime-1].exit;
				}
				
				Gizmos.color = Color.blue;
				Gizmos.DrawCube(station.avgPosition, new Vector3(0.2f, entryDelta * gizmoScale, 0.2f) );
				Gizmos.color = Color.yellow;
				Gizmos.DrawCube(station.avgPosition, new Vector3(exitDelta * gizmoScale, 0.2f, exitDelta * gizmoScale) );
			}
		}
	}
	
	void Update () {
		foreach (Station station in stations ) {
			station.screenPosition = Camera.main.WorldToScreenPoint(station.avgPosition);
			station.screenPosition = new Vector3( station.screenPosition.x, Screen.height - station.screenPosition.y, station.screenPosition.z); // the GUI coordinate system is upside down relative to every other coordinate system
		}
	}
 
	void OnGUI ()	{
		foreach (Station station in stations) {
			if (station.avgPosition != Vector3.zero) {
				GUI.Label(new Rect(station.screenPosition.x, station.screenPosition.y, 128f, 32f), station.nameRemote);
			}
		}
		
		GUI.Label(new Rect(16f, 16f, 64f, 32f), "CURRENT TIME: " + currentTime.ToString() );
		currentTime = Mathf.RoundToInt(GUI.HorizontalSlider(new Rect(16f, 16f, Screen.width * 0.618f, Screen.height * 0.05f), currentTime, 0f, 24f));
	}
				
}
