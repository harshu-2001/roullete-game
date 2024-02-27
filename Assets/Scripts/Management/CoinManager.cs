
using UnityEngine;
using System;
using ViewModel;
using System.Collections.Generic;

// using UnityEngine.Purchasing.Extension;


[Serializable]
public class ConsumableItem {
    public string Name;
    public string Id;
    public string desc;
    public float price;
}
public class CoinManager : MonoBehaviour
{
    public ConsumableItem cItem;
    public GameObject pannel;

    public CharacterTable characterTable;
    
   public void OnClick(){
        

        pannel.SetActive(true);
         
    }
    
    public void AddCoins(int value ){
        int aux = characterTable.characterMoney.characterMoney.Value;
        characterTable.characterMoney.characterMoney.Value +=value;
        GlobalConstants.CoinValue= characterTable.characterMoney.characterMoney.Value;
        characterTable.OnSaveGame.OnNext(true);
       
        Dictionary<string,object> screenproperties = new Dictionary<string, object>();
        screenproperties.Add("email",GlobalConstants.Instance.Email);  // Replace with transictionid
        RudderStackHelper.SendScreenEvent("Add Money Screen",screenproperties);
        
        Dictionary<string,object> eventproperties = new Dictionary<string, object>();
        eventproperties.Add("coins",aux);
        eventproperties.Add("coinBuy",value);  
        RudderStackHelper.SendTrackEvent("CoinAdded",eventproperties);

    }
    public void OnClose(){
        pannel.SetActive(false);
    }

}
