using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drumstick : MonoBehaviour {

	public enum HandType{
		none = 0, left, right
	}

	public HandType handController = HandType.none; 
}
