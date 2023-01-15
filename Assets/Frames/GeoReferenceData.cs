using UnityEngine;

namespace Frames
{
	/// <summary>
	/// The collection of frames in current use. Represents the collection that is currently being handles.
	/// Stores the information to place a collection geographically relative to the ESRI stuff. 
	/// </summary>
	public class GeoReference
	{

		public GeoLocation Location { get; set; }
		public GeoReference(GeoLocation location)
		{
			Location = location;
		}


		
		
	}
}