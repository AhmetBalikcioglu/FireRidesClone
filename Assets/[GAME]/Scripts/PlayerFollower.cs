//This class is on the main camera to follow player.
//You may optimize it on SetPosition section and
//Write a proper way to update blocks positions on the map to make it an infite gameplay.

using UnityEngine;

public class PlayerFollower : MonoBehaviour {

    private float _zDifference;
    private float _yDifference;

    private void OnEnable()
    {
        SetPosition();
    }

    public void SetPosition()
    {
        //Optimize this portion in an optimized way.
        _zDifference = CharacterManager.Instance.Player.transform.position.z - transform.position.z;
        _yDifference = CharacterManager.Instance.Player.transform.position.y - transform.position.y;
    }

    int lastPassageIndex = -1;
    private void Update()
    {
        transform.position = new Vector3(transform.position.x, CharacterManager.Instance.Player.transform.position.y - _yDifference, CharacterManager.Instance.Player.transform.position.z - _zDifference);
        //BlockCreator.GetSingleton().UpdateBlockPosition(passageIndex); //You may call update block position here to make it an infinite map.
        //Hint:
        //It must be called when it is really needed in a very optimized way.
        
    }

}
