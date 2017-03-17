using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;

public class SignInScript : MonoBehaviour {

    //GameSavesProj.Properties.Settings.SavedGamesConnectionString
    //Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\SavedGames.mdf;Integrated Security=True
    //System.Data.SqlClient

    public static string FirstName;
    public static string Surname;

    public InputField FieldEmailInput1;
    public InputField FieldEmailInput2;
    public InputField FieldFirstName;
    public InputField FieldSecondName;
    public Regex regex;

    private LevelManager lmInstance;

	// Use this for initialization
	void Start () {
        regex = new Regex(@"^\S+@\S+\.[\S |\.]+$");
        lmInstance = GameObject.Find("LevelManager").GetComponent<LevelManager>();
	}

	public void Check ()
    {
        if (FieldFirstName.text.Length != 0 && FieldFirstName.text.Length <= 15 && OnlyLetters(FieldFirstName.text))//ensures first name is within range
        {
            if (FieldSecondName.text.Length != 0 && FieldSecondName.text.Length <= 25 && OnlyLetters(FieldSecondName.text))//ensures surname is within range
            {
                string CheckString = FieldEmailInput1.text;
                if (CheckString == FieldEmailInput2.text)//verifies emails and checks if they are identical
                {
                    if (EmailMatch(CheckString))//As both are confirmed to be the same, I only need to check one to see if it is valid
                    {
                        FirstName = FieldFirstName.text;//assign first name
                        Surname = FieldSecondName.text;//assign surname
                        lmInstance.LoadNextLevel();//loads next level
                    }
                    else
                    {
                        FieldEmailInput1.text = "Please Enter a Valid Email Address";//error message for invalid email addresses
                        FieldEmailInput2.text = "Please Enter a Valid Email Address";
                    }
                }
                else
                {
                    FieldEmailInput1.text = "Please Enter the same Email Address";//error message if emails aren't the same
                    FieldEmailInput2.text = "Please Enter the same Email Address";
                }
            }
            else
            {
                if (OnlyLetters(FieldSecondName.text))
                {
                    FieldSecondName.text = "Input a name of valid length";//error message for surname
                }
                else
                {
                    FieldSecondName.text = "Error - enter only letters";
                }
            }
        }
        else
        {
            if (OnlyLetters(FieldFirstName.text))
            {
                FieldFirstName.text = "Input a name of valid length";//error message for first name
            }
            else
            {
                FieldFirstName.text = "Error - enter only letters";
            }
        }
    }

    bool OnlyLetters(string str) //Validates names - makes sure they are only letters or hypens
    {
        foreach (char c in str)
        {
            if (!char.IsLetter(c) && c != '-')
                return false;
        }
        return true;
    }

    bool EmailMatch (string email) //uses regex to check is email passes regex
    {
        Match match = regex.Match(email);
        if (match.Success)
        {
            print(match.Value);
            return true;
        }
        else return false;
    }

}
