using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ObjCard : MonoBehaviour {

	public Text numtext;
	public uint number;
	public uint posX;
	public uint posY;

	public uint preposX;
	public uint preposY;

	public bool destroy;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Init(uint posx, uint posy, uint num = 2) {
		posX = preposX = posx;
		posY = preposY = posy;
		Number = num;
		destroy = false;
	}

	public uint Number {
		set {
			if (value % 2 != 0) {
				throw new UnityException("nuber must be %2 == 0");
			}
			number = value;
			numtext.text = string.Format("{0}", Number);
			/* todo 根据数字改变颜色
			var image = gameObject.GetComponent<Image>();
			if (number == 2)
				image.color = Color.white;
				*/
		}
		get {
			return number;
		}
	}

	public bool NeedtoMove() {
		if (preposX != posX || preposY != posY) {
			return true;
		}
		return false;
	}
}
