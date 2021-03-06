// radar.cs
using UnityEngine;
using System.Collections;

public class Radar : MonoBehaviour
{
	const float MAX_DISTANCE = 10f;
	const int RADAR_SIZE = 256;
	
	public Transform playerController;
	public Texture radarBackground;
	public Texture targetBlip;

	
	private void OnGUI()
	{
		// background displaying top left in square of 128 pixels

		Rect radarBackgroundRect = new Rect(0,0, RADAR_SIZE, RADAR_SIZE);
		GUI.DrawTexture(radarBackgroundRect,radarBackground,ScaleMode.StretchToFill,true);
		
		// find all 'cube' tagged objects
		GameObject[] cubeGOArray = GameObject.FindGameObjectsWithTag("cube");
		
		// draw blips for all within distance
		Vector3 playerPos = playerController.transform.position;
		foreach (GameObject cubeGO in cubeGOArray)  
		{
			Vector3 targetPos = cubeGO.transform.position;
			float distanceToTarget = Vector3.Distance(targetPos,playerPos);
			if( (distanceToTarget <= MAX_DISTANCE) )
				DrawBlip(playerPos, targetPos, distanceToTarget);
		}


	}
	
	private void DrawBlip(Vector3 playerPos, Vector3 targetPos, float distanceToTarget)
	{
		// distance from target to player
		float dx =  targetPos.x - playerPos.x;
		float dz =  targetPos.z - playerPos.z;
		
		// find angle from player to target
		float angleToTarget = Mathf.Atan2(dx,dz) * Mathf.Rad2Deg;
		
		// direction player facing
		float anglePlayer = playerController.eulerAngles.y;
		
		// subtract player angle, to get relative angle to object
		// subtract 90
		// (so 0 degrees (same direction as player) is UP)
		float angleRadarDegrees =  angleToTarget - anglePlayer - 90;
		
		// calculate (x,y) position given angle and distance
		float normalisedDistance = distanceToTarget / MAX_DISTANCE;	
		float angleRadians = angleRadarDegrees * Mathf.Deg2Rad;
		float blipX = normalisedDistance * Mathf.Cos(angleRadians);
		float blipY = normalisedDistance * Mathf.Sin(angleRadians);	
		
		// scale blip position according to radar size
		blipX *= RADAR_SIZE/2;
		blipY *= RADAR_SIZE/2;
		
		// offset blip position relative to radar center (64,64)
		blipX += RADAR_SIZE/2;
		blipY += RADAR_SIZE/2;
		
		// draw target texture at calculated location
		Rect blipRect = new Rect(blipX - 5, blipY - 5, 10, 10);
		GUI.DrawTexture(blipRect, targetBlip);		
	}
}