using System;
using RudderStack;
using UnityEngine;
using System.Collections.Generic;


public static class RudderStackHelper
{
    private static RudderClient rudderClient;
    public static void Intialise(){ GetRudderStackSdk();}

    private static RudderClient GetRudderStackSdk()
    {
        if (rudderClient == null)
        {
            RudderClient.SerializeSqlite();

            // And then initialize the sdk in the Awake method of your main GameObject Script
            RudderConfigBuilder configBuilder = new RudderConfigBuilder()
            .WithDataPlaneUrl("https://ztechasaxlromu.dataplane.rudderstack.com")
            .WithLogLevel(RudderLogLevel.DEBUG);

            rudderClient = RudderClient.GetInstance("2aDPNcfJV8KL0342K5whu8RNgh3", configBuilder.Build());
        }
        return rudderClient;
    }

    public static void SendIdentityEvent()
    {
        //pre-defined API's for inserting standard traits
        
        RudderMessage identifyMessage = new RudderMessageBuilder().Build();
        RudderTraits traits = new RudderTraits();

        //pre-defined API's for inserting standard traits
        traits.PutEmail(GlobalConstants.Instance.Email);
        traits.PutName(GlobalConstants.Instance.DisplayName);
        // // traits.Put("avatar",GlobalConstants.Instance.ImageUrl);
        traits.PutCreatedAt(DateTime.Now.ToString());

        GetRudderStackSdk().Identify(GlobalConstants.Instance.Email, traits, identifyMessage);
    }

    public static void SendTrackEvent(string trackEventName, Dictionary<string,object> eventProperties=null)
    {

        // create message to track
        if(eventProperties==null){   
            eventProperties =new Dictionary<string, object>();
        }
        eventProperties.Add("email", GlobalConstants.Instance.Email);
        eventProperties.Add("updatedCoins", GlobalConstants.CoinValue);
        RudderMessageBuilder builder = new RudderMessageBuilder();
        builder.WithEventName(trackEventName);// clickEvent, 

        if(eventProperties!=null){builder.WithEventProperties(eventProperties);}

        GetRudderStackSdk().Track(builder.Build());
    }

    public static void SendScreenEvent(string screenEventName,Dictionary<string,object>screenProperties)
    {
       
        RudderMessageBuilder screenBuilder = new RudderMessageBuilder();
        screenBuilder.WithEventName(screenEventName);// Home Screen
        screenBuilder.WithEventProperties(screenProperties);
        GetRudderStackSdk().Screen(screenBuilder.Build());
    }

    public static void SaveDataFn() 
    {      
        Debug.Log($"Savedata");
        string json = JsonUtility.ToJson(GlobalConstants.Dts);
        GlobalConstants.DbRef.Child("users").Child(GlobalConstants.Instance.UserId).SetRawJsonValueAsync(json);
    }
}
