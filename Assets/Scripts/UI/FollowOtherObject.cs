using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowOtherObject : MonoBehaviour
{
    public GameObject ObjectToFollow;

    void Update(){
        this.transform.position = ObjectToFollow.transform.position;
    }
}
