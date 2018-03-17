using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapJoins : MonoBehaviour {
    public GameObject LeftForeArm;
    public GameObject LeftHand;
    public GameObject RightForeArm;
    public GameObject RightHand;
    public GameObject LeftLeg;
    public GameObject LeftFoot;
    public GameObject RightLeg;
    public GameObject RightFoot;

    public readonly Dictionary<int, GameObject[]> dictionary = new Dictionary<int, GameObject[]>();
    GameObject[] Jab_Spheres;
    GameObject[] Join12;
    GameObject[] Join34;
    GameObject[] Join58;

    GameObject Current_Sphere;
    GameObject Current_Join12;
    GameObject Current_Join34;
    GameObject Current_Join58;

    int object_index;
    int Join12_index;
    int Join34_index;
    int Joins58_index;
    string J_name = "";
    string S_Name = "";
    Renderer rend;
    int _score;
    public Text Score;
    public float _Power;
    private bool flag = true;
    
    public Text Power;
    // Use this for initialization
    void Start () {
        _Power = 0f;
        _score = 0;
        setPowerText();
        setScoreText();
        RandomFirst();
        Map();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
     
    void RandomFirst()
    {
        //dictionary.Add(1, new GameObject[] { HandLeft, HandRight, KneeLeft, FootLeft, KneeRight, FootLeft });
        // dictionary.Add(2, new GameObject[] { ElbowLeft, HandLeft, ElbowRight, HandRight, KneeLeft, FootLeft, KneeRight, FootLeft });
        // dictionary.Add(3, new GameObject[] { HandLeft, ElbowLeft, HandRight, ElbowRight });

        Join12 = new GameObject[] { LeftHand, RightHand, LeftLeg, LeftFoot, RightLeg, RightFoot };
        Join34 = new GameObject[] { LeftHand, LeftForeArm, RightForeArm, RightHand, LeftLeg, LeftFoot, RightLeg, RightFoot };
        Join58 = new GameObject[] { LeftHand, LeftForeArm, RightHand, RightForeArm };
    }

    public void Map()
    {
        if (Current_Sphere)
        {
            rend = Current_Sphere.GetComponent<Renderer>();
            rend.material.color = Color.white;
        }
        if(J_name != "")
        {
            if (Current_Join12)
            {
                rend = Current_Join12.GetComponent<Renderer>();
                rend.material.color = Color.white;
            }
            if (Current_Join34)
            {
                rend = Current_Join34.GetComponent<Renderer>();
                rend.material.color = Color.white;
            }
            if(Current_Join58)
            {
                rend = Current_Join58.GetComponent<Renderer>();
                rend.material.color = Color.white;
            }

            if (J_name.Contains("LeftHand") || J_name.Contains("RightHand") || J_name.Contains("LeftFoot") || J_name.Contains("RightFoot"))
            {
                _score = _score + 5;
                

            }
            else
            {
                _score = _score + 10;
            }
            GameData.Current.UpdateScore(_score);
            setScoreText();


        }
        S_Name = "";
        J_name = "";
        Jab_Spheres = GameObject.FindGameObjectsWithTag("Sphere");
        object_index = Random.Range(0, Jab_Spheres.Length);
        //Current_Sphere = Jab_Spheres[object_index];
        if (flag)
        {
            object_index = 7;
            flag = false;
        }
        else
        {
            object_index = 6;
            flag = true;
        }
        Current_Sphere = Jab_Spheres[object_index];
        S_Name = Current_Sphere.name;
        rend = Current_Sphere.GetComponent<Renderer>();
        rend.material.color = Color.red;

        Debug.Log(Current_Sphere);
        /*if (Current_Sphere.name.Contains("Sphere 1") || Current_Sphere.name.Contains("Sphere 2"))
        {
            Debug.Log("12");
            Join12_index = Random.Range(0, Join12.Length);
            Current_Join12 = Join12[Join12_index];
            J_name = Current_Join12.name;
            Debug.Log(J_name);
            Render(Current_Join12);
        }
        else if (Current_Sphere.name.Contains("Sphere 3") || Current_Sphere.name.Contains("Sphere 4"))
        {
            Debug.Log("34");
            Join34_index = Random.Range(0, Join34.Length);
            Current_Join34 = Join34[Join34_index];
            J_name = Current_Join34.name;
            Debug.Log(J_name);
            Render(Current_Join34);
        }
        else
        {
            Debug.Log("5678");
            Joins58_index = Random.Range(0, Join58.Length);
            Current_Join58 = Join58[Joins58_index];
            J_name = Current_Join58.name;
            Debug.Log(J_name);
            Render(Current_Join58);
        }*/
        Joins58_index = 2;
        Current_Join58 = Join58[Joins58_index];
        J_name = Current_Join58.name;
        Debug.Log(J_name);
        Render(Current_Join58);
    }

    public  string JoinName()
    {
        return J_name;
    }

    public string SphereName()
    {
        return S_Name;
    }

    private void Render(GameObject objectname)
    {
        rend = objectname.GetComponent<Renderer>();
        rend.material.color = Color.blue;
    }

    void setScoreText()
    {
        Score.text = "Score : " + _score.ToString();
    }

    public void setPower(GameObject joint)
    {
        JoinForce jf = joint.GetComponent<JoinForce>();
        _Power = jf.P_FinalVelocity;
        Debug.Log("Collided:"+_Power.ToString());
        float [] FV = jf.FinalVelocities.ToArray();
        int length = jf.FinalVelocities.Count;
        Debug.Log("****************************************************************************************************************");
        for(int i=0; i<length; i++)
        {
            Debug.Log("****************"+ FV[i] +"***************");
        }
        Debug.Log("************Sum: " + jf.sum + "****************");
        Debug.Log("************Average: " + jf.sum / length+ "****************");
        Debug.Log("********************************************************************************************************************");
        _Power = jf.sum / length;
        GameData.Current.NextPower(_Power);
        setPowerText();
    }

    void setPowerText()
    {
        Power.text = "Power :"+ _Power.ToString();
    }
}
