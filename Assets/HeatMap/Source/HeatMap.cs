/* * * * *
 * The HeatMap Tool
 * ------------------------------
 * 
 * The Heatmap Tool allows developers to rapidly add a telemetry infrastructure to track custom events, 
 * and visualize the collected data point in a heatmap that overlays the game level map. 
 * For example, a developer can track the point of player’s deaths to evaluate the difficulty of the level, 
 * or he can track how a player progresses through the map to evaluate the spatial characteristics of the level map. 
 * The power of the tool comes from the multi-client server architecture where each instance of the game posts data 
 * to a remote server, which in turn stores it in the database layer. The developer can fetch the data and visualize 
 * it on her local instance. The tool uses SimpleJSON (http://wiki.unity3d.com/index.php/SimpleJSON) to help facilitate 
 * the communication between clients and server, and either MongoDB or MySQL to store the data.
 * 
 * Written by Kaustubh Srivastava 
 * 2014-10-10
 * 
 * See Samples for examples on how to use the HeatMap Tool.
 * 
 * * * * */

using UnityEngine;
using System.Collections;
using System;
using SimpleJSON;
using System.Collections.Generic;

public class HeatMap : MonoBehaviour {
	
	/// <summary>
	/// Setting isEnabled to false stops all tracking threads.
	/// </summary>
	public bool 				isEnabled  =  true;

	/// <summary>
	/// Plots the data for the specified tag.
	/// The tag encapsulates the data about the url, the marker and the plotting type.
	/// </summary>
	/// <param name="tag">The associated <see cref="HeatTag"/></param>
	public void PlotData(HeatTag tag) {
		StartCoroutine (GetDataHelper (tag));
	}

	/// <summary>
	/// Post the specified vector for the tag.
	/// The tag encapsulates the data about the url.
	/// </summary>
	/// <param name="vec">The vector that is posted</param>
	/// <param name="tag">The associated <see cref="HeatTag"/></param>
	public void Post(Vector3 vec, HeatTag tag) {
		StartCoroutine(PostHelper(vec, tag));
	}


	/// <summary>
	/// Tracks the player. Posts the player's transform's positions every specified rate seconds.
	/// </summary>
	/// <param name="player">The tracked player.</param>
	/// <param name="tag">The associated <see cref="HeatTag"/></param>
	/// <param name="rate">The rate that data is posted in seconds.</param>
	public void TrackPlayer(GameObject player, HeatTag tag, float rate) {
		StartCoroutine (TrackPlayerHelper (player, tag, rate));
	}


	private Hashtable 			markerTable;
	
	private struct HeatData {
		public Vector3 vec;
		public string label;
		public long ticks;
		public float gameTimeStamp;
		
		public HeatData(JSONNode data) {
			this.vec = new Vector3 (
				float.Parse(data["x"]),
				float.Parse(data["y"]),
				float.Parse(data["z"])
				);
			this.label = data["label"];
			this.ticks = long.Parse(data["ticks"]);
			this.gameTimeStamp = float.Parse(data["gameTimeStamp"]);
		}
	}


	/* PRIVATE METHODS */

	private void Start () {
		markerTable = new Hashtable();
	}

	/// <summary>
	/// Helper for tracking player.
	/// </summary>
	/// <returns>The player helper.</returns>
	/// <param name="player">Player.</param>
	/// <param name="tag">Tag.</param>
	/// <param name="rate">Rate.</param>
	private IEnumerator TrackPlayerHelper (GameObject player, HeatTag tag, float rate)
	{
		while(isEnabled) {
			if (player) Post(player.transform.position, tag);
			yield return new WaitForSeconds(rate);
		}
	}

	/// <summary>
	/// Helper to get data.
	/// </summary>
	/// <returns>The data helper.</returns>
	/// <param name="tag">Tag.</param>
	private IEnumerator GetDataHelper (HeatTag tag)
	{
		WWW download = new WWW ( tag.Url );
		yield return download;
		if((!string.IsNullOrEmpty(download.error))) {
			print( "Error downloading: " + download.error );
		} else {
			var data = JSONNode.Parse(download.text);
			StartCoroutine(PlotHeat(data, tag));
		}
	}

	/// <summary>
	/// Plots the heat data.
	/// </summary>
	/// <returns>The heat.</returns>
	/// <param name="data">Data.</param>
	/// <param name="tag">Tag.</param>
	private IEnumerator PlotHeat (JSONNode data, HeatTag tag)
	{
		for (int i = 0; i < data.Count; i++) {
			HeatData heatData = new HeatData(data[i]);
			if (tag.Type == HeatTag.HeatType.MAP) {
				PlotHeatMap(tag, heatData);
			}
			else if (tag.Type == HeatTag.HeatType.POINT) {
				PlotHeatPoint(tag, heatData);
			}
			else {
				throw new Exception ("HeatType not found");
			}
			yield return null;
		}
	}

	/// <summary>
	/// Plots the heat point.
	/// </summary>
	/// <param name="tag">Tag.</param>
	/// <param name="data">Data.</param>
	private void PlotHeatPoint(HeatTag tag, HeatData data) {
		GameObject point = Instantiate(tag.Marker, data.vec, Quaternion.identity) as GameObject;
		point.renderer.enabled = true;
	}

	/// <summary>
	/// Snaps the coordinate to a cell in the grid.
	/// </summary>
	/// <returns>The coordinate.</returns>
	/// <param name="vec">Vec.</param>
	/// <param name="dim">Dim.</param>
	private Vector3 SnapCoord(Vector3 vec, float dim) {
		return new Vector3(
			((int) (vec.x/dim)) * dim,
			((int) (vec.y/dim)) * dim,
			((int) (vec.z/dim)) * dim
		);
	}

	/// <summary>
	/// Plots the heat map.
	/// </summary>
	/// <param name="tag">Tag.</param>
	/// <param name="data">Data.</param>
	private void PlotHeatMap(HeatTag tag, HeatData data) {

		Vector3 		pos = SnapCoord(data.vec, tag.Size);
		int 			key = pos.GetHashCode();
		GameObject 		point;
		Color 			markerColor = Color.blue;
		
		if  (markerTable.Contains(key)) {
			point = markerTable[key] as GameObject;
			markerColor = new Color (
				point.renderer.material.color.r + tag.MapPointColorDelta,
				point.renderer.material.color.g,
				point.renderer.material.color.b - tag.MapPointColorDelta
			);
		}
		else {
			point = Instantiate(tag.Marker, pos, Quaternion.identity) as GameObject;
			point.renderer.enabled = true;
			markerTable.Add(key, point);
		}

		markerColor.a = tag.MapPointTransparencyValue;
		point.renderer.material.color = markerColor;
	}

	/// <summary>
	/// Helper function to post data.
	/// </summary>
	/// <returns>The helper.</returns>
	/// <param name="vec">Vec.</param>
	/// <param name="tag">Tag.</param>
	private IEnumerator  PostHelper (Vector3 vec, HeatTag tag){
		WWWForm form = new WWWForm();
		form.AddField( "x", vec.x.ToString());
		form.AddField( "y", vec.y.ToString());
		form.AddField( "z", vec.z.ToString());
		form.AddField( "label", tag.Label.ToUpper());
		form.AddField( "ticks", System.DateTime.Now.Ticks.ToString());
		form.AddField( "gameTimeStamp", Time.time.ToString());
		WWW download = new WWW( tag.Url, form );
		yield return download;
		if((!string.IsNullOrEmpty(download.error))) {
			print("Error downloading: " + download.error);
		}
	}

}
