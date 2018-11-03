using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicHandler : MonoBehaviour {

	public Text Display;

	public int stopUpdate = 0;
	public int NumDecimals = 0;
	public int AddOnce = 0;
	public string FirstNum = "0";
	public string tempchar;
	public string[] StringArray;
	public char[] CharacterArray;

	public char[] ExponentialArray;
	public string[] ExponentialArrayX;
	public bool r_case;
	public string ExpOperation;
	public string tempExpResult;
	public string powerOfTen;
	public string tempLength;
	public int Exp_Pos;

	public double Result;

	public int AddCount = 0;
	public string[] AddArray;
	public string AddResult_temp;

	public int SubstractCount = 0;
	public string[] SubstractArray;
	public string SubstractResult_temp;

	public string m_process;
	public string m_temp1;
	public string m_temp2;
	public int m_case = 0;
	public float m_tempResult;
	
	public string d_process;
	public string d_temp1;
	public string d_temp2;
	public int d_case = 0;
	public float d_tempResult;

	public int MD_case = 0;

	void Awake() {
		Display.text = "";
	}

	void Start() {
		CharacterArray = new char[16384];
		StringArray = new string[16384];
		AddArray = new string[16384];
		SubstractArray = new string[16384];
		ExponentialArrayX = new string[16384];
	}

	void Update() {
		if (stopUpdate == 0) {
			CharacterArray = Display.text.ToCharArray();
			for (int i = 0; i < CharacterArray.Length; i++) {
				if (CharacterArray[i].ToString() == null) {
					break;
				}
				StringArray[i] = CharacterArray[i].ToString();
			}
		}
	}

	public void Decimal() {	
		if (NumDecimals == 0) {
			Display.text += ".";
			NumDecimals = 1;
		}
	}

	public void EqualTo() {
		stopUpdate = 1;
		RepairString();
		for (int i = 0; i < StringArray.Length; i++) {
			if (StringArray[i] == null) {
				break;
			}
			if (StringArray[i] == "*") {
				Debug.Log("Multiplying...");
				Multiplication(i);
			}
			if (StringArray[i] == "/") {
				//Debug.Log("Dividing...");
			 	Division(i);
			}
		}
		//Debug.Log("StringArray[0]: " + StringArray[0]);
		for (int i = 0; i < StringArray.Length; i++) {
			if (StringArray [i] == null) {
				break;
			}
			if (StringArray[i] == "E") {
				i += 2;
			} else if ((StringArray[i] == "+") || (StringArray[i] == "-") || (StringArray[i] == "*") || (StringArray[i] == "/")) {
				if (i == 0) {
					break;
				}
				for (int k = 0; k < i; k++) {
					if (AddOnce == 0) {
						FirstNum += StringArray[k];
						//Debug.Log(StringArray[k]);
					}
				}
				AddOnce = 1;
			}
		}
		FirstNum = ClarifyExponential(FirstNum, false);
		//Debug.Log("FirstNum: " + FirstNum);
		for (int i = 0; i < StringArray.Length; i++) {
			if (StringArray[i] == null) {
				break;
			}
			if (StringArray[i] == "E") {
				i += 2;
			}
			if (StringArray[i] == "+") {
				i += 1;
				//Debug.Log("Adding...");
				Addition(i);
			}
			if (StringArray[i] == "-") {
				i += 1;
				//Debug.Log("Substracting...");
				Substraction(i);
			}
		}
		//Debug.Log("Performing Operation...");
		PerformOperation();
		//Debug.Log("Operation Done.");
		//Debug.Log("Result: " + Result.ToString());
		Display.text = Result.ToString();
		//Debug.Log("Displayed!");
		Reset();
		//Debug.Log("Resetted.");
	}

	public void BackSpace() {
		tempchar = Display.text.Substring(Display.text.Length - 1);

		if (tempchar == ".") {
			NumDecimals = 0;
		}

		if (Display.text.Length == 1) {
			Display.text = "";
		} else if ((tempchar == "+") || (tempchar == "-") || (tempchar == "*") || (tempchar == "/")) {
			Display.text = Display.text.Remove(Display.text.Length - 1);
			NumDecimals = 0;
			for (int i = StringArray.Length - 1; i >= 0; i--) {
				if (StringArray[i] == null) {
					break;
				}
				if ((StringArray[i] != "+") && (StringArray[i] != "-") && (StringArray[i] != "*") && (StringArray[i] != "/")) {
					tempchar = StringArray[i];
					if (tempchar == ".") {
						NumDecimals = 1;
					}
				}
			}
		} else {
			Display.text = Display.text.Remove(Display.text.Length - 1);
		}
	}

	public string ClarifyExponential(string thisString, bool Convert) {
		ExponentialArray = thisString.ToCharArray();
		tempExpResult = "";
		tempLength = "";
		powerOfTen = "";
		Exp_Pos = 0;
		r_case = false;
		for (int x = 0; x < ExponentialArray.Length; x++) {
			if (ExponentialArray[x].ToString() == "E") {
				Exp_Pos = x;
				if (ExponentialArray[x + 1].ToString() == "+") {
					ExpOperation = "+";
					for (int y = x + 2; y < ExponentialArray.Length; y++) {
						powerOfTen += ExponentialArray[y].ToString();
					}
				} else {
					ExpOperation = "-";
					for (int y = x + 2; y < ExponentialArray.Length; y++) {
						powerOfTen += ExponentialArray[y].ToString();
						//Debug.Log("Power of Ten: " + powerOfTen);
					}
				}
				break; 
			} else {
				tempExpResult += ExponentialArray[x].ToString();
			}
		}

		/*if ((Convert) && (tempExpResult.Length > 4)) {
			if (Exp_Pos != 0) {
				for (int w = 0; w < Exp_Pos; w++) {
					if ((ExponentialArray[w].ToString() == ".") && (ExpOperation == "+")) {
						powerOfTen = (int.Parse(powerOfTen) - (Exp_Pos - (w + 1))).ToString();
					} else if ((ExponentialArray[w].ToString() == ".") && (ExpOperation == "-")) {
						powerOfTen = (int.Parse(powerOfTen) + (Exp_Pos - (w + 1))).ToString();
					} else {
						tempExpResult += ExponentialArray[w].ToString();
					}
				}
			}

			tempLength = (tempExpResult.Length).ToString();
			ExponentialArray = tempExpResult.ToCharArray();
			tempExpResult = "";
			for (int i = 0; i < ExponentialArray.Length; i++) {
				if (ExponentialArray[i].ToString() == null) {
					break;
				}
				ExponentialArrayX[i] = ExponentialArray[i].ToString();
			}

			for (int m = 3; m >= 0; m--) {
				if (((int.Parse(ExponentialArrayX[m].ToString()) >= 5) || (r_case)) && (m > 0) && (ExponentialArrayX[m - 1].ToString() != "9")) {
					ExponentialArrayX[m - 1] = (int.Parse(ExponentialArrayX[m - 1].ToString()) + 1);
					r_case = false;
				} else if (((int.Parse(ExponentialArrayX[m].ToString()) >= 5) || (r_case)) && (m > 0) && (ExponentialArrayX[m - 1].ToString() == "9")) {
					ExponentialArrayX[m - 1] = '0';
					r_case = true;
				}
			}

			for (int n = 0; n <= 3; n++) {
				if (r_case) {
					tempExpResult += (10).ToString();
					r_case = false;
					n += 1;
				}
				if (n == 1) {
					tempExpResult += ".";
				}
				tempExpResult += ExponentialArrayX[n].ToString();
			}

			if (int.Parse(powerOfTen) != 0) {
				if (Operation == "+") {
					tempExpResult = tempExpResult + "E+" + (int.Parse(powerOfTen) + 1).ToString();
				} else {
					tempExpResult = tempExpResult + "E-" + (int.Parse(powerOfTen) + 1).ToString();
				}
			}

			return tempExpResult;
		}*/

		if (Exp_Pos != 0) {
			tempExpResult = "";
		} else if (Exp_Pos == 0) {
			return tempExpResult;
		}
		for (int w = 0; w < Exp_Pos; w++) {
			if ((ExponentialArrayX[w].ToString() == ".") && (ExpOperation == "+")) {
				powerOfTen = (int.Parse(powerOfTen) - (Exp_Pos - (w + 1))).ToString();
			} else if ((ExponentialArrayX[w].ToString() == ".") && (ExpOperation == "-")) {
				powerOfTen = (int.Parse(powerOfTen) + (Exp_Pos - (w + 1))).ToString();
			} else {
				tempExpResult += ExponentialArrayX[w].ToString();
			}
		}
		//Debug.Log("Power of Ten: " + powerOfTen);
		if ((ExpOperation == "+") && (int.Parse(powerOfTen) > 0)) {
			for (int z = 1; z <= int.Parse(powerOfTen); z++) {
				tempExpResult = tempExpResult + "0";
			}
		} else if ((ExpOperation == "-") && (int.Parse(powerOfTen) > 0)) {
			for (int z = 1; z < int.Parse(powerOfTen); z++) {
				tempExpResult = "0" + tempExpResult;
			}
			tempExpResult = "0." + tempExpResult;
		}

		return tempExpResult;
		//Debug.Log("tempExpResult: " + tempExpResult.ToString());
	}

	/*public string StandardForm (string s_convert) {
		s_convert = ClarifyExponential(s_convert, false);
		StandardArray = s_convert.ToCharArray();
		Exponent = 0;
		s_Operation = "+";
		for (int i = 0; i < StandardArray.Length; i++) {
			if (StandardArray[i] == ".") {
				for (int j = (i + 1); j < StandardArray.Length; j++) {
					if (StandardArray[j] != 0) {
						for (int k = j; k < StandardArray.Length; k++) {
							CutPart += StandardArray[k];
						}
						s_Operation = "-";
						Exponent = (StandardArray.Length - (i + 1)) - 3;
					}
				}
			}
		}

		BrokenArray = CutPart.ToCharArray();
		if ((s_Operation == "-") && (BrokenArray.Length >= 2)) {
			CutPart = BrokenArray[0] + ".";
			for (int i = 1; i < 4; i++) {
				if (BrokenArray[i] == null) {
					break;
				}
				if (i == 3) {
					if (int.Parse(BrokenArray[4]) > 4) {
						if ((int.Parse(BrokenArray[3]) + 1) >= 10) {
							CutPart += "0";
						} else {
							CutPart += (int.Parse(BrokenArray[3]) + 1).ToString();
						}
					} else {
						CutPart += BrokenArray[i].ToString();
					}
					break;
				} else {
					CutPart += BrokenArray[i].ToString();
				}
			}

		}
	}*/

	public void Addition(int i) {
		for (int j = i; j < StringArray.Length; j++) {
			if (StringArray[j] == null) {
				break;
			}
			if (((StringArray[j] != "+") && (StringArray[j] != "-") && (StringArray[j] != "*") && (StringArray[j] != "/")) || (((StringArray[j - 1]) == "E") && (j > 1))) {
				AddArray[AddCount] += StringArray[j];
			} else {
				break;
			}
			//Debug.Log(AddArray[AddCount] + " at position: " + AddCount);
		}
		//Debug.Log("Addition Over - Part 1.");
		AddArray[AddCount] = ClarifyExponential(AddArray[AddCount], false);
		//Debug.Log("Addition Over - Part 2.");
		AddCount += 1;		
	}

	public void Substraction(int i) {
		for (int j = i; j < StringArray.Length; j++) {
			if (StringArray[j] == null) {
				break;
			}
			if (((StringArray[j] != "+") && (StringArray[j] != "-") && (StringArray[j] != "*") && (StringArray[j] != "/")) || (((StringArray[j - 1]) == "E") && (j > 1))) {
				SubstractArray[SubstractCount] += StringArray[j];
			} else {
				break;
			}
		}
		SubstractArray[SubstractCount] = ClarifyExponential(SubstractArray[SubstractCount], false);
		SubstractCount += 1;		
	}

	public void Multiplication(int i) {
		StringArray[i] = "0";
		m_temp1 = "";
		m_temp2 = "";
		for (int j = i; j >= 0; j--) {
			if (j == 0) {
				m_process = "+";
				AddOnce = 1;
				for (int k = j; k < i; k++) {
					m_temp1 += StringArray[k];
					StringArray[k] = "0";
				}
				break;
			}
			if (StringArray[j - 1] == "E") {
				j -= 2;
			}
			if (j == 0) {
				m_process = "+";
				AddOnce = 1;
				for (int k = j; k < i; k++) {
					m_temp1 += StringArray[k];
					StringArray[k] = "0";
				}
				break;
			}
			if (MD_case == 1) {
				m_process = d_process;
				m_temp1 = d_tempResult.ToString();
				MD_case = 0;
				break;
			}
			if (m_case == 1) {
				m_temp1 = m_tempResult.ToString();
				m_case = 0;
				break;
			}
			if (StringArray[j] == "+") {
				j += 1;
				m_process = "+";
				for (int k = j; k < i; k++) {
					m_temp1 += StringArray[k];
					StringArray[k] = "0";
				}
				break;
			}
			if (StringArray[j] == "-") {
				j += 1;
				m_process = "-";
				for (int k = j; k < i; k++) {
					m_temp1 += StringArray[k];
					StringArray[k] = "0";
				}
				break;
			}
		}

		//Debug.Log("Half Multiplication Reached.");

		i += 1;
		if (StringArray[i] == m_process) {
			m_process = "+";
			StringArray[i] = "0";
		} else if ((StringArray[i] == "+") || (StringArray[i] == "-")) {
			m_process = "-";
			StringArray[i] = "0";
		}
		for (int k = i; k < StringArray.Length; k++) {
			m_temp2 = "";
			if (StringArray[k] == null) {
				break;
			}
			if (StringArray[k] == "*") {
				m_case = 1;
			}
			if (StringArray[k] == "/") {
				MD_case = 1;
			}
			if (StringArray[k] == "E") {
				k += 2;
			}
			if ((StringArray[k] == "+") || (StringArray[k] == "-") || (StringArray[k] == "*") || (StringArray[k] == "/")) {
				//Debug.Log("In Loop.");
				for (int l = i; l < k; l++) {
					//Debug.Log(StringArray[l]);
					m_temp2 += StringArray[l];
					StringArray[l] = "0";
				}
				break;
			}		
		}
		if (m_temp2 == "") {
			for (int k = i; k < StringArray.Length; k++) {
				m_temp2 += StringArray[k];
				StringArray[k] = "0";
			}
		}

		//Debug.Log(m_temp1 + " before_1");
		m_temp1 = ClarifyExponential(m_temp1, false);
		//Debug.Log(m_temp1 + " after_1");
		//Debug.Log(m_temp2 + " before_2");
		m_temp2 = ClarifyExponential(m_temp2, false);
		//Debug.Log(m_temp2 + " after_2");

		m_tempResult = float.Parse(m_temp1) * float.Parse(m_temp2);

		//Debug.Log("Temp1: " + m_temp1);
		//Debug.Log("Temp2: " + m_temp2);
		//Debug.Log("TempResult: " + m_tempResult.ToString());

		if ((m_process == "+") && (m_case == 0) && (MD_case == 0)) {
			AddArray[AddCount] = m_tempResult.ToString();
			//Debug.Log(AddArray[AddCount]);
			AddCount += 1;
		}

		if ((m_process == "-") && (m_case == 0) && (MD_case == 0)) {
			SubstractArray[SubstractCount] = m_tempResult.ToString();
			//Debug.Log(SubstractArray[SubstractCount]);
			SubstractCount += 1;
		}		
	}
	
	public void Division(int i) {
		StringArray[i] = "0";
		d_temp1 = "";
		d_temp2 = "";
		for (int j = i; j >= 0; j--) {
			if (j == 0) {
				d_process = "+";
				AddOnce = 1;
				for (int k = j; k < i; k++) {
					d_temp1 += StringArray[k];
					StringArray[k] = "0";
				}
				break;
			}
			if (StringArray[j - 1] == "E") {
				j -= 2;
			}
			if (j == 0) {
				d_process = "+";
				AddOnce = 1;
				for (int k = j; k < i; k++) {
					d_temp1 += StringArray[k];
					StringArray[k] = "0";
				}
				break;
			}
			if (MD_case == 1) {
				d_process = m_process;
				d_temp1 = m_tempResult.ToString();
				MD_case = 0;
				break;
			}
			if (d_case == 1) {
				d_temp1 = d_tempResult.ToString();
				d_case = 0;
				break;
			}
			if (StringArray[j] == "+") {
				j += 1;
				d_process = "+";
				for (int k = j; k < i; k++) {
					d_temp1 += StringArray[k];
					StringArray[k] = "0";
				}
				break;
			}
			if (StringArray[j] == "-") {
				j += 1;
				d_process = "-";
				for (int k = j; k < i; k++) {
					d_temp1 += StringArray[k];
					StringArray[k] = "0";
				}
				break;
			}
		}

		i += 1;
		if (StringArray[i] == d_process) {
			d_process = "+";
			StringArray[i] = "0";
		} else if ((StringArray[i] == "+") || (StringArray[i] == "-")) {
			d_process = "-";
			StringArray[i] = "0";
		}
		for (int k = i; k < StringArray.Length; k++) {
			d_temp2 = "";
			if (StringArray[k] == null) {
				break;
			}
			if (StringArray[k] == "/") {
				d_case = 1;
			}
			if (StringArray[k] == "*") {
				MD_case = 1;
			}
			if (StringArray[k] == "E") {
				k += 2;
			}
			if ((StringArray[k] == "+") || (StringArray[k] == "-") || (StringArray[k] == "*") || (StringArray[k] == "/")) {
				for (int l = i; l < k; l++) {
					d_temp2 += StringArray[l];
					StringArray[l] = "0";
				}
				break;
			}
		}
		if (d_temp2 == "") {
			for (int k = i; k < StringArray.Length; k++) {
				d_temp2 += StringArray[k];
				StringArray[k] = "0";
			}
		}

		d_temp1 = ClarifyExponential(d_temp1, false);
		d_temp2 = ClarifyExponential(d_temp2, false);

		d_tempResult = float.Parse(d_temp1) / float.Parse(d_temp2);

		Debug.Log("d_Temp1: " + d_temp1);
		Debug.Log("d_Temp2: " + d_temp2);
		//Debug.Log("TempResult: " + m_tempResult.ToString());

		if ((d_process == "+") && (d_case == 0) && (MD_case == 0)) {
			AddArray[AddCount] = d_tempResult.ToString();
			//Debug.Log(AddArray[AddCount]);
			AddCount += 1;
		}

		if ((d_process == "-") && (d_case == 0) && (MD_case == 0)) {
			SubstractArray[SubstractCount] = d_tempResult.ToString();
			//Debug.Log(SubstractArray[SubstractCount]);
			SubstractCount += 1;
		}		
	}
	

	public void RepairString() {
		for (int i = StringArray.Length - 1; i > 0; i--) {
			if ((StringArray[i] != "") && (StringArray[i] != null)) {
				if ((StringArray[i] == "+") || (StringArray[i] == "-")) {
					if (StringArray[i - 1] == StringArray[i]) {
						StringArray[i] = "0";
						StringArray[i - 1] = "+";
					}
					if ((StringArray[i] == "+") && (StringArray[i - 1] == "-")) {
						StringArray[i] = "0";
						StringArray[i - 1] = "-";
					}
					if ((StringArray[i] == "-") && (StringArray[i - 1] == "+")) {
						StringArray[i] = "0";
						StringArray[i - 1] = "-";
					}  
				}
			}
		}
	}

	public void PerformOperation() {
		for (int i = 0; i < AddArray.Length; i++) {
			Debug.Log("Add_Array: " + i + " " + AddArray[i]);
			if ((AddArray[i] == null) || (AddArray[i] == "")) {
				break;
			} else {
				AddResult_temp += AddArray[i];
				Result += double.Parse(AddArray[i]);
			}
			//Debug.Log("Result: " + Result);
			//Debug.Log("AddResult in String: " + AddResult_temp);
		}
		for (int i = 0; i < SubstractArray.Length; i++) {
			//Debug.Log("Substract_Array: " + i + " " + SubstractArray[i]);
			if ((SubstractArray[i] == null) || (SubstractArray[i] == "")) {
				break;
			} else {
				SubstractResult_temp += SubstractArray[i];
				Result -= double.Parse(SubstractArray[i]);
			}
			//Debug.Log("Result: " + Result);
		}
		Result += double.Parse(FirstNum);
	}

	public void Reset() {
		for (int i = 0; i < AddArray.Length; i++) {
			if (AddArray[i] == null) {
				break;
			}
			AddArray[i] = ""; 
		}
		for (int i = 0; i < SubstractArray.Length; i++) {
			if (SubstractArray[i] == null) {
				break;
			}
			SubstractArray[i] = ""; 
		}
		for (int i = 0; i < StringArray.Length; i++) {
			if (StringArray[i] == null) {
				break;
			}
			StringArray[i] = "";
		}
		FirstNum = "0";
		stopUpdate = 0;
		Update();
		NumDecimals = 0;
		for (int i = 0; i < CharacterArray.Length; i++) {
			if (CharacterArray[i].ToString() == null) {
				break;
			}
			if (CharacterArray[i].ToString() == ".") {
				NumDecimals = 1;
			}
		}
		AddOnce = 0;
		AddCount = 0;
		AddResult_temp = "";
		SubstractCount = 0;
		SubstractResult_temp = "";
		m_tempResult = 0.0f;
		d_tempResult = 0.0f;
		Result = 0.0d;
	}
}