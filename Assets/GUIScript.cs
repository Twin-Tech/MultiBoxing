using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEditor;
using UnityEngine.UI;

public class GUIScript : MonoBehaviour {

    private string user;
	// Use this for initialization
	void Start () {

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
        GameObject.Find("Login").active = false;
    }
}
