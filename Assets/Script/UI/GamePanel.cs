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

	public float movespeed = 0.08f;
	public int movenum = 0;
	public bool until = true;

	public GamePanel() {
	}

	public void ResetPanel() {
		foreach(KeyValuePair<LogicPos, RectTransform> p in CardList) {
			GameObject.Destroy(p.Value.gameObject);
		}
		CardList.Clear();

		for(int i = 0; i < 2; i++) {
			ProduceACard();
		}
	}

	public void ProduceACard() {
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

	public void ProcessMoveCard() {
		movenum = 0;
		List<RectTransform> list = new List<RectTransform>();
		foreach (KeyValuePair<LogicPos, RectTransform> p in CardList) {
			var script = p.Value.GetComponent<ObjCard>();
			if (!script.destroy)
				list.Add(p.Value);
			if (script.NeedtoMove()) {
				StartCoroutine(MoveCard(p.Value));
				movenum = 1;
			}
		}
		CardList.Clear();
		foreach(RectTransform rect in list) {
			var script = rect.GetComponent<ObjCard>();
			var logicpos = new LogicPos(script.posX, script.posY);
			CardList[logicpos] = rect;
		}
		list.Clear();
		if (movenum > 0)
			until = false;
	}

	IEnumerator MoveCard(RectTransform card) {
		var script = card.GetComponent<ObjCard>();
		var currentpos = card.anchoredPosition;
		var tagpos = GetObjPosition(script.posX, script.posY);
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
		var logicpos = new LogicPos(script.posX, script.posY);
		if (script.destroy) {
			CardList[logicpos].GetComponent<ObjCard>().Number = GameManager.Instance.GetCardNumber(script.posX-1, script.posY-1);;
			Destroy(card.gameObject);
		} else {
			card.anchoredPosition = tagpos;
			script.preposX = script.posX;
			script.preposY = script.posY;
			script.Number = GameManager.Instance.GetCardNumber(script.posX-1, script.posY-1);
			CardList[logicpos] = card;
		}
		MoveFinish();
	}

	public void MoveFinish() {
		if (movenum <= 0) {
			return;
		}
		movenum -= 1;
		if (movenum <= 0) {
			movenum = 0;
			ProduceACard();
			until = true;
		}
	}

	public void SetCardTagPos(uint cardPosX, uint cardPosY, uint tagPosX, uint tagPosY, bool destroy) {
		var taglogic = new LogicPos(cardPosX, cardPosY);
		if (CardList.ContainsKey(taglogic)) {
			var objcard = CardList[taglogic].GetComponent<ObjCard>();
			objcard.preposX = cardPosX;
			objcard.preposY = cardPosY;
			objcard.posX = tagPosX;
			objcard.posY = tagPosY;
			objcard.destroy = destroy;
		}
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
		if (!until) 
			return;
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
