// This Script only handles input to the Display Screen whenever a UI Button or Keyboard Key is pressed.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Basic : MonoBehaviour {

	public Text Display;
	public int Arraysize = 50;
	public int ScrollWidth = 0;

	public BasicHandler SuperScript;

	public RectTransform ScrollChild;
	public RectTransform ScrollDisplay;

	//private string MyString = "";
	private string TempChar = "";

	public char[] CharacterArray;

	//private float TempFloat = 0.0f;
	//private int TempNum = 0;
	//private int Count = 0;
	//private float result = 0.0f;

	//Errors Handler
	//private int NumDecimals = 0;
	//private string TempChar2 = "";
	//private string Operation = "";

	//private float[] TempFloatArray;

	/*void Start() {
		TempFloatArray = new float[Arraysize];
		Display.text = "";
	}*/

	/*public void QuickCheck() {
		if (MyString == ".") {
			MyString = "0.";
		}
	}*/

	void Start() {
		CharacterArray = new char[1000];
	}

	void Update() {
		CharacterArray = Display.text.ToCharArray();

		ScrollWidth = 0;
		for (int i = 0; i < CharacterArray.Length; i++) {
			if (CharacterArray[i].ToString() == null) {
				break;
			}
			ScrollWidth += 1;
		}

		ScrollChild.sizeDelta = new Vector2 (150 + (ScrollWidth * 100), ScrollChild.sizeDelta.y);
		ScrollDisplay.sizeDelta = new Vector2 (150 + (ScrollWidth * 100), ScrollDisplay.sizeDelta.y);

		if (Input.GetKeyDown(KeyCode.Alpha0)) {
			Button0();
		}
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			Button1();
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			Button2();
		}
		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			Button3();
		}
		if (Input.GetKeyDown(KeyCode.Alpha4)) {
			Button4();
		}
		if (Input.GetKeyDown(KeyCode.Alpha5)) {
			Button5();
		}
		if (Input.GetKeyDown(KeyCode.Alpha6)) {
			Button6();
		}
		if (Input.GetKeyDown(KeyCode.Alpha7)) {
			Button7();
		}
		if (Input.GetKeyDown(KeyCode.Alpha8)) {
			Button8();
		}
		if (Input.GetKeyDown(KeyCode.Alpha9)) {
			Button9();
		}
		if (Input.GetKeyDown(KeyCode.Period)) {
			Dot();
		}
		if (Input.GetKeyDown(KeyCode.Backspace)) {
			BackSpace();
		}
		if (Input.GetKeyDown(KeyCode.KeypadEnter)) {
			Equals();
		}
		if (Input.GetKeyDown(KeyCode.KeypadPlus)) {
			Add();
		}
		if (Input.GetKeyDown(KeyCode.KeypadMinus)) {
			Substract();
		}
		if (Input.GetKeyDown(KeyCode.KeypadMultiply)) {
			Multiply();
		}
		if (Input.GetKeyDown(KeyCode.KeypadDivide)) {
			Divide();
		}
		if (Input.GetKeyDown(KeyCode.E)) {
			Display.text += "E";
		}
		if (Input.GetKeyDown(KeyCode.T)) {
			Display.text = (100000000000000000000.0).ToString();
		}
	}

	public void Convert_Display () {
		Display.text += TempChar;
		//ConvertFloat();
	}

	/*public void Reset() {
		for (int i = 0; i < Arraysize; i++) {
			//TempFloatArray[i] = 0.0f;
			//Debug.Log(TempFloatArray[i]);
		}
		TempFloat = 0.0f;
		//TempFloatArray[0] = result;
		MyString = result.ToString();
	}*/

	public void Button0() {
		TempChar = "0";
		Convert_Display();
	}

	public void Button1() {
		TempChar = "1";
		Convert_Display();
	}

	public void Button2() {
		TempChar = "2";
		Convert_Display();
	}

	public void Button3() {
		TempChar = "3";
		Convert_Display();
	}

	public void Button4() {
		TempChar = "4";
		Convert_Display();
	}

	public void Button5() {
		TempChar = "5";
		Convert_Display();
	}

	public void Button6() {
		TempChar = "6";
		Convert_Display();
	}

	public void Button7() {
		TempChar = "7";
		Convert_Display();
	}

	public void Button8() {
		TempChar = "8";
		Convert_Display();
	}

	public void Button9() {
		TempChar = "9";
		Convert_Display();
	}

	public void Dot() {
		SuperScript.Decimal();
	}

	public void BackSpace() {
		SuperScript.BackSpace();
	}

	public void Equals() {
		SuperScript.EqualTo();
	}

	public void Add() {
		TempChar = "+";
		Convert_Display();
		SuperScript.NumDecimals = 0;	
	}

	public void Substract() {
		TempChar = "-";
		Convert_Display();
		SuperScript.NumDecimals = 0;
	}

	public void Multiply() {
		TempChar = "*";
		Convert_Display();
		SuperScript.NumDecimals = 0;
	}

	public void Divide() {
		TempChar = "/";
		Convert_Display();
		SuperScript.NumDecimals = 0;
	}

	/*public void Debugger() {
		Debug.Log("TempFloat: " + TempFloat);
		Debug.Log("MyString: " + MyString);
		for (int i = 0; i < 5; i++) {
			Debug.Log("Array " + i + ": " + TempFloatArray[i]);
		}
		Debug.Log("Count:" + Count);
	}*/
}
