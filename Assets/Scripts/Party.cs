using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Party : MonoBehaviour
{
    public int partySlots;
    public List<Character> partyMembers;
    
    // The relative position x of the first character of the party
    private float localFrontPositionX;
    public float FrontPositionX {
        get {
            return transform.position.x + localFrontPositionX;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Calculate the front position where the first character will stop
        PixelPerfectCamera pixelCamera = Camera.main.GetComponent<PixelPerfectCamera>();
        float screenWidth = pixelCamera.refResolutionX / pixelCamera.assetsPPU;
        localFrontPositionX =  partySlots - 0.5f - (screenWidth / 2);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int SlotsAvailable()
    {
        int slots = partySlots;
        foreach(Character character in partyMembers) {
            slots -= character.size;
        }

        return slots;
    }

    public float GetPositionFromSlot(int slot)
    {

        return 0;
    }


    public void AddCharacter(Character character)
    {
 
    }
}
