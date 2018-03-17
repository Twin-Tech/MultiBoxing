using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Kinect = Windows.Kinect;

public class JoinForce : MonoBehaviour {

    private Vector3 previousPosition;
    private TimeSpan previousFrameTime;
    private int LastFrameCount;
    public float P_FinalVelocity;
    float TimeInterval;
    float time = 1f;
    public Queue<float> FinalVelocities = new Queue<float>(10);
    public float sum = 0;


    // Use this for initialization
    void Start () {
        /*rowDataTemp = new string[8];
        rowDataTemp[0] = "TimeStamp";
        rowDataTemp[1] = "FrameCount";
        rowDataTemp[2] = "FrameTimeDiff";
        rowDataTemp[3] = "Distance";
        rowDataTemp[4] = "CosTheta";
        rowDataTemp[5] = "InitialVelocity";
        rowDataTemp[6] = "Accelaration";
        rowDataTemp[7] = "FinalVelocity";
        rowData.Add(rowDataTemp);*/
        //previousPosition = transform.position;
        previousPosition = CharacterMotion.RightWrist;
        P_FinalVelocity = 0f;
        TimeInterval = 0f;
        LastFrameCount = 1;
        previousFrameTime = new TimeSpan(0,0,0,0,0);
    }
	
	// Update is called once per frame
	void Update () {

        int FrameCount = BodySourceManager.GetFrameCount();
        TimeSpan FrameTime = BodySourceManager.GetKinectDataTime();
        Vector3 Position = CharacterMotion.RightWrist;
       
        if(FrameCount > 1)
        {
            if(FrameCount > LastFrameCount)
            {
                float distance = Vector3.Distance(previousPosition, transform.position);
                if(distance == 0f)
                {
                    P_FinalVelocity = 0f;
                }

                if (time <  0f)
                {
                    float poped_out_value = 0;
                    TimeInterval = GetTimeSpanDifference(FrameTime, previousFrameTime);
                    P_FinalVelocity = distance / TimeInterval;
                    if(FinalVelocities.Count >= 10)
                    {
                        poped_out_value = FinalVelocities.Dequeue();
                    }
                    if (FinalVelocities.Count < 10)
                    {
                        FinalVelocities.Enqueue(P_FinalVelocity);
                        sum += P_FinalVelocity - poped_out_value;
                    }

                    //Debug.Log("Average Velocities:" + sum/FinalVelocities.Count);
                    //float cosT = Vector3.Dot(previousPosition, Position) / (previousPosition.magnitude * Position.magnitude);
                    //float InitialVelocity = P_FinalVelocity * cosT;
                    //float ut = InitialVelocity * TimeInterval;
                    //float Acc = 2 * (distance - ut) / (TimeInterval * TimeInterval);
                    //P_FinalVelocity = (Acc * TimeInterval) + InitialVelocity;
                    /*Debug.Log("Previous Position:" + previousPosition);
                    Debug.Log("Current Position:" + Position);
                    Debug.Log("Frame Count:" + FrameCount);
                    Debug.Log("FrameTime:" +TimeInterval);
                    Debug.Log("DeltaTime:" + Time.deltaTime);
                    Debug.Log("Distance:" + distance);
                    Debug.Log("Cos Theta:" + cosT);
                    Debug.Log("InitialVelocity:" + InitialVelocity);
                    Debug.Log("ut:" + ut);
                    Debug.Log("Accelaration:"+ Acc);
                    Debug.Log("FinalVelocity:" + P_FinalVelocity);

                    rowDataTemp = new string[8];
                    rowDataTemp[0] = FrameTime.ToString();
                    rowDataTemp[1] = FrameCount.ToString();
                    rowDataTemp[2] = TimeInterval.ToString();
                    rowDataTemp[3] = distance.ToString();
                    rowDataTemp[4] = cosT.ToString();
                    rowDataTemp[5] = InitialVelocity.ToString();
                    rowDataTemp[6] = Acc.ToString();
                    rowDataTemp[7] = P_FinalVelocity.ToString();
                    rowData.Add(rowDataTemp);
                    Save();*/
                }
                else
                {
                    time -= Time.deltaTime;
                }

                previousPosition = Position;
                LastFrameCount = FrameCount;
                previousFrameTime = FrameTime;
            }
        }
        else
        {
            previousPosition = Position;
            previousFrameTime = FrameTime;
            P_FinalVelocity = 0;
        }
    }

    private void OnApplicationPause(bool pause)
    {
        previousPosition = CharacterMotion.RightWrist;
        P_FinalVelocity = 0f;
        TimeInterval = 0f;
    }

    private void OnApplicationQuit()
    {
    }
/*
    public void Save()
    {
        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
        {
            sb.AppendLine(string.Join(delimiter, output[index]));
            Debug.Log(output[index]);
        }

        string filePath = Application.dataPath + "/" + "Saved_data.csv"; ;

        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
    }
    */

    private float  GetTimeSpanDifference(TimeSpan S1, TimeSpan S2)
    {
       /* Debug.Log(S1.Ticks);
        Debug.Log(S2.Ticks);
        Debug.Log(S1.Ticks - S2.Ticks);*/
        return Mathf.Abs(((float)(S1.Ticks - S2.Ticks))/100000000000f);
    }

   
}

