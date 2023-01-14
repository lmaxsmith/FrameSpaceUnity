using System;
using Argyle.UnclesToolkit.SceneStuff;
using Newtonsoft.Json;
using UnityEngine;

namespace Frames
{
	/// <summary>
	/// The Unit of data for serializing a frame. Includes pose, timing, and content. 
	/// </summary>
	public class FrameData
	{
		#region ==== Serialized Properties ====------------------


		#region == GeoJson ==----

		public string type { get; set; } = "Feature";
		/// <summary>
		/// Lat lon elevation global position
		/// </summary>
		public GeoLocation geometry { get; set; }
		

		#endregion ----/GeoJson ==

		/// <summary>
		/// Euler angle from North
		/// </summary>
		public Vector3 Rotation { get; set; }
		
		[JsonIgnore]
		public DateTime Created { get; set; }
		[JsonIgnore]
		public string AssetId { get; set; }
		
		
		public float AspectRatio { get; set; }


		#endregion -----------------/Serialized Properties ====

		public FrameData(){}
		public FrameData(GeoLocation geometry, Vector3 rotation)
		{
			this.geometry = geometry;
			this.Rotation = rotation;
			this.AssetId = Guid.NewGuid().ToString();
			AspectRatio = Reference.MainCamera.aspect;
		}
	}

	/// <summary>
	/// For deserializing the upload response. 
	/// </summary>
	public class FrameDataResponse
	{
		public string id { get; set; }
		public DateTime created { get; set; }
		public ImageUrl imageURL { get; set; }
		
		public FrameDataResponse(){}
	}

	/// <summary>
	/// Subtype for fetching images
	/// </summary>
	public class ImageUrl
	{
		public string cloudflareUUID { get; set; }
		public string imageUploadURL { get; set; }
		public string imageDownloadURL { get; set; }

		public ImageUrl() { }
	}
}