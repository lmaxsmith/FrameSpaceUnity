using System;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Frames
{
	public class GeoLocation
	{

		public string type { get; set; } = "Point";
		public double[] coordinates { get; set; }
		

		public GeoLocation(double lattitude, double longitude, double elevation)
		{
			coordinates = new double[3] { lattitude, longitude, elevation };
		}

		private const double EarthRadius = 6378000;
		private const double mPerDegLat = 111000;
		static double mPerDegLon(double lat) => (Math.PI / 180) * EarthRadius;

		/// <summary>
		/// Calculates a new lat/lon/elevation based on an initial value and a vector3 offset in meters.
		/// The offset assumes north forward.
		/// </summary>
		/// <param name="start"></param>
		/// <param name="relativePosition"></param>
		/// <returns></returns>
		public GeoLocation Translate(Vector3 relativePosition) => Translate(this, relativePosition);


		/// <summary>
		/// Calculates a new lat/lon/elevation based on an initial value and a vector3 offset in meters.
		/// The offset assumes north forward.
		/// </summary>
		/// <param name="start"></param>
		/// <param name="relativePosition"></param>
		/// <returns></returns>
		public static GeoLocation Translate(GeoLocation start, Vector3 relativePosition)
		{
			return new GeoLocation(
				start.coordinates[0] + relativePosition.z / mPerDegLat,
				start.coordinates[1] + relativePosition.x / mPerDegLon(start.coordinates[0]),
				start.coordinates[2] + relativePosition.y);
		}


		/// <summary>
		/// Finds the relative position in vector3space between two geographic locations
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		public Vector3 TranslationTo(GeoLocation target)
		{
			return new Vector3(
				(float) (target.coordinates[1] - coordinates[1]) /(float) mPerDegLon(target.coordinates[0]),
				(float) (target.coordinates[2] - coordinates[2]),
				(float) (target.coordinates[0] - coordinates[0]) / (float) mPerDegLat
			);
		}
	}
}