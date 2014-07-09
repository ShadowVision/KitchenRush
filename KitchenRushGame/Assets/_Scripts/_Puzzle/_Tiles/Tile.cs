using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
	public enum TileType{
		NONE,
		WHITE,
		RED,
		BLUE,
		GREEN,
		PURPLE,
		GOLD
	}
	public TileType type;
	public int x = 0;
	public int y = 0;

	public GameObject tileEffectTemplate;

	private Board board;
	private bool mouseDown = false;
	private Animation anim;
	private Vector3 targetPos;
	private bool falling = false;
	private float velY = 0;
	private float gravity = -.01f;
	private Vector3 mouseClickPosition;
	private bool dead = false;

	private bool sliding = false;
	private float slideStartTime = 0;
	private const float slideDuractionInSeconds = .1f;
	private Vector3 slideStartPos;
	private Vector3 slideEndPos;

	public Vector3 boardPosition;

	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponent<Animation>();
	}
	
	// Update is called once per frame
	void Update () {
		if(falling){
			fall();
		}
		if(sliding){
			tickSlide();
		}
	}
	private void fall(){
		Vector3 newPosition = transform.position;
		newPosition.y += velY;
		velY+= gravity;
		if(newPosition.y < targetPos.y){
			falling = false;
			velY = 0;
			newPosition.y = targetPos.y;
		}
		transform.position = newPosition;
	}
	public void setStats(int X, int Y){
		setStats(X,Y,board);
	}
	public void setStats(int X, int Y, Board BOARD){
		x = X;
		y = Y;
		board = BOARD;
	}
	public void setTargetPosition(Vector3 newPosition){
		targetPos = newPosition;
		boardPosition = targetPos;
		if(targetPos.y < transform.position.y){
			falling = true;
			velY+=gravity;
		}
	}
	private void OnMouseDown(){
//		Debug.Log("Click");
		mouseDown = true;
		mouseClickPosition = Input.mousePosition;
	}
	private void OnMouseUp(){
//		Debug.Log("release");
		if(mouseDown){
			board.clickTile(x,y);
		}
		mouseDown = false;
	}
	private void runMouseUp(){
		mouseDown = false;
	}
	private void OnMouseExit(){
		if(!dead && mouseDown){
			mouseDown = false;
			slideTile();
		}
	}
	private void slideTile(){
		Vector3 dir = Input.mousePosition - mouseClickPosition;
		Board.Dir direction;

		if(Mathf.Abs(dir.y) > Mathf.Abs(dir.x)){
			if(dir.y > 0){
				//up
				direction = Board.Dir.UP;
			}else{
				//down
				direction = Board.Dir.DOWN;
			}
		}else{
			if(dir.x > 0){
				//right
				direction = Board.Dir.RIGHT;
			}else{
				//left
				direction = Board.Dir.LEFT;
			}
		}
//		Debug.Log("Slid: " + dir + " " + direction);
		board.slideTile(x,y,direction);
	}

	public void die(){
		dead = true;
		Destroy (collider);
		if(anim != null){
			anim.Play();
		}
		Instantiate (tileEffectTemplate, transform.position, transform.rotation);
		SpriteRenderer[] sprites = gameObject.GetComponentsInChildren<SpriteRenderer> ();
		foreach (SpriteRenderer sprite in sprites) {
			sprite.sortingOrder = 100;
		}
		Invoke("destroy", 1.5f);
	}
	private void destroy(){
		Destroy(gameObject);
	}

	public void slideTo(Vector3 newPosition){
		slideStartPos = transform.localPosition;
		slideEndPos = newPosition;
		boardPosition = slideEndPos;
		sliding = true;
		slideStartTime = Time.time;
	}
	private void tickSlide(){
		float delta = (Time.time-slideStartTime)/slideDuractionInSeconds;
		if (delta > 1) {
			sliding = false;
			transform.localPosition = slideEndPos;
		}else{
			transform.localPosition = Vector3.Lerp(slideStartPos,slideEndPos,delta);
		}
	}
}
