using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

struct LogicPos {
	public uint posX;
	public uint posY;

	public LogicPos(uint x, uint y) {
		posX = x;
		posY = y;
	}
}

public class GamePanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	static private GamePanel _instance;
	
	public static GamePanel Instance {
		get { return _instance; }
	}

	private Dictionary<LogicPos, RectTransform> CardList = new Dictionary<LogicPos, RectTransform>();
	//List<RectTransform> CardList = new List<RectTransform>();
	
	public GameObject prefabNumber;
	private Vector2 pressPosition;

	public float movespeed = 0.01f;

	public GamePanel() {
	}

	public void ResetPanel() {
		foreach(KeyValuePair<LogicPos, RectTransform> p in CardList) {
			GameObject.Destroy(p.Value.gameObject);
		}
		CardList.Clear();

		for(int i = 0; i < 2; i++) {
			uint posX, posY;
			if(GameManager.Instance.ProduceCard(out posX, out posY)) {
				var numcard = Instantiate(prefabNumber).GetComponent<RectTransform>();
				var script = numcard.GetComponent<ObjCard>();
				script.Init(posX, posY);
				numcard.SetParent(gameObject.transform);
				numcard.anchoredPosition = GetObjPosition(posX, posY);
				var logicpos = new LogicPos(posX, posY);
				CardList[logicpos] = numcard;
			}
		}
	}

	public void ProcessMoveCard(uint currentPosX, uint currentPosY, uint tagPosX, uint tagPosY) {
		var currentPos = new LogicPos(currentPosX, currentPosY);
		if (!CardList.ContainsKey(currentPos)) {
			print ("MoveCard Error CurrentPos");
			return;
		}
		var card = CardList[currentPos];
		CardList.Remove(currentPos);
		StartCoroutine(MoveCard(card, tagPosX, tagPosY));
	}

	IEnumerator MoveCard(RectTransform card, uint tagPosX, uint tagPosY) {
		var currentpos = card.anchoredPosition;
		var tagpos = GetObjPosition(tagPosX, tagPosY);
		var speed_x = (tagpos.x - currentpos.x) / movespeed;
		var speed_y = (tagpos.y - currentpos.y) / movespeed;
		bool stop = false;
		var movetime = 0.0f;
		while (!stop) {
			movetime += Time.deltaTime;
			if (movetime > movespeed) {
				stop = true;
			}
			card.anchoredPosition = new Vector2(card.anchoredPosition.x + speed_x * Time.deltaTime, card.anchoredPosition.y + speed_y * Time.deltaTime);
			yield return null;
		}
		var taglogic = new LogicPos(tagPosX, tagPosY);
		if (CardList.ContainsKey(taglogic)) {
			CardList[taglogic].GetComponent<ObjCard>().Number *= 2;
			Destroy(card.gameObject);
		} else {
			card.GetComponent<ObjCard>().posX = tagPosX;
			card.GetComponent<ObjCard>().posY = tagPosY;
			card.anchoredPosition = tagpos;
			CardList[taglogic] = card;
		}
	}

	public uint GetCardNumber(uint posX, uint posY) {
		LogicPos pos = new LogicPos(posX, posY);
		var script = CardList[pos].GetComponent<ObjCard>();
		return script.Number;
	}

	public Vector2 GetObjPosition(uint PosX, uint PosY) {
		var index = (PosX - 1) * GameManager.Width + PosY;
		var gameobj = GameObject.Find(string.Format("Bg_Obj{0}", index));
		if (gameobj == null) {
			throw new UnityException(string.Format("gameobj Bg_Obj{0} cannot found!", index));
		}
		return gameobj.GetComponent<RectTransform>().anchoredPosition;
	}

	public void OnPointerDown(PointerEventData eventdata) {
		pressPosition = eventdata.position;
	}

	public void OnPointerUp(PointerEventData eventdata) {
		Vector2 pointerUpPosition = eventdata.position;
		GameManager.Instance.OnPointerMove(pressPosition, pointerUpPosition);
	}

	void Awake() {
		_instance = this;
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}


}
