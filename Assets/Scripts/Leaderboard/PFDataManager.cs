
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PlayFab.ClientModels;
using PlayFab;

public class PFDataManager : MonoBehaviour

{

    public static PFDataManager dataInstance;
    [SerializeField] TMP_Text XP_Display;
  
    [SerializeField] TMP_Text Level_Display;
   
    [SerializeField] TMP_InputField XP_Input;

    int currentXP;

    private void Start()
    {
        if (dataInstance == null)
            dataInstance = this;


      GetUserXPData();
    }


    public void OnSetUserXPData()
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() {
                {"XP",XP_Input.text.ToString() }
        }
        },
        result => Debug.Log("Successfully updated user data"),
        error =>
        {
            Debug.Log("Got error setting user data Ancestor to Arthur");
            Debug.Log(error.GenerateErrorReport());
        });
    }

    public void OnSetUserXPData(int XP)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() {
                {"XP",XP_Input.text.ToString() }
        }
        },
        result => Debug.Log("Successfully updated user data"),
        error =>
        {
            Debug.Log("Got error setting user data Ancestor to Arthur");
            Debug.Log(error.GenerateErrorReport());
        });
    }

    



    public void GetUserXPData(/*string myPlayFabID*/)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            //PlayFabID = myPlayFabID,
            //Key = null
        },
        result =>
        {
            Debug.Log("Got user data:");
            if (result.Data == null || !result.Data.ContainsKey("XP")) 
                Debug.Log("No XP");
            else
            {
               
                //if (int.Parse(result.Data["XP"].Value) > 100)
                //{
                //    Leve
                //}
                Debug.Log("XP: " + result.Data["XP"].Value);
                XP_Display.text = "XP: " + result.Data["XP"].Value;


            }
        }, error =>
        {
            Debug.Log("error retrieving user data:");
            Debug.Log(error.GenerateErrorReport());
        });
    }

    public int GetUserCurrentXPData(/*string myPlayFabID*/)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            //PlayFabID = myPlayFabID,
            //Key = null
        },
        result =>
        {
            Debug.Log("Got user data:");
            if (result.Data == null || !result.Data.ContainsKey("XP"))
                Debug.Log("No XP");
            else
            {

                //if (int.Parse(result.Data["XP"].Value) > 100)
                //{
                //    Leve
                //}
                Debug.Log("XP: " + result.Data["XP"].Value);
                XP_Display.text = "XP: " + result.Data["XP"].Value;
                currentXP = int.Parse(result.Data["XP"].Value);

            }
        }, error =>
        {
            Debug.Log("error retrieving user data:");
            Debug.Log(error.GenerateErrorReport());
        });

        return currentXP;
    }


}
