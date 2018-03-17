
using UnityEngine;

public class CollideDetect : MonoBehaviour {
    string Join_Name;
    string Sphere_Name;
    string T_Sphere;
    private void OnTriggerEnter(Collider other)
    {

        GameObject GO = GameObject.Find("Jab");
        MapJoins MJ = GO.GetComponent<MapJoins>();
        Join_Name = MJ.JoinName();
        Sphere_Name = MJ.SphereName();
        if (gameObject.name.Equals(Sphere_Name)){
            //T_Sphere = CH.Sphere();
            if (T_Sphere != "")
            {
                //if (other.name.Contains(Join_Name) && Sphere_Name == T_Sphere)
                if (other.name.Contains(Join_Name))
                {
                    Debug.Log("**********************************************************************************************************************************************************************************8");
                    MJ.Map();
                    MJ.setPower(other.gameObject);
                }
            }
        }
    }
}
