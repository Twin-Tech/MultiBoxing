using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
    NetworkAnimator NetAnim;
    Animator Anim;
    int IdleHash;
    int RunHash;
    int JumpHash;
	// Use this for initialization
	void Start () {
        //NetworkManager.singleton.StartClient();
        Anim = GetComponent<Animator>();
        NetAnim = GetComponent<NetworkAnimator>();
        IdleHash = Animator.StringToHash("Idle");
        RunHash = Animator.StringToHash("Run");
        JumpHash = Animator.StringToHash("Jump");
        for(int i = 0; i<3; i++)
        {
            Debug.Log(NetAnim.GetParameterAutoSend(i));
        }
	}
	
	// Update is called once per frame
	void Update () {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        //transform.position = new Vector3( transform.position.x, transform.position.y, transform.position.z + 0.01f);
        if(v > 0)
        {
            Anim.SetTrigger(RunHash);
            NetAnim.SetTrigger(RunHash);
        }
        else if (Input.GetKeyDown("space"))
        {
            Anim.SetTrigger(JumpHash);
            NetAnim.SetTrigger(JumpHash);
        }
        else
        {
            Anim.SetTrigger(IdleHash);
            NetAnim.SetTrigger(IdleHash);
        }

    }
}
