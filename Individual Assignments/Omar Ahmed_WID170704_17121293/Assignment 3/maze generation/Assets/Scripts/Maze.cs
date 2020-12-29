using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Maze : MonoBehaviour {

	public IntVector2 size;

	public MazeCell cellPrefab;

	public float generationStepDelay;

	public MazePassage passagePrefab;

	public MazeDoor doorPrefab;

	[Range(0f, 1f)]
	public float doorProbability;

	public MazeWall[] wallPrefabs;

	public MazeRoomSettings[] roomSettings;

	private MazeCell[,] cells;

	private List<MazeRoom> rooms = new List<MazeRoom>();

	public IntVector2 RandomCoordinates {
		get {
			return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z));
		}
	}

	public bool ContainsCoordinates (IntVector2 coordinate) {
		return coordinate.x >= 0 && coordinate.x < size.x && coordinate.z >= 0 && coordinate.z < size.z;
	}

	public MazeCell GetCell (IntVector2 coordinates) {
		return cells[coordinates.x, coordinates.z];
	}

	public IEnumerator Generate () {
		WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
		cells = new MazeCell[size.x, size.z];
		List<MazeCell> activeCells = new List<MazeCell>();
		//for each cell position create a basic ground cell and add it to the activeCells list
		for (int x = 0; x < size.x; x++) {
			for (int z = 0; z < size.z; z++) {
				
			MazeCell newCell =CreateCell(new IntVector2(x, z));
            newCell.Initialize(CreateRoom(-1));
		    activeCells.Add(newCell);
			//build walls at all the borders
			if(x==0){
             CreateWall(newCell, null, MazeDirection.West);
			 }
			if(x==size.x-1){
			 CreateWall(newCell, null, MazeDirection.East);
			 }
		    if(z==0){
			 CreateWall(newCell, null, MazeDirection.South);
			 }
		    if(z==size.z-1){
			 CreateWall(newCell, null, MazeDirection.North);
			 }
						}
		}
		for(int currentIndex = 0; currentIndex < activeCells.Count; currentIndex++) {
		yield return delay;
		DoNextGenerationStep(activeCells,currentIndex);
		}
	
	}


	private void DoNextGenerationStep (List<MazeCell> activeCells,int index) {
		
		   //Binary Tree Algorithm
            MazeCell currentCell = activeCells[index];
		   //choose a random direction between North and East
            MazeDirection direction = MazeDirections.RandomValue;
			//if we are at X border choose North as East will go out of the grid size   
		    if(currentCell.coordinates.x==size.x-1){
			  direction =MazeDirection.North; 
		     }
			 //if we are at Z border choose East as North will go out of the grid size
		    if(currentCell.coordinates.z==size.z-1){
		 	 direction =MazeDirection.East; 
		     }
		     //get the coordinates of the neighbour
		    IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();
		    if (ContainsCoordinates(coordinates)) {
			    MazeCell neighbor = GetCell(coordinates);
				//create the passage in the random chosen direction
				CreatePassage(currentCell, neighbor, direction);
				//create the wall in the other direction available rather than the one chosen for the passage
				//ex if passage is created at North create wall at East and if passage is created at East create wall at North
				if(direction==MazeDirection.North){
				  CreateWall(currentCell, neighbor,MazeDirection.East);
				}
				else if(direction==MazeDirection.East){
                  CreateWall(currentCell, neighbor,MazeDirection.North);
				}
				
		        if (currentCell.room.settingsIndex == neighbor.room.settingsIndex) {
				  CreatePassageInSameRoom(currentCell, neighbor, direction);
			    }
			
		}
		
	
			
		
		
		
		
	}

	private MazeCell CreateCell (IntVector2 coordinates) {
		MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
		cells[coordinates.x, coordinates.z] = newCell;
		newCell.coordinates = coordinates;
		newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.z;
		newCell.transform.parent = transform;
		newCell.transform.localPosition = new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 0f, coordinates.z - size.z * 0.5f + 0.5f);
		return newCell;
	}

	private void CreatePassage (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
		MazePassage prefab = Random.value < doorProbability ? doorPrefab : passagePrefab;
		MazePassage passage = Instantiate(prefab) as MazePassage;
		passage.Initialize(cell, otherCell, direction);
		passage = Instantiate(prefab) as MazePassage;
		if (passage is MazeDoor) {
			otherCell.Initialize(CreateRoom(cell.room.settingsIndex));
		}
		else {
			otherCell.Initialize(cell.room);
		}
		passage.Initialize(otherCell, cell, direction.GetOpposite());
	}

	private void CreatePassageInSameRoom (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
		MazePassage passage = Instantiate(passagePrefab) as MazePassage;
		passage.Initialize(cell, otherCell, direction);
		passage = Instantiate(passagePrefab) as MazePassage;
		passage.Initialize(otherCell, cell, direction.GetOpposite());
		if (cell.room != otherCell.room) {
			MazeRoom roomToAssimilate = otherCell.room;
			cell.room.Assimilate(roomToAssimilate);
			rooms.Remove(roomToAssimilate);
			Destroy(roomToAssimilate);
		}
	}

	private void CreateWall (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
		MazeWall wall = Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)]) as MazeWall;
		wall.Initialize(cell, otherCell, direction);

	}

	private MazeRoom CreateRoom (int indexToExclude) {
		MazeRoom newRoom = ScriptableObject.CreateInstance<MazeRoom>();
		newRoom.settingsIndex = Random.Range(0, roomSettings.Length);
		if (newRoom.settingsIndex == indexToExclude) {
			newRoom.settingsIndex = (newRoom.settingsIndex + 1) % roomSettings.Length;
		}
		newRoom.settings = roomSettings[newRoom.settingsIndex];
		rooms.Add(newRoom);
		return newRoom;
	}
}