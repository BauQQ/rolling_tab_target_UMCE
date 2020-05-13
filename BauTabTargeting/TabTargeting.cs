//this class is build for a mmo/wow style tabtargeting through mobs in range. 
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mirror;
using TMPro;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerIndicator))]
public partial class PlayerTabTargeting
{

    [Header("MMO Targeting")]
    //This is the default range, can be adjusted through the unity editor;
    public int MmoTabTargetRange = 30;    

    //Default tag, can be adjusted through the unity editor;
    public string ObjectTag = "Monster";

    //Has to be infront of the camera for player to target it - NOT IMPLEMENTED
    private bool PlayerLookingAtTarget = false;

    //Set the nextTarget/default target;
    private int nextTarget = 0;

    //This method is what chooses our target and does the calculations on distance/range
    public void MMOTabTarget(){
        //Get all objects tagged as Monsters, can be modified to any tag in the editor
        GameObject[] objects = GameObject.FindGameObjectsWithTag(ObjectTag.Trim());

        //Using any to figure out if there is any entities in the specified tag - Any() can be used
        if(objects.Count() > 0){ 
            //Make a list of mobs in range throught health calculation 
            //and mob distance, does not select anyone thats dead
            List<Monster> monstersInRange = objects.SelectMany(x => x.GetComponents<Monster>()).Where(y => y.health.current > 0)
            .Where(x => Vector3.Distance(transform.position, x.transform.position)<MmoTabTargetRange).ToList(); 
            
            //Using any to figure out if monsterInRange List is empty or not - Any() can be used
            if(monstersInRange.Count()>0){
                //Using ElementAtOrDefault to observer and 
                //check if the next target is available otherwise default to first mob in list
                if(monstersInRange.ElementAtOrDefault(nextTarget)!=null){
                    if(MobIsSeenOrNot(monstersInRange[nextTarget])){
                        //Selecting target on current nextTarget 
                        indicator.SetViaParent(monstersInRange[nextTarget].transform);
                        player.CmdSetTarget(monstersInRange[nextTarget].netIdentity);      
                        nextTarget = nextTarget + 1 % monstersInRange.Count; 
                    }  
                }else{        
                    //Selecting target 0 in list, cause nextTarget doesn't exist
                    indicator.SetViaParent(monstersInRange[0].transform);
                    player.CmdSetTarget(monstersInRange[0].netIdentity);                       
                    nextTarget = 1 % monstersInRange.Count;     
                }          
            }else{
                //Clear tab targeting target
               clearTargetAction();
            }
        }else{
            //Clear tab targeting target
          clearTargetAction();
        }
    }

    //Not implemented yet
    private bool MobIsSeenOrNot(Monster monster){    
        if(PlayerLookingAtTarget){
            return true;
        }else{
            return true;
        }
    }

    //Clearing the target and action 
    public void clearTargetAction(){
        if(player.target!=null){      
            //Clear target
            player.target = null;
            indicator.Clear();
            //Set next target as 0
            nextTarget = 0;
        }
    }
}