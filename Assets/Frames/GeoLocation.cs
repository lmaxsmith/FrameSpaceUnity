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
	}
}