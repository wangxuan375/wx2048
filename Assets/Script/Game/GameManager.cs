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

	public bool HaveCard(uint posX, uint posY) {
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
			posX = (uint)Random.Range((int)0, GameManager.Width);
			posY = 1;//(uint)Random.Range((int)0, GameManager.Height);
			randcount += 1;
			if (randcount > 100) {
				print ("rand error");
				return false;
			}
			//print (string.Format("[{0}, {1}]", posX, posY));
		} while(HaveCard(posX, posY));
		havecard[posX, posY] = true;
		posX += 1;
		posY += 1;
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
				ProcessUp();//print ("Up");
			else
				print ("Down");
		}
	}

	public void ProcessUp() {
		bool[,] tmp = new bool[GameManager.Width, GameManager.Height];
		for (uint x = 1; x < GameManager.Height; x++) {
			for (uint y = 0; y < GameManager.Width; y++) {
				if (!HaveCard(x, y))			//没卡就不用处理了
					continue;
				uint dest_x = x-1;
				for (dest_x = x-1; dest_x >= 0 && dest_x < GameManager.Height; dest_x--) {
					if (tmp[dest_x, y])			// 被合并过了
						break;
					//print (string.Format("x:{0},y:{1}", x+1, y+1));
					//print (string.Format("destx:{0}, y:{1}", dest_x+1, y+1));
					if (HaveCard(dest_x, y) && GamePanel.Instance.GetCardNumber(x+1, y+1) != 
					    GamePanel.Instance.GetCardNumber(dest_x+1, y+1)) // 不相等
						break;
				}
				dest_x += 1;
				if (dest_x != x) {
					if (HaveCard(dest_x, y) && GamePanel.Instance.GetCardNumber(x+1, y+1) == GamePanel.Instance.GetCardNumber(dest_x+1, y+1))
						tmp[dest_x, y] = true;
					print ("MoveCard");
					GamePanel.Instance.ProcessMoveCard(x+1, y+1, dest_x+1, y+1);
					havecard[x, y] = false;
					havecard[dest_x, y] = true;
				}
			}
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
