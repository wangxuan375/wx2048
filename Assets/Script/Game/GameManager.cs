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

	public uint[,] cardnum;

	public GameManager() {
		cardnum = new uint[GameManager.Width, GameManager.Height];
	}

	public bool HaveCard(uint posX, uint posY) {
		if (posX < 0 || posX >= Width || posY < 0 || posY >= Height) {
			print (string.Format("What!!! Error Position [{0}, {1}]", posX, posY));
			return true;
		}
		return (cardnum[posX, posY] > 0);
	}

	public bool ProduceCard(out uint posX, out uint posY) {
		posX = 0;
		posY = 0;
		uint randcount = 0;
		do {
			posX = (uint)Random.Range((int)0, GameManager.Width);
			posY = (uint)Random.Range((int)0, GameManager.Height);
			randcount += 1;
			if (randcount > 100) {
				print ("rand error");
				return false;
			}
			//print (string.Format("[{0}, {1}]", posX, posY));
		} while(HaveCard(posX, posY));
		cardnum[posX, posY] = 2;
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
				ProcessRight();//print ("right");
			else
				ProcessLeft();//print ("left");
		} else {
			if (vec.y > 0) 
				ProcessUp();//print ("Up");
			else
				ProcessDown();//print ("Down");
		}
		GamePanel.Instance.ProcessMoveCard();
	}

	public void ProcessUp() {
		bool[,] merge = new bool[GameManager.Width, GameManager.Height];
		for (uint x = 1; x < GameManager.Height; x++) {
			for (uint y = 0; y < GameManager.Width; y++) {
				if (!HaveCard(x, y))			//没卡就不用处理了
					continue;
				uint dest_x = x-1;
				for (dest_x = x-1; dest_x >= 0 && dest_x < GameManager.Height; dest_x--) {
					if (merge[dest_x, y])			// 被合并过了
						break;
					if (HaveCard(dest_x, y) && GetCardNumber(x, y) != GetCardNumber(dest_x, y)) // 不相等
						break;
				}
				dest_x += 1;
				if (dest_x != x) {
					bool destroy = false;
					if (HaveCard(dest_x, y) && GetCardNumber(x, y) == GetCardNumber(dest_x, y)) {
						merge[dest_x, y] = true;
						destroy = true;
					}
					cardnum[dest_x, y] += cardnum[x, y];
					cardnum[x, y] = 0;
					GamePanel.Instance.SetCardTagPos(x+1, y+1, dest_x+1, y+1, destroy);
				}
			}
		}
	}

	public void ProcessDown() {
		bool[,] merge = new bool[GameManager.Width, GameManager.Height];
		for (uint x = GameManager.Height - 2; x >= 0 && x < GameManager.Height; x--) {
			for (uint y = 0; y < GameManager.Width; y++) {
				if (!HaveCard(x, y))			//没卡就不用处理了
					continue;
				uint dest_x = x+1;
				for (dest_x = x+1; dest_x < GameManager.Height; dest_x++) {
					if (merge[dest_x, y])			// 被合并过了
						break;
					if (HaveCard(dest_x, y) && GetCardNumber(x, y) != GetCardNumber(dest_x, y)) // 不相等
						break;
				}
				dest_x -= 1;
				if (dest_x != x) {
					bool destroy = false;
					if (HaveCard(dest_x, y) && GetCardNumber(x, y) == GetCardNumber(dest_x, y)) {
						merge[dest_x, y] = true;
						destroy = true;
					}
					cardnum[dest_x, y] += cardnum[x, y];
					cardnum[x, y] = 0;
					GamePanel.Instance.SetCardTagPos(x+1, y+1, dest_x+1, y+1, destroy);
				}
			}
		}
	}

	public void ProcessLeft() {
		bool[,] merge = new bool[GameManager.Width, GameManager.Height];
		for (uint y = 1; y < GameManager.Width; y++) {
			for (uint x = 0; x < GameManager.Height; x++) {
				if (!HaveCard(x, y))			//没卡就不用处理了
					continue;
				uint dest_y = y-1;
				for (dest_y = y-1; dest_y >= 0 && dest_y < GameManager.Width; dest_y--) {
					if (merge[x, dest_y])			// 被合并过了
						break;
					if (HaveCard(x, dest_y) && GetCardNumber(x, y) != GetCardNumber(x, dest_y)) // 不相等
						break;
				}
				dest_y += 1;
				if (dest_y != y) {
					bool destroy = false;
					if (HaveCard(x, dest_y) && GetCardNumber(x, y) == GetCardNumber(x, dest_y)) {
						merge[x, dest_y] = true;
						destroy = true;
					}
					cardnum[x, dest_y] += cardnum[x, y];
					cardnum[x, y] = 0;
					GamePanel.Instance.SetCardTagPos(x+1, y+1, x+1, dest_y+1, destroy);
				}
			}
		}
	}
	
	public void ProcessRight() {
		bool[,] merge = new bool[GameManager.Width, GameManager.Height];
		for (uint y = GameManager.Width - 2; y >= 0 && y < GameManager.Width; y--) {
			for (uint x = 0; x < GameManager.Height; x++) {
				if (!HaveCard(x, y))			//没卡就不用处理了
					continue;
				uint dest_y = y+1;
				for (dest_y = y+1; dest_y < GameManager.Width; dest_y++) {
					if (merge[x, dest_y])			// 被合并过了
						break;
					if (HaveCard(x, dest_y) && GetCardNumber(x, y) != GetCardNumber(x, dest_y)) // 不相等
						break;
				}
				dest_y -= 1;
				if (dest_y != y) {
					bool destroy = false;
					if (HaveCard(x, dest_y) && GetCardNumber(x, y) == GetCardNumber(x, dest_y)) {
						merge[x, dest_y] = true;
						destroy = true;
					}
					cardnum[x, dest_y] += cardnum[x, y];
					cardnum[x, y] = 0;
					GamePanel.Instance.SetCardTagPos(x+1, y+1, x+1, dest_y+1, destroy);
				}
			}
		}
	}

	public uint GetCardNumber(uint posx, uint posy) {
		return cardnum[posx, posy];
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
