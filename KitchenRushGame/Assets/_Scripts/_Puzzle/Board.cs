using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour {
	public static Board instance;
	public static Vector2 currentSelectedTile;
	public Tile[] tileTemplates;
	public Tile goldTileTemplate;
	public GameObject comboPopupTemplate;
	public int tilesWide = 5;
	public int tilesTall = 5;
	private float tileSize = 1;
	public int minNumberOfTiles = 3;
	public GameObject feverTemplate;
	public SpriteRenderer pauseSprite;
	private GameObject feverObject;
	private Tile[,] tiles;
	private bool inFever = false;
	private bool spawnGoldTileNext = false;
	private bool canInteract = true;
	private Vector2 lastClickedPosition;
	
	public bool canClick = true;
	public BasicDelegate onSlideDelegate;
	public BasicDelegate onTapDelegate;
	public BasicDelegate onOrderDelegate;
	public delegate void BasicDelegate();

	public bool tutorial = false;

	public enum Dir{
		UP,
		DOWN,
		LEFT,
		RIGHT
	}
	// Use this for initialization
	void Start () {
	
	}
	void Awake(){
		instance = (Board)this;
	}
	// Update is called once per frame
	void Update () {
	
	}
	public void stopInteraction(){
		canInteract = false;
		Collider2D[] col = gameObject.GetComponentsInChildren<Collider2D>();
		foreach(Collider2D c in col){
			c.enabled = false;
		}
		pauseSprite.enabled = true;
		GameLogic.instance.timer.pauseTimer();
	}
	public void pauseInteraction(float seconds){
		stopInteraction ();
		Invoke("resetInteraction", seconds);
	}
	public void resetInteraction(){
		if(!GameLogic.instance.isPaused){
			canInteract = true;
			pauseSprite.enabled = false;
			GameLogic.instance.timer.resumeTimer();
			Collider2D[] col = gameObject.GetComponentsInChildren<Collider2D>();
			foreach(Collider2D c in col){
				c.enabled = true;
			}
		}
	}
	public void buildBoard(){
		Debug.Log ("Building Board");
		Combo.resetCombo ();
		tiles = new Tile[tilesWide,tilesTall];
		Vector3 newPosition = new Vector3(0,0,0);
		for(int x=0; x<tilesWide; x++){
			for(int y=0; y<tilesTall; y++){
				spawnTile (x,y, newPosition);
				newPosition.y += tileSize;
			}
			newPosition.y = 0;
			newPosition.x += tileSize;
		}
	}
	private Tile spawnTile(int x, int y, Vector3 newPosition){
		Tile template = getTileTemplate();
		if(spawnGoldTileNext){
			spawnGoldTileNext = false;
			template = goldTileTemplate;
		}
		tiles[x,y] = (Tile)Instantiate(template,newPosition, Quaternion.identity);
		tiles[x,y].setStats(x,y,(Board)this);
		tiles[x,y].transform.parent = transform;
		tiles [x, y].boardPosition = newPosition;
		return tiles[x,y];
	}
	public Tile getTileTemplate(){
		return tileTemplates[Random.Range(0,tileTemplates.Length)];
	}
	public void slideTile(int x, int y, Dir direction){
		if(canInteract){
			if(tileOnBoard(x,y)){
				if(!tutorial){
					GameLogic.instance.startGame();
				}
				//take combo down one
				//decrementCombo();
				Vector2 dir = getDirection(direction);
				int nextX = x+(int)dir.x;
				int nextY = y+(int)dir.y;
				if(tileOnBoard(nextX,nextY) && tileCanSwap(tiles[x,y].type,dir,nextX,nextY)){
					Debug.Log("swapping " + x + "x" + y + " with " + nextX + ", " + nextY);
					SoundFiles.playSlide();
					Tile temp = tiles[x,y];
					tiles[x,y] = tiles[nextX,nextY];
					tiles[nextX,nextY] = temp;
					Vector3 tempPosition =tiles[x,y].boardPosition;
					//tiles[x,y].transform.position = tiles[nextX,nextY].transform.position;
					//tiles[nextX,nextY].transform.position = tempPosition;
					tiles[x,y].slideTo(tiles[nextX,nextY].boardPosition);
					tiles[nextX,nextY].slideTo(tempPosition);
					tiles[x,y].setStats(x,y,(Board)this);
					tiles[nextX,nextY].setStats(nextX,nextY,(Board)this);
					stopFever();
					if(onSlideDelegate != null){
						onSlideDelegate();
					}
				}
			}
		}
	}
	private bool tileCanSwap(Tile.TileType tileType,Vector2 dir, int x, int y){
		return true;
/*		Vector2[] dirs = new Vector2[4]{
			new Vector2(1,0),
			new Vector2(-1,0),
			new Vector2(0,1),
			new Vector2(0,-1)
		};
		int checkX;
		int checkY;
		for(int i=0; i<dirs.Length; i++){
			checkX = x-(int)dirs[i].x;
			checkY = y-(int)dirs[i].y;
			if(tileOnBoard(checkX,checkY) && dir != dirs[i] && tiles[checkX,checkY].type == tileType){
				return true;
			}
		}
		return false;
		*/
	}

	private Vector2 getDirection(Dir d){
		Vector2 dir = new Vector2();
		if(d==Dir.UP){
			dir.y = 1;
		}else if(d==Dir.DOWN){
			dir.y = -1;
		}else if(d==Dir.LEFT){
			dir.x = -1;
		}else if(d==Dir.RIGHT){
			dir.x = 1;
		}
		return dir;
	}
	private bool tileOnBoard(int x, int y){
		return (x >= 0 && x< tilesWide && y >= 0 && y< tilesTall && tiles[x,y] != null);
	}

	public void clickTile(int x, int y){
		if(canInteract && canClick){
			lastClickedPosition = new Vector2(x,y);
			startGroupClearAt(x, y);

		}
	}

	private Tile.TileType tileType;
	private Vector2[] clearList;
	private int tileIndex=0;
	private int checkIndex=0;
	private Vector2[] positionsChecked;

	private void fillArrayWithEmpty(Vector2[] array){
		for(int i=0; i<array.Length; i++){
			array[i].x = -1;
			array[i].y = -1;
		}
	}
	private void startGroupClearAt(int x, int y){
		if(tileOnBoard(x,y)){
			//init clear variables
			tileIndex = 0;
			checkIndex = 0;
			tileType = tiles[x,y].type;
			clearList = new Vector2[tilesWide*tilesTall];
			fillArrayWithEmpty(clearList);
			positionsChecked = new Vector2[tilesWide*tilesTall];
			fillArrayWithEmpty(positionsChecked);

			//search nearby tiles and compile a list of tiles to destroy
			clearGroupAt(x,y);
			//play respective animation
			/*if(tileIndex > minNumberOfTiles*3){
				AnimationController.instance.playAnimation(AnimationController.AnimationType.GOOD3);
			}else if(tileIndex > minNumberOfTiles*2){
				AnimationController.instance.playAnimation(AnimationController.AnimationType.GOOD2);
			}else if(tileIndex > minNumberOfTiles){
				AnimationController.instance.playAnimation(AnimationController.AnimationType.GOOD1);
			}*/

			if(tileIndex >= minNumberOfTiles){
				AnimationController.instance.playAnimation(AnimationController.AnimationType.GOOD1);
			}
			//pauseInteraction(2);

			//destroy tiles in list
			destroyTileGroup();
		}
	}
	//Look at neihboring tiles and clear all of same color if their are enough
	private void clearGroupAt(int x, int y){
		//make sure we haven't already checked this tile
		if(tileOnBoard(x,y)){ //If on board
			if(!tileAlreadyChecked(x,y)){
				positionsChecked[checkIndex] = new Vector2(x,y);//save that we are checking this tile
				checkIndex++;
				if(tiles[x,y].type == tileType){
					//This tile is part of the group
					clearList[tileIndex] = new Vector2(x,y);
					tileIndex++;
					//check tile in each direction
					clearGroupAt(x+1,y);
					clearGroupAt(x-1,y);
					clearGroupAt(x,y+1);
					clearGroupAt(x,y-1);
				}
			}
		}
	}
	private bool tileAlreadyChecked(int x, int y){
		foreach(Vector2 pos in positionsChecked){
			if(pos.x == x && pos.y == y){
				return true;
			}
		}
		return false;
	}
	private void destroyTileGroup(){
		if(tileIndex >= minNumberOfTiles){
			if(onTapDelegate != null){
				onTapDelegate();
			}

			if(!tutorial){
				GameLogic.instance.startGame();
			}
			SoundFiles.playComplete();
			foreach(Vector2 tilePos in clearList){
				if(tilePos.x != -1 && tilePos.y != -1){
					destroyTile((int)tilePos.x,(int)tilePos.y);
				}
			}
			dropTiles(clearList);

			//start fever mode or add to combo
			incrementCombo();
			

		}
	}
	private void dropTiles(Vector2[] holes){
		holes = getLowestSpaces(holes);
		foreach(Vector2 hole in holes){
			if(hole.x != -1 && hole.y != -1){
				int x =(int)hole.x;
				int yOffset = 0;
				for(int y = (int)hole.y; y<tilesTall; y++){
					Tile tileAbove;
					int aboveIndex = 0;
					do{
						aboveIndex++;
						if(y+aboveIndex < tilesTall){
							tileAbove = tiles[x,y+aboveIndex];
							tiles[x,y+aboveIndex] = null;
						}else{
							tileAbove = spawnTile(x,y, new Vector3(x,y+2+yOffset,0));
							yOffset++;
						}
					}while(tileAbove == null);
					tiles[x,y] = tileAbove;
					tiles[x,y].setTargetPosition(new Vector3(x,y));
					tiles[x,y].setStats(x,y);
				}
			}
		}
	}
	public Vector2[] getLowestSpaces(Vector2[] spaces){
		Vector2[] lowSpaces = new Vector2[tilesWide];
		Vector2[] currentLowestValues = new Vector2[tilesWide];
		int count = spaces.Length;

		//set all lowest values to be the maximum possible 
		for(int i=0; i<currentLowestValues.Length;i++){
			currentLowestValues[i] = new Vector2(i,tilesTall);
		}
		//Compare each space with the lowest space for the column and store the lowest values
		for(int i=0; i<spaces.Length; i++){
			if(spaces[i].x != -1){
				Vector2 space = spaces[i];
				if(space.y < currentLowestValues[(int)space.x].y){
					currentLowestValues[(int)space.x] = space;
				}
			}else{
				break;
			}
		}
		count = 0;
		for(int i=0; i<currentLowestValues.Length; i++){
			Vector2 newSpace = currentLowestValues[i];
			if(newSpace.y != tilesTall){
				lowSpaces[count] = newSpace;
				count++;
			}
		}
		Vector2[] finalLowSpaces = new Vector2[count];
		for(int i=0 ;i<count; i++){
			finalLowSpaces[i] = lowSpaces[i];
		}
		return finalLowSpaces;
	}
	public void destroyBoard(){
		if(tiles != null){
			foreach(Tile tile in tiles){
				if(tile != null){
					Destroy(tile.gameObject);
				}
			}
			tiles = null;
		}
	}
	private void destroyTile(int x, int y){
		Tile tile = tiles[x,y];
		tiles[x,y] = null;
		Objective obj = Objective.instance;
		if(obj!=null){
			//tell objective we captured a tile RETURNS true if the objective is completed
			if(obj.captureTile(tile.type)){
				//Finished the current objective
				
				SoundFiles.playComplete2();

				if(onOrderDelegate != null){
					onOrderDelegate();
				}
				//spawnGoldTileNext = true;
			}
		}
		tile.die();
	}

	private void incrementCombo(){
		if(inFever){
			Combo.addCombo(1);
			Vector3 popupPosition = new Vector3(lastClickedPosition.x,lastClickedPosition.y,0);
			GameObject comboPopup = (GameObject)Instantiate(comboPopupTemplate,popupPosition,Quaternion.identity);
		}else{
			startFever();
		}
	}
	private void decrementCombo(){
		Combo.addCombo(-1);
	}
	private void startFever(){
		if(!inFever){
			inFever = true;
			//feverObject = (GameObject)Instantiate(feverTemplate,transform.position + new Vector3(tilesWide/2f,tilesTall/2f,2),Quaternion.identity);
			//feverObject.transform.localScale = new Vector3(tilesWide+(margin*2),tilesTall+(margin*2),1);
			//feverObject.transform.localScale = new Vector3(100,100,1);
		}
	}
	private void stopFever(){
		if(inFever){
			inFever = false;
			//Destroy(feverObject.gameObject);
		}
	}
}
