// This Script is to handle Basic operations such as Addition, Substraction, Multiplication and Division. 
// It also handles decimal points and standard forms.

// Concept: Convert each character on the display in char and store it in an array. Convert all that into an string array when 
//       	'=' is pressed. At the end, the result is based on 2 arrays. One for Adding and One for Substracting. Signs are checked
//			and multiplication/division symbols are eliminated to decide in which of the above arrays a particular number goes.

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

	// Sets the Display where numbers are displayed to empty.
	void Awake() {
		Display.text = "";
	}

	// Initialize main arrays that will be used.
	void Start() {
		CharacterArray = new char[16384];
		StringArray = new string[16384];
		AddArray = new string[16384];
		SubstractArray = new string[16384];
		ExponentialArrayX = new string[16384];
	}

	// When the '=' key is pressed, everything on the display (each character) will be appended to a string array.
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

	// When the '.' key is pressed, add a decimal point if possible.
	public void Decimal() {	
		if (NumDecimals == 0) {
			Display.text += ".";
			NumDecimals = 1;
		}
	}

	// This is called whenever the '=' is pressed.
	public void EqualTo() {
		stopUpdate = 1; // So that it does not continue to convert the display to string array, look at the Multiplication/Division function to undertand why.
		RepairString(); // Will check if there are 2 or more signs beside each other and simplify that. (+-, -+, ++, --).
		for (int i = 0; i < StringArray.Length; i++) {
			if (StringArray[i] == null) {
				break; // End of Array.
			}
			if (StringArray[i] == "*") {
				Multiplication(i);
			}
			if (StringArray[i] == "/") {
			 	Division(i);
			}
		}
		for (int i = 0; i < StringArray.Length; i++) { // If theres' no '*' or '/', then the first number of the array needs to be found.
			if (StringArray [i] == null) {
				break;
			}
			if (StringArray[i] == "E") { // Too long numbers are converted in '<Number in standard form>E[+ or *]<Exponent>' by Unity itself.
				i += 2;					//  We Will convert it into a long number afterwards. 
			} else if ((StringArray[i] == "+") || (StringArray[i] == "-") || (StringArray[i] == "*") || (StringArray[i] == "/")) {
				if (i == 0) { // There needs to be no sign before a number if we want to proceed. Other functions will do the same thing if 
					break;    // there's a sign before. That part is just used to store the first num. (e.g. In '1 + 2 + 3', 1 will be stored here.)
				}
				for (int k = 0; k < i; k++) { // Will be storing the number from position 0 to the closest sign.
					if (AddOnce == 0) { // AddOnce is set by Multiplication or Division funtion if they've already taken care of the First Number. 
						FirstNum += StringArray[k]; 
					}
				}
				AddOnce = 1;
			}
		}
		FirstNum = ClarifyExponential(FirstNum, false); // Converts the number, if in exponential form to a really long number.
		for (int i = 0; i < StringArray.Length; i++) { // Checks if there's a '+' sign or a '-' sign and go through Addition/Substraction.
			if (StringArray[i] == null) {
				break;
			}
			if (StringArray[i] == "E") {
				i += 2;
			}
			if (StringArray[i] == "+") {
				i += 1;
				Addition(i);
			}
			if (StringArray[i] == "-") {
				i += 1;
				Substraction(i);
			}
		}
		PerformOperation(); // Function to calculate the final result.
		Display.text = Result.ToString();
		Reset();
	}

	public void BackSpace() { // Called whenever the user wishes to remove a character.
 		tempchar = Display.text.Substring(Display.text.Length - 1); // Gets the last character inputted, the rightmost one.

		if (tempchar == ".") { 
			NumDecimals = 0; // So that we can put a decimal point again if we erased it.
		}

		if (Display.text.Length == 1) { // If the display had only 1 character then logically it would now be blank.
			Display.text = "";
		} else if ((tempchar == "+") || (tempchar == "-") || (tempchar == "*") || (tempchar == "/")) {
			Display.text = Display.text.Remove(Display.text.Length - 1); // Erases that last character if it is a sign and tells the program that another decimal point can be entered.
			NumDecimals = 0;
			for (int i = StringArray.Length - 1; i >= 0; i--) {
				if (StringArray[i] == null) {
					break;
				}
				if ((StringArray[i] != "+") && (StringArray[i] != "-") && (StringArray[i] != "*") && (StringArray[i] != "/")) {
					tempchar = StringArray[i];
					if (tempchar == ".") {
						NumDecimals = 1; // After erasing the sign, if the number before had a decimal point, then we don't want that the user inputs a decimal point again.
					}
				}
			}
		} else {
			Display.text = Display.text.Remove(Display.text.Length - 1); // If no sign is to be erased, then erase whatever the last character is.
		}
	}

	public string ClarifyExponential(string thisString, bool Convert) { // Unity by default converts a long number into a standard form one. 
		ExponentialArray = thisString.ToCharArray();                   //  So we want to change that to a long number itself.
		tempExpResult = "";                                           //   The format is '<Number>E< + or - ><power of Ten>.'
		tempLength = "";
		powerOfTen = "";
		Exp_Pos = 0;
		r_case = false;
		for (int x = 0; x < ExponentialArray.Length; x++) {
			if (ExponentialArray[x].ToString() == "E") { // Is the number in the previouly mentioned format?
				Exp_Pos = x;
				if (ExponentialArray[x + 1].ToString() == "+") { // We want to multiply by 10 if the Power of Ten is a positive one.
					ExpOperation = "+";
					for (int y = x + 2; y < ExponentialArray.Length; y++) {
						powerOfTen += ExponentialArray[y].ToString(); // Store the Power of Ten here as it is the number after the 'E+'.
					}
				} else {
					ExpOperation = "-"; // We want to divide by 10 if the Power of Ten is a negative one.
					for (int y = x + 2; y < ExponentialArray.Length; y++) {
						powerOfTen += ExponentialArray[y].ToString(); // Store the Power of Ten here as it is the number after the 'E-'.
					}
				}
				break; 
			} else {
				tempExpResult += ExponentialArray[x].ToString(); // Store the num before the 'E' or if there's no 'E', the number passed as the parameter.
			}
		}

		if (Exp_Pos != 0) {
			tempExpResult = "";
		} else if (Exp_Pos == 0) { // There was no 'E'. Everything is fine. Return the number as it was passed.
			return tempExpResult;
		}
		for (int w = 0; w < Exp_Pos; w++) {
			if ((ExponentialArrayX[w].ToString() == ".") && (ExpOperation == "+")) {    // If there's a decimal point, substract the number of places needed
				powerOfTen = (int.Parse(powerOfTen) - (Exp_Pos - (w + 1))).ToString(); //  to be moved from the power of ten.
			} else if ((ExponentialArrayX[w].ToString() == ".") && (ExpOperation == "-")) { // If there's a decimal point, add the number of places needed
				powerOfTen = (int.Parse(powerOfTen) + (Exp_Pos - (w + 1))).ToString(); //  to be moved to the power of ten.
			} else {
				tempExpResult += ExponentialArrayX[w].ToString(); // No decimal point (yet)? Just append the other numbers then.
			}
		}
		if ((ExpOperation == "+") && (int.Parse(powerOfTen) > 0)) { // Power of ten is Positive! Just add '0s' at the end of the string.
			for (int z = 1; z <= int.Parse(powerOfTen); z++) {
				tempExpResult = tempExpResult + "0";
			}
		} else if ((ExpOperation == "-") && (int.Parse(powerOfTen) > 0)) { // Power of ten is Negative! Just add a decimal point and '0s' 
			for (int z = 1; z < int.Parse(powerOfTen); z++) {             //  at the start of the string.
				tempExpResult = "0" + tempExpResult;
			}
			tempExpResult = "0." + tempExpResult;
		}

		return tempExpResult;
	}

	public void Addition(int i) {
		for (int j = i; j < StringArray.Length; j++) {
			if (StringArray[j] == null) {
				break;
			}
			if (((StringArray[j] != "+") && (StringArray[j] != "-") && (StringArray[j] != "*") && (StringArray[j] != "/")) || (((StringArray[j - 1]) == "E") && (j > 1))) {
				AddArray[AddCount] += StringArray[j]; // Until we meet a sign or the end of the Array, add the number from the position of '+' to our AddArray.
			} else {
				break;
			}
		}
		AddArray[AddCount] = ClarifyExponential(AddArray[AddCount], false);
		AddCount += 1;		
	}

	public void Substraction(int i) {
		for (int j = i; j < StringArray.Length; j++) {
			if (StringArray[j] == null) {
				break;
			}
			if (((StringArray[j] != "+") && (StringArray[j] != "-") && (StringArray[j] != "*") && (StringArray[j] != "/")) || (((StringArray[j - 1]) == "E") && (j > 1))) {
				SubstractArray[SubstractCount] += StringArray[j]; // Until we meet a sign or the end of the Array, add the number from the position of '-' to our SubstractArray.
			} else {
				break;
			}
		}
		SubstractArray[SubstractCount] = ClarifyExponential(SubstractArray[SubstractCount], false);
		SubstractCount += 1;		
	}

	public void Multiplication(int i) { // We need to find out both the number before and after the '*' this time.
		StringArray[i] = "0";
		m_temp1 = "";
		m_temp2 = "";
		for (int j = i; j >= 0; j--) {
			if (j == 0) { // If we're at the first position and there's no sign, then the number is positive.
				m_process = "+";
				AddOnce = 1;
				for (int k = j; k < i; k++) { // Add that number to our variable.
					m_temp1 += StringArray[k];
					StringArray[k] = "0"; // Replace the position(s) that number took as '0'.
				}
				break;
			}
			if (StringArray[j - 1] == "E") { // Don't want errors due to that 'E' and its signs, eh?
				j -= 2;
			}
			if (j == 0) { // Why we repeat that check again? Check the if just above! Also, we can't put the if selection above on top as we
				m_process = "+"; // would get an error if the position 'j - 1' did not exist.
				AddOnce = 1;
				for (int k = j; k < i; k++) {
					m_temp1 += StringArray[k];
					StringArray[k] = "0";
				}
				break;
			}
			if (MD_case == 1) { // This variable is set to 1 if there's a multiplication after a division or vice-versa.
				m_process = d_process;
				m_temp1 = d_tempResult.ToString(); // The result of the division is set as the number before the multiplication sign then.
				MD_case = 0;
				break;
			}
			if (m_case == 1) { // This variable is set to one if there's another multiplication after the current one.
				m_temp1 = m_tempResult.ToString();
				m_case = 0;
				break;
			}
			if (StringArray[j] == "+") { // There's a '+' sign before the number before the '*' sign. To the Addition Array.
				j += 1;
				m_process = "+";
				for (int k = j; k < i; k++) {
					m_temp1 += StringArray[k];
					StringArray[k] = "0";
				}
				break;
			}
			if (StringArray[j] == "-") { // There's a '-' sign before the number before the '*' sign. To the Substraction Array.
				j += 1;
				m_process = "-";
				for (int k = j; k < i; k++) {
					m_temp1 += StringArray[k];
					StringArray[k] = "0";
				}
				break;
			}
		}

		i += 1;
		if (StringArray[i] == m_process) { // Checks what sign, if any, is found after the '*' sign to determine the end Array.
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
				for (int l = i; l < k; l++) { // Stores the number found after the '*' sign.
					m_temp2 += StringArray[l];
					StringArray[l] = "0";
				}
				break;
			}		
		}
		if (m_temp2 == "") { // Stores the number after the '*' sign if it was the last number inputted.
			for (int k = i; k < StringArray.Length; k++) {
				m_temp2 += StringArray[k];
				StringArray[k] = "0";
			}
		}

		// Represents the number in a normal format if they were in the standard form format.
		
		m_temp1 = ClarifyExponential(m_temp1, false);
		m_temp2 = ClarifyExponential(m_temp2, false);

		m_tempResult = float.Parse(m_temp1) * float.Parse(m_temp2); // Result of the Multiplication.

		if ((m_process == "+") && (m_case == 0) && (MD_case == 0)) { // Adds that Result to the Addition Array if it was a positive one.
			AddArray[AddCount] = m_tempResult.ToString();
			AddCount += 1;
		}

		if ((m_process == "-") && (m_case == 0) && (MD_case == 0)) { // Adds that Result to the Substraction Array if it was a negative one.
			SubstractArray[SubstractCount] = m_tempResult.ToString();
			SubstractCount += 1;
		}		
	}
	
	public void Division(int i) { // We need to find out both the number before and after the '/' this time.
		StringArray[i] = "0";
		d_temp1 = "";
		d_temp2 = "";
		for (int j = i; j >= 0; j--) {
			if (j == 0) { // If we're at the first position and there's no sign, then the number is positive.
				d_process = "+";
				AddOnce = 1;
				for (int k = j; k < i; k++) { // Add that number to our variable.
					d_temp1 += StringArray[k];
					StringArray[k] = "0"; // Replace the position(s) that number took as '0'.
				}
				break;
			}
			if (StringArray[j - 1] == "E") { // Don't want errors due to that 'E' and its signs, eh?
				j -= 2;
			}
			if (j == 0) { // Why we repeat that check again? Check the if just above! Also, we can't put the if selection above on top as we
				d_process = "+"; // would get an error if the position 'j - 1' did not exist.
				AddOnce = 1;
				for (int k = j; k < i; k++) {
					d_temp1 += StringArray[k];
					StringArray[k] = "0";
				}
				break;
			}
			if (MD_case == 1) { // This variable is set to 1 if there's a division after a multiplication or vice-versa.
				d_process = m_process;
				d_temp1 = m_tempResult.ToString();
				MD_case = 0;
				break;
			}
			if (d_case == 1) { // This variable is set to one if there's another division after the current one.
				d_temp1 = d_tempResult.ToString();
				d_case = 0;
				break;
			}
			if (StringArray[j] == "+") { // There's a '+' sign before the number before the '/' sign. To the Addition Array.
				j += 1;
				d_process = "+";
				for (int k = j; k < i; k++) {
					d_temp1 += StringArray[k];
					StringArray[k] = "0";
				}
				break;
			}
			if (StringArray[j] == "-") { // There's a '-' sign before the number before the '/' sign. To the Substraction Array.
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
		if (StringArray[i] == d_process) { // Checks what sign, if any, is found after the '/' sign to determine the end Array.
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
				for (int l = i; l < k; l++) { // Stores the number found after the '/' sign.
					d_temp2 += StringArray[l];
					StringArray[l] = "0";
				}
				break;
			}
		}
		if (d_temp2 == "") { // Stores the number after the '/' sign if it was the last number inputted.
			for (int k = i; k < StringArray.Length; k++) {
				d_temp2 += StringArray[k];
				StringArray[k] = "0";
			}
		}

		// Represents the number in a normal format if they were in the standard form format.

		d_temp1 = ClarifyExponential(d_temp1, false);
		d_temp2 = ClarifyExponential(d_temp2, false);

		d_tempResult = float.Parse(d_temp1) / float.Parse(d_temp2); // Result of the Division.

		if ((d_process == "+") && (d_case == 0) && (MD_case == 0)) { // Adds that Result to the Addition Array if it was a positive one.
			AddArray[AddCount] = d_tempResult.ToString();
			AddCount += 1;
		}

		if ((d_process == "-") && (d_case == 0) && (MD_case == 0)) { // Adds that Result to the Substraction Array if it was a negative one.
			SubstractArray[SubstractCount] = d_tempResult.ToString();
			SubstractCount += 1;
		}		
	}
	

	public void RepairString() { // Checks if there's 2 or more like or unlike signs side by side and simplifies that.
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
		for (int i = 0; i < AddArray.Length; i++) { // Adds the content of the Addition Array to the end result.
			if ((AddArray[i] == null) || (AddArray[i] == "")) {
				break;
			} else {
				AddResult_temp += AddArray[i];
				Result += double.Parse(AddArray[i]);
			}
		}
		for (int i = 0; i < SubstractArray.Length; i++) { // Substracts the content of the Substraction Array from the end result.
			if ((SubstractArray[i] == null) || (SubstractArray[i] == "")) {
				break;
			} else {
				SubstractResult_temp += SubstractArray[i];
				Result -= double.Parse(SubstractArray[i]);
			}
		}
		Result += double.Parse(FirstNum);
	}

	public void Reset() { // Want me to explain what this function does too? :v
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