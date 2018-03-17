using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Xml;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class MainScript : MonoBehaviour {

    public string gestureType;
    public List<string> selectedJoints;
    public GameObject abc;
    public float angle;
    public string[] joints = { "Head", "Neck", "SpineShoulder", "ShoulderLeft", "ElbowLeft", "WristLeft", "ThumbLeft", "HandLeft", "HandTipLeft", "ShoulderRight", "ElbowRight", "WristRight", "ThumbRight", "HandRight", "HandTipRight", "SpineMid", "SpineBase", "HipLeft", "KneeLeft", "AnkleLeft", "FootLeft", "HipRight", "KneeRight", "AnkleRight", "FootRight" };
    Dictionary<string, int[]> boneMap = new Dictionary<string, int[]>();
    KeyValuePair<string, int[]> a1, a2;
    XmlDocument xmldoc;
    XmlNode rootnode;
    int count = 0;
    int framecount = 0;
	// Use this for initialization
	void Start () {
        GameObject.Find("ground").SetActive(false);
        GameObject.Find("Canvas1").SetActive(false);
        GameObject.Find("BodySourceManger").SetActive(false);
        GameObject.Find("skeleton").SetActive(false);
        gestureType = "Dynamic";
        xmldoc = new XmlDocument();
        rootnode = xmldoc.CreateElement("root");
        xmldoc.AppendChild(rootnode);
        a1 = new KeyValuePair<string, int[]>(null, new int[] { -1, -1 });
        a2 = new KeyValuePair<string, int[]>(null, new int[] { -1, -1 });
        populateBoneMap();
        Button[] btn = GameObject.FindObjectsOfType<Button>();
        foreach (Button b in btn)
        {
            if (!b.name.Contains("Bone") && b.name != "AddAngle" && b.name!="Submit" && b.name!="Button")
            {
                b.interactable = false ;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		if(recordingstarted && framecount%7==0)
        {
            record();
            count++;
        }
        framecount++;
	}
    public void populateBoneMap()
    {
        boneMap.Add("NeckBone",new[] { 0, 1 });
        boneMap.Add("CervicalSpineBone", new[] { 1, 2 });
        boneMap.Add("ShoulderLeftBone", new[] { 2, 3 });
        boneMap.Add("UpperArmLeftBone", new[] { 3, 4 });
        boneMap.Add("ForeArmLeftBone", new[] { 4, 5 });
        boneMap.Add("ThumbLeftBone", new[] { 5, 6 });
        boneMap.Add("HandLeftBone", new[] { 5, 7 });
        boneMap.Add("HandTipLeftBone", new[] { 7, 8 });
        boneMap.Add("ShoulderRightBone", new[] { 2, 9 });
        boneMap.Add("UpperArmRightBone", new[] { 9, 10 });
        boneMap.Add("ForeArmRightBone", new[] { 10, 11 });
        boneMap.Add("ThumbRightBone", new[] { 11, 12 });
        boneMap.Add("HandRightBone", new[] { 11, 13 });
        boneMap.Add("HandTipRightBone", new[] { 13, 14 });
        boneMap.Add("SpineUpperBone", new[] { 2, 15 });
        boneMap.Add("SpineLowerBone", new[] { 15, 16 });
        boneMap.Add("HipLeftBone", new[] { 16, 17 });
        boneMap.Add("ThighLeftBone", new[] { 17, 18 });
        boneMap.Add("LegLeftBone", new[] { 18, 19 });
        boneMap.Add("FootLeftBone", new[] { 19, 20 });
        boneMap.Add("HipRightBone", new[] { 16, 21 });
        boneMap.Add("ThighRightBone", new[] { 21, 22 });
        boneMap.Add("LegRightBone", new[] { 22, 23 });
        boneMap.Add("FootRightBone", new[] { 23, 24 });
    }
    public void selectJoint(Button joint)
    {
        Text selected = GameObject.Find("SelectedJoints").GetComponent<Text>();
        if (!selected.text.Contains(joint.name))
        {
            selected.text += "\n" + joint.name;
            selectedJoints.Add(joint.name);
            joint.image.color = Color.green;
        }
        else
        {
            selected.text = selected.text.Replace("\n" + joint.name, "");
            joint.image.color = Color.white;
            selectedJoints.Remove(joint.name);
        }
    }
    public void TriggerButton1()
    {

        Dropdown drop = GameObject.Find("PartDropdown").GetComponent<Dropdown>();
        int selectedvalue = drop.value;
        Button[] btn = GameObject.FindObjectsOfType<Button>();
        foreach(Button b in btn)
        {
            if (!b.name.Contains("Bone") && b.name!="AddAngle")
            {
                b.image.color = Color.white;
                b.interactable = true;
            }
        }
        Text selected = GameObject.Find("SelectedJoints").GetComponent<Text>();
        selected.text = "Selected Joints";
        if (selectedvalue==0)
        {

            Button hipl = GameObject.Find("HipLeft").GetComponent<Button>();
            hipl.interactable = false;
            Button hipr = GameObject.Find("HipRight").GetComponent<Button>();
            hipr.interactable = false;
            Button kneel = GameObject.Find("KneeLeft").GetComponent<Button>();
            kneel.interactable = false;
            Button kneer = GameObject.Find("KneeRight").GetComponent<Button>();
            kneer.interactable = false;
            Button ankl = GameObject.Find("AnkleLeft").GetComponent<Button>();
            ankl.interactable = false;
            Button ankr = GameObject.Find("AnkleRight").GetComponent<Button>();
            ankr.interactable = false;
            Button footl = GameObject.Find("FootLeft").GetComponent<Button>();
            footl.interactable = false;
            Button footr = GameObject.Find("FootRight").GetComponent<Button>();
            footr.interactable = false;
        }
        else if(selectedvalue==1)
        {
            foreach(Button b in btn)
            {
                if( b.name.Contains("Thumb") || b.name.Contains("Head") || b.name.Contains("Neck") || b.name.Contains("Shoulder") || b.name.Contains("SpineMid") || b.name.Contains("Hand") || b.name.Contains("Wrist") || b.name.Contains("Elbow"))
                {
                    b.interactable = false;
                }
            }
        }
    }
    public void changeDynamic()
    {
        Toggle dyn = GameObject.Find("Dynamic").GetComponent<Toggle>();
        if (dyn.isOn)
            gestureType = "Dynamic";
        else
            gestureType = "Static";
    }
    public void SubmitClicked()
    {
        Dropdown drop = GameObject.Find("PartDropdown").GetComponent<Dropdown>();
        drop.interactable = false;
        Toggle dyn = GameObject.Find("Dynamic").GetComponent<Toggle>();
        dyn.interactable = false;
        Button[] btn = GameObject.FindObjectsOfType<Button>();
        foreach (Button b in btn)
        {
            if (b.name.Contains("Bone") || b.name.Contains("AddAngle") || b.name.Contains("Finish"))
            {
                b.interactable = true;
            }
            else
                b.interactable = false;
        }
        XmlNode joint = xmldoc.CreateElement("joint");
        XmlAttribute att = xmldoc.CreateAttribute("name");
        att.Value = "GestureType";
        joint.Attributes.Append(att);
        XmlNode type = xmldoc.CreateElement("type");
        type.InnerText = gestureType;
        joint.AppendChild(type);
        rootnode.AppendChild(joint);
    }
    public int anglecount = 0;
    float offset = 75;
    public void addAngle()
    {
        Button fin = GameObject.Find("Finish").GetComponent<Button>();
        RectTransform finrect = fin.GetComponent<RectTransform>();
        finrect.position = new Vector3(finrect.position.x, finrect.position.y - 75, finrect.position.z);
        fin.interactable = false;
        Button b = GameObject.Find("AddAngle").GetComponent<Button>();
        b.interactable = false;
        GameObject content = GameObject.Find("Content");
        GameObject angle = Instantiate(abc);
        angle.name = "angle" + anglecount.ToString();
        angle.transform.parent = content.transform;
        angle.transform.localScale = new Vector3(1, 1, 1);
        angle.transform.position = new Vector3(content.transform.position.x, content.transform.position.y - offset, content.transform.position.z);
        offset += 75;
        anglecount++;
        Button bt = angle.GetComponentInChildren<Button>();
        bt.onClick.AddListener(setAngle);
        Dropdown[] dd = GameObject.FindObjectsOfType<Dropdown>();
        foreach (Dropdown d in dd)
        {
            if (d.name == "Dropdown")
            {
                d.ClearOptions();
                d.AddOptions(selectedJoints);
            }
        }
    }
    KeyValuePair<string,int[]> getBoneInfo(string boneName)
    {
        foreach(KeyValuePair<string,int[]> tmp in boneMap)
        {
            if (tmp.Key == boneName)
                return tmp;
        }
        return new KeyValuePair<string, int[]>(null, new int[] { -1, -1 });
    }
    int flag = 0;
    public void setBones(Button b)
    {
        string current = "angle" + (anglecount - 1).ToString();
        Text[] abc = GameObject.Find(current).GetComponentsInChildren<Text>();
        if(flag==0)
        {
            foreach (Text t in abc)
            {
                if (t.name == "Bone1")
                {
                    a1 = getBoneInfo(b.name);
                    flag = 1;
                    t.text = b.name;
                }
            }
        }
        else if (flag == 1)
        {
            foreach (Text t in abc)
            {
                if (t.name == "Bone2")
                {
                    a2 = getBoneInfo(b.name);
                    flag = 0;
                    t.text = b.name;
                }
            }
        }
    }
    string start, mid, end;
    bool checkBone()
    {
        if(a1.Key!=null && a2.Key!=null)
        {
            if (a1.Value[0] == a2.Value[1])
            {
                mid = joints[a1.Value[0]];
                start = joints[a2.Value[0]];
                end = joints[a1.Value[1]];
                return true;
            }
            else if (a1.Value[1] == a2.Value[0])
            {
                mid = joints[a1.Value[1]];
                start = joints[a2.Value[1]];
                end = joints[a1.Value[0]];
                return true;
            }
            else
                return false;
        }
        return false;
    }
    void setAngletoxml(string forJoint)
    {
        XmlNode joint = xmldoc.CreateElement("joint");
        XmlAttribute att = xmldoc.CreateAttribute("name");
        att.Value = "ValidAngle";
        joint.Attributes.Append(att);
        XmlAttribute att2 = xmldoc.CreateAttribute("forJoint");
        att2.Value = forJoint;
        joint.Attributes.Append(att2);
        XmlAttribute att3 = xmldoc.CreateAttribute("start");
        att3.Value = start;
        joint.Attributes.Append(att3);
        XmlAttribute att4 = xmldoc.CreateAttribute("mid");
        att4.Value = mid;
        joint.Attributes.Append(att4);
        XmlAttribute att5= xmldoc.CreateAttribute("end");
        att5.Value =end;
        joint.Attributes.Append(att5);

        XmlNode type = xmldoc.CreateElement("angle");
        type.InnerText = angle.ToString();
        joint.AppendChild(type);
        rootnode.AppendChild(joint);
    }
    public void setAngle()
    {
        string current = "angle" + (anglecount - 1).ToString();
        GameObject abc = GameObject.Find(current);
        InputField ip = abc.GetComponentInChildren<InputField>();
        if (checkBone() && ip.text!="")
        {
            Dropdown dd = abc.GetComponentInChildren<Dropdown>();
            dd.interactable = false;
            List<Dropdown.OptionData> menuoptions = dd.GetComponent<Dropdown>().options;
            angle = float.Parse(ip.text);
            setAngletoxml(menuoptions[dd.value].text);
            ip.interactable = false;
            Button b = abc.GetComponentInChildren<Button>();
            b.interactable = false;
            Button bt = GameObject.Find("AddAngle").GetComponent<Button>();
            bt.interactable = true;
            Button fin = GameObject.Find("Finish").GetComponent<Button>();
            fin.interactable = true;
        }
        else
        {
            EditorUtility.DisplayDialog("Error", "Select Bones next to each other", "Okay");
        }
    }
    
    public void finish()
    {
        GameObject.Find("GUI").SetActive(false);
    }

    public float leftarmLength = 0.0f;
    public float rightarmLength = 0.0f;
    public void getInitialData()
    {

        XmlNode joint, child;
        XmlAttribute att;
        float realhandlength = 0.0f;
        float realheight = 0.0f;
        float realshoulder = 0.0f;
        float realfootlevel = 0.0f;
        float angle = 0;
        GameObject head = GameObject.Find("Head").gameObject;
        GameObject shoulderLeft = GameObject.Find("ShoulderLeft").gameObject;
        GameObject shoulderRight = GameObject.Find("ShoulderRight").gameObject;
        GameObject foot = GameObject.Find("FootLeft").gameObject;
        JointsProperties jphead = head.GetComponent<JointsProperties>();
        JointsProperties jpshoulderl = shoulderLeft.GetComponent<JointsProperties>();
        JointsProperties jpshoulderr = shoulderRight.GetComponent<JointsProperties>();
        JointsProperties jpfoot = foot.GetComponent<JointsProperties>();
        realheight = jphead.position.y;
        realshoulder = Vector3.Distance(jpshoulderl.position, jpshoulderr.position);
        realhandlength = (rightarmLength + leftarmLength) / 2;
        realfootlevel = jpfoot.position.y;

        joint = xmldoc.CreateElement("joint");
        att = xmldoc.CreateAttribute("name");
        att.Value = "HandLength";
        joint.Attributes.Append(att);
        child = xmldoc.CreateElement("length");
        child.InnerText = realhandlength.ToString();
        joint.AppendChild(child);
        rootnode.AppendChild(joint);

        joint = xmldoc.CreateElement("joint");
        att = xmldoc.CreateAttribute("name");
        att.Value = "ShoulderWidth";
        joint.Attributes.Append(att);
        child = xmldoc.CreateElement("length");
        child.InnerText = realshoulder.ToString();
        joint.AppendChild(child);
        rootnode.AppendChild(joint);

        joint = xmldoc.CreateElement("joint");
        att = xmldoc.CreateAttribute("name");
        att.Value = "HeadPos";
        joint.Attributes.Append(att);
        child = xmldoc.CreateElement("length");
        child.InnerText = realheight.ToString();
        joint.AppendChild(child);
        rootnode.AppendChild(joint);

        joint = xmldoc.CreateElement("joint");
        att = xmldoc.CreateAttribute("name");
        att.Value = "AngleAdjust";
        joint.Attributes.Append(att);
        child = xmldoc.CreateElement("length");
        child.InnerText = realfootlevel.ToString();
        joint.AppendChild(child);
        rootnode.AppendChild(joint);

        foreach (string j in selectedJoints)
        {
            JointsProperties thisjoint = GameObject.Find(j).GetComponent<JointsProperties>();
            joint = xmldoc.CreateElement("joint");
            att = xmldoc.CreateAttribute("name");
            att.Value = j + "Initial";
            joint.Attributes.Append(att);
            child = xmldoc.CreateElement("x");
            child.InnerText = thisjoint.position.x.ToString();
            joint.AppendChild(child);
            child = xmldoc.CreateElement("y");
            child.InnerText = thisjoint.position.y.ToString();
            joint.AppendChild(child);
            child = xmldoc.CreateElement("z");
            child.InnerText = thisjoint.position.z.ToString();
            joint.AppendChild(child);
            if (j.Contains("Hand") || j.Contains("Elbow") || j.Contains("Wrist"))
            {
                Vector3 a, b;
                if (j.Contains("Left"))
                {
                    a = thisjoint.position - jpshoulderl.position;
                    b = new Vector3(jpshoulderl.position.x, 0, jpshoulderl.position.z) - jpshoulderl.position;
                    angle = Math.Abs(Vector3.Angle(a, b));
                }
                else if (j.Contains("Right"))
                {
                    a = thisjoint.position - jpshoulderr.position;
                    b = new Vector3(jpshoulderr.position.x, 0, jpshoulderr.position.z) - jpshoulderr.position;
                    angle = Math.Abs(Vector3.Angle(a, b));
                }
                child = xmldoc.CreateElement("angle");
                child.InnerText = angle.ToString();
                joint.AppendChild(child);
            }
            rootnode.AppendChild(joint);
        }
        Button ini = GameObject.Find("SetInitial").GetComponent<Button>();
        ini.interactable = false;
        Button st = GameObject.Find("StartRecording").GetComponent<Button>();
        st.interactable = true;
    }
    public bool recordingstarted = false;
    public void startRecordClicked()
    {
        recordingstarted = true;
        framecount = 0;
    }
    public void record()
    {
        Button st = GameObject.Find("StartRecording").GetComponent<Button>();
        st.interactable = false;
        Button stop = GameObject.Find("StopRecording").GetComponent<Button>();
        stop.interactable = true;
        XmlNode joint, child;
        XmlAttribute att;
        GameObject shoulderLeft = GameObject.Find("ShoulderLeft").gameObject;
        GameObject shoulderRight = GameObject.Find("ShoulderRight").gameObject;
        JointsProperties jpshoulderl = shoulderLeft.GetComponent<JointsProperties>();
        JointsProperties jpshoulderr = shoulderRight.GetComponent<JointsProperties>();
                foreach (string j in selectedJoints)
                {
                    JointsProperties thisjoint = GameObject.Find(j).GetComponent<JointsProperties>();
                    joint = xmldoc.CreateElement("joint");
                    att = xmldoc.CreateAttribute("name");
                    att.Value = j + count.ToString();
                    joint.Attributes.Append(att);
                    child = xmldoc.CreateElement("x");
                    child.InnerText = thisjoint.position.x.ToString();
                    joint.AppendChild(child);
                    child = xmldoc.CreateElement("y");
                    child.InnerText = thisjoint.position.y.ToString();
                    joint.AppendChild(child);
                    child = xmldoc.CreateElement("z");
                    child.InnerText = thisjoint.position.z.ToString();
                    joint.AppendChild(child);
                    if (j.Contains("Hand") || j.Contains("Elbow") || j.Contains("Wrist"))
                    {
                        Vector3 a, b;
                        if (j.Contains("Left"))
                        {
                            a = thisjoint.position - jpshoulderl.position;
                            b = new Vector3(jpshoulderl.position.x, 0, jpshoulderl.position.z) - jpshoulderl.position;
                            angle = Math.Abs(Vector3.Angle(a, b));
                        }
                        else if (j.Contains("Right"))
                        {
                            a = thisjoint.position - jpshoulderr.position;
                            b = new Vector3(jpshoulderr.position.x, 0, jpshoulderr.position.z) - jpshoulderr.position;
                            angle = Math.Abs(Vector3.Angle(a, b));
                        }
                        child = xmldoc.CreateElement("angle");
                        child.InnerText = angle.ToString();
                        joint.AppendChild(child);
                    }
                    rootnode.AppendChild(joint);
                }
    }
    public void stopRecording()
    {
        recordingstarted = false;
        xmldoc.Save("info.xml");
    }

}
