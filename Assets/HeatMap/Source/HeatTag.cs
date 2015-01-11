/* * * * *
 * HeatTag Class
 * ------------------------------
 * 
 * The HeatTag is class responsible for encapusulating the The HeatTag is class responsible for encapsulating the label, 
 * url and the query parameters needs to post and fetch data to the server. 
 * 
 * Features / attributes:
 * - Configure the way you want your data to be displayed, either MAP or POINT.
 * - Specify a custom GameObject as the Marker to plot.
 * - Control the data being fetched through SetTimestamp(), SetGameTimestamp() and SetSorted()
 * - Specify the size of grids that the heat map renders in.
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

public class HeatTag {

	public enum HeatType { MAP, POINT };

	private float 		DefaultPointSize = 0.2f;
	private float 		DefaultMapSize = 1f;
	
	/// <summary>
	/// The transparency value of a map marker. 
	/// Ignored if type is <c>HeatType.POINT</c>
	/// </summary>
	public float 		MapPointTransparencyValue = 0.6f;

	/// <summary>
	/// The color delta added to the marker when event density is increased.
	/// Have a high delta when the max event density is low.
	/// Have a low delta when the max event density is high.
	/// Ignored if type is <c>HeatType.POINT</c>
	/// </summary>

	public float 		MapPointColorDelta = 0.05f;

	/// <summary>
	/// The marker gameObject used for plotting HeatMaps.
	/// </summary>
	public GameObject 	Marker = null;

	private Color 		pointColor = Color.red;

	/// <summary>
	/// Gets or sets the color of the point.
	/// Ignored if type is <c>HeatType.MAP</c>
	/// </summary>
	/// <value>The color of the point marker.</value>
	public Color 		PointColor
	{
		get {
			return pointColor; 
		}
		set {
			pointColor = value;
			if (Marker) Marker.renderer.material.color = value;
		}
	}

	private float 		size;

	/// <summary>
	/// Gets or sets the size.
	/// In <c>HeatType.POINT</c>, the size is the size of the point marker.
	/// In <c>HeatType.MAP</c>, the size is the size of the map marker and size of the cell in the grid. 
	/// Have a high size when you want to capture a bigger area, and low size for a lower area.
	/// </summary>
	/// <value>The size of the marker</value>
	public float 		Size
	{
		get {
			return size;
		}
		set {
			size = value;
			if (Marker) Marker.transform.localScale = new Vector3 (value, value, value);
		}
	}

	private string 		label;

	/// <summary>
	/// Gets or sets the label.
	/// </summary>
	/// <value>The label.</value>
	public string 		Label
	{
		get {
			return label;
		}
		set {
			label = value.Replace(" ", "_");
			if (Marker) Marker.name = label;
		}
	}

	private string 		url;

	/// <summary>
	/// Gets or sets the URL.
	/// This is the url to the heatmap.php script on the server.
	/// </summary>
	/// <value>The URL.</value>
	public string 		Url
	{
		get {
			return url;
		}
		set {
			url = value + "?label=" + WWW.EscapeURL (label.ToUpper());
		}
	}

	private HeatType 	type;

	/// <summary>
	/// Gets or sets the type.
	/// Setting type resets the marker GameObject.
	/// </summary>
	/// <value>The type that the data points should be plotted</value>
	public HeatType 	Type
	{
		get {
			return type;
		}
		set {
			type = value;
			if (value == HeatType.MAP) {
				SetTypeToMap();
			}
			else {
				SetTypeToPoint();
			}
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="HeatTag"/> class.
	/// </summary>
	/// <param name="label">The label associated with HeatTag.</param>
	/// <param name="url">The url to the heatmap.php script on the server.</param>
	/// <param name="type">The type of data point.</param>
	public HeatTag(string label, string url, HeatType type = HeatType.MAP)
	{
		Label = label;
		Url = url;
		Type = type;
	}

	/// <summary>
	/// Sets the query to fetch data point only after specified time.
	/// </summary>
	/// <param name="dateTime">The latest Date time</param>
	public void SetTimestamp(DateTime dateTime) {
		this.url += "&ticks=" + WWW.EscapeURL (dateTime.Ticks.ToString());
	}

	/// <summary>
	/// Sets the query to fetch data point only after specified game time.
	/// </summary>
	/// <param name="time">The latest game time.</param>
	public void SetGameTimestamp(float time) {
		this.url += "&gameTimeStamp=" + WWW.EscapeURL (time.ToString());
	}

	/// <summary>
	/// Sets the query to fetch data sorted by game time.
	/// </summary>
	public void SetSorted() {
		this.url += "&sorted=" + WWW.EscapeURL ("true");
	}

	/// <summary>
	/// Sets the type to <c>HeatType.MAP</c>.
	/// </summary>
	private void SetTypeToMap() {

		Marker = GameObject.CreatePrimitive(PrimitiveType.Cube);
		Marker.renderer.material.color = Color.blue;
		Marker.renderer.material = new Material (Shader.Find("Transparent/Diffuse"));
		Size = DefaultMapSize;
		Marker.renderer.enabled = false;
		Marker.collider.enabled = false;
	}

	/// <summary>
	/// Sets the type to <c>HeatType.POINT</c>.
	/// </summary>
	private void SetTypeToPoint() {

		Marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		Marker.renderer.material.color = pointColor;
		Marker.renderer.material = new Material (Shader.Find("Transparent/Diffuse"));
		Size = DefaultPointSize;
		Marker.renderer.enabled = false;
		Marker.collider.enabled = false;
	}


}
