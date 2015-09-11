using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ObjCard : MonoBehaviour {

	public Text numtext;
	public uint number;
	public uint posX;
	public uint posY;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Init(uint posx, uint posy, uint num = 2) {
		posX = posx;
		posY = posy;
		Number = num;
		numtext.text = string.Format("{0}", Number);
	}

	public uint Number {
		set {
			if (value % 2 != 0) {
				throw new UnityException("nuber must be %2 == 0");
			}
			number = value;
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
}
