using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	static uint width = 4;
	static uint height = 4;

	public static uint Width {
		get { return GameManager.width; }
	}
	
	public static uint Height {
		get { return GameManager.height; }
	}


	static private GameManager _instance;

	public static GameManager Instance {
		get { return _instance; }
	}

	public bool[,] havecard;

	public GameManager() {
		havecard = new bool[GameManager.Width, GameManager.Height];
	}

	public bool HavaCard(uint posX, uint posY) {
		if (posX < 0 || posX >= Width || posY < 0 || posY >= Height) {
			print (string.Format("What!!! Error Position [{0}, {1}]", posX, posY));
			return true;
		}
		return havecard[posX, posY];
	}

	public bool ProduceCard(out uint posX, out uint posY) {
		posX = 0;
		posY = 0;
		uint randcount = 0;
		do {
			posX = (uint)Random.Range((int)0, GameManager.Width) + 1;
			posY = (uint)Random.Range((int)0, GameManager.Height) + 1;
			randcount += 1;
			if (randcount > 100) {
				print ("rand error");
				return false;
			}
			//print (string.Format("[{0}, {1}]", posX, posY));
		} while(HavaCard(posX-1, posY-1));
		havecard[posX-1, posY-1] = true;
		return true;
	}

	public void OnPointerMove(Vector2 startPos, Vector2 endPos) {
		Vector2 vec = endPos - startPos;
		float absx = Mathf.Abs(vec.x);
		float absy = Mathf.Abs(vec.y);
		if (absx < 30.0f && absy < 30.0f) {
			return;
		}
		if (absx > absy) {
			if (vec.x > 0)
				print ("right");
			else
				print ("left");
		} else {
			if (vec.y > 0) 
				print ("Up");
			else
				print ("Down");
		}
	}

	void Awake() {
		_instance = this;
	}

	// Use this for initialization
	void Start () {
		GamePanel.Instance.ResetPanel();
	}
	
	// Update is called once per frame
	void Update () {
	}
}
