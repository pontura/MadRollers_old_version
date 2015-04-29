using UnityEngine;
using System.Collections;

public class GrabbableJetPAck : MonoBehaviour {
	
	private GameObject character;
	
	// Use this for initialization
	void Start () {
		//character = GameObject.FindGameObjectWithTag("Player");	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
    //void OnTriggerEnter (Collider other) {
    //    if(other.gameObject.CompareTag("Player"))
    //    {
    //        //character.SendMessage("EnableJetPack", SendMessageOptions.DontRequireReceiver);
    //    }
    //}
}
