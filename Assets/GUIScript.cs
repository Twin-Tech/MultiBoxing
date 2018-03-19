using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEditor;
using UnityEngine.UI;


public class GUIScript : MonoBehaviour {

    private Dictionary<string, int> selectedExercise;
    private List<string> selectedJoints;
    string currExercise;
    private int breakTime, sets;
    private string user;
    private GameObject loginUI, inter, skel;
	// Use this for initialization
	void Start () {
        loginUI = GameObject.Find("Login");
        inter = GameObject.Find("Interface");
        skel = GameObject.Find("Skeleton");
        inter.SetActive(false);
        skel.SetActive(false);
        //currExercise = new Exercise();
        selectedExercise = new Dictionary<string, int>();
        selectedJoints = new List<string>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void register()
    {
        StreamReader fread = new StreamReader("user.txt");
        string uname = GameObject.Find("NameField").GetComponent<InputField>().text;
        string pwd = GameObject.Find("PasswordField").GetComponent<InputField>().text;
        Debug.Log(uname + " " + pwd);
        string temp;
        bool flag = false;
        while ((temp = fread.ReadLine()) != null && flag == false)
        {
            temp = temp.Substring(0, temp.IndexOf(":"));
            Debug.Log(temp);
            if(uname==temp)
            {
                flag = true;
                EditorUtility.DisplayDialog("Error", "User Already Exists.", "Okay");
                break;
            }
        }
        fread.Close();
        if (flag == false)
        {
            StreamWriter file = new StreamWriter("user.txt");
            file.WriteLine(uname + ":" + pwd);
            file.Close();
        }
    }
    public void login()
    {
        StreamReader file = new StreamReader("user.txt");
        string uname = GameObject.Find("NameField").GetComponent<InputField>().text;
        string pwd = GameObject.Find("PasswordField").GetComponent<InputField>().text;
        string match = uname + ":" + pwd;
        string temp;
        bool flag = false;
        while((temp=file.ReadLine())!=null && flag==false)
        {
            if (temp == match)
            {
                user = uname;
                file.Close();
                loginSuccess();
                flag = true;
                break;
            }
        }
        if (flag == false)
        {
            EditorUtility.DisplayDialog("Error", "Invalid Username or Password.", "Okay");
            file.Close();
        }
    }
    public void loginSuccess()
    {
        loginUI.SetActive(false);
        inter.SetActive(true);
        skel.SetActive(true);
    }


    public void OKdemo(InputField ip)
    {
        if (ip.text != "")
        {
            int rep = System.Convert.ToInt32(ip.text);
            selectedExercise[currExercise] = rep;

            foreach (KeyValuePair<string, int>  k in selectedExercise)
            {
                Debug.Log(k.Key+":"+k.Value);
            }
            currExercise = null;

            //Debug.Log(currExercise.name + ":" + currExercise.reps);
        }
        else
        {
            EditorUtility.DisplayDialog("Error", "Enter the number of Reps.", "Okay");
        }
    }

    public void ExercisePressed(Button b)
    {
        if (b.image.color == Color.green)
        {
            EditorUtility.DisplayDialog("Error", "You have already selected this exercise.", "Okay");
            currExercise = b.name;
        }
        else
        {
            b.image.color = Color.green;
            currExercise = b.name;
            selectedExercise.Add(currExercise, 0);
        }
    }

    public void addJoint(Button b)
    {
        string jointName = b.name;
        if (selectedJoints.Contains(jointName))
        {
            selectedJoints.Remove(jointName);
            b.image.color = Color.white;
        }
        else
        {
            selectedJoints.Add(jointName);
            b.image.color = Color.green;
        }
        foreach (string s in selectedJoints)
        {
            Debug.Log(s);
        }
    }

    public void finish()
    {
        InputField br = GameObject.Find("BreakField").GetComponent<InputField>();
        InputField set = GameObject.Find("SetsField").GetComponent<InputField>();
        if(selectedJoints.Count==0)
            EditorUtility.DisplayDialog("Error", "You have not selected any joints.", "Okay");
        else if(selectedExercise.Count==0)
            EditorUtility.DisplayDialog("Error", "You have not selected any exercise.", "Okay");
        else if(br.text=="")
            EditorUtility.DisplayDialog("Error", "You have not entered break time.", "Okay");
        else if (set.text == "")
            EditorUtility.DisplayDialog("Error", "You have not entered number of sets.", "Okay");
        else
        {
            breakTime = Convert.ToInt32(br.text);
            sets = Convert.ToInt32(set.text);
            inter.SetActive(false);
            skel.SetActive(false);
            EditorUtility.DisplayDialog("Error", "Details Updated.", "Okay");
        }
    }
}
