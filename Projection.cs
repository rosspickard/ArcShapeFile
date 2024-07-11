using System;
using System.Collections.Generic;
using System.Text;

namespace ArcShapeFile
{
    /// <summary>
    /// A class containing the values read from a PRJ file
    /// </summary>
    /// <remarks>
    /// This class is essentially read only and grabs the available details from the projection (.PRJ) file with the same name as the ShapeFile.  To write
    /// a projection file use the <see cref="O:ArcShapeFile.ShapeFile.WriteProjection"/> method.</remarks>
    public class Projection
    {

        #region **********          Local Variables               **********

        string mvarCoordSystem;
        string mvarProjSystem;
        string mvarDatum;
        string mvarSpheroidName;
        double mvarEqRadius;
        double mvarFlatInv;
        double mvarPolarRadius;
        double mvarEccentricity;
        string mvarPrimeMerName;
        double mvarPrimeMeridian;
        double mvarCentralMeridian;
        string mvarGeoSpaceUnitName;
        double mvarGeoSpaceUnitSize;
        string mvarType;

        string mvarProjectionName;
        double mvarFalseEast;
        double mvarFalseNorth;
        double mvarLatOrigin;
        double mvarLongOrigin;
        double mvarScaleFactor;
        string mvarProjectionUnitName;
        double mvarProjectionUnitSize;

        #endregion

        #region **********          Public Projection Properties  **********
        /// <summary>
        /// What type of projection is this?
        /// </summary>
        public string Type
        {
            get { return mvarType; }
            internal set { mvarType = value; }
        }
        /// <summary>
        /// The Geographic Cooordinate System Name
        /// </summary>
        public string GeoCoordSystem
        {
            get { return mvarCoordSystem; }
            internal set { mvarCoordSystem = value; }
        }
        /// <summary>
        /// The Projection Name
        /// </summary>
        public string ProjCoordSystem
        {
            get { return mvarProjSystem; }
            internal set { mvarProjSystem = value; }
        }
        /// <summary>
        /// The Datum used in the projection
        /// </summary>
        public string Datum
        {
            get { return mvarDatum; }
            internal set { mvarDatum = value; }
        }
        /// <summary>
        /// The name of the spheroid used in the projection
        /// </summary>
        public string SpheroidName
        {
            get { return mvarSpheroidName; }
            internal set { mvarSpheroidName = value; }
        }
        /// <summary>
        /// The Equatorial Radius
        /// </summary>
        public double EquitorialRadius
        {
            get { return mvarEqRadius; }
            internal set { mvarEqRadius = value; }
        }
        /// <summary>
        /// The Inverse of the flattening
        /// </summary>
        public double FlatteningInverse
        {
            get { return mvarFlatInv; }
            internal set { mvarFlatInv = value; }
        }
        /// <summary>
        /// The Polar Radius
        /// </summary>
        public double PolarRadius
        {
            get { return mvarPolarRadius; }
            internal set { mvarPolarRadius = value; }
        }
        /// <summary>
        ///  The Eccentricity
        /// </summary>
        public double Eccentricity
        {
            get { return mvarEccentricity; }
            internal set { mvarEccentricity = value; }
        }
        /// <summary>
        /// The name of the Prime Meridian
        /// </summary>
        public string PrimeMeridianName
        {
            get { return mvarPrimeMerName; }
            internal set { mvarPrimeMerName = value; }
        }
        /// <summary>
        /// The Prime Meridian value
        /// </summary>
        public double PrimeMeridian
        {
            get { return mvarPrimeMeridian; }
            internal set { mvarPrimeMeridian = value; }
        }
        /// <summary>
        /// The Central Meridian value
        ///</summary>
        public double CentralMeridian
        {
            get { return mvarCentralMeridian; }
            internal set { mvarCentralMeridian = value; }
        }
        /// <summary>
        /// The name of the coordinate system units
        /// </summary>
        public string GeoSpaceUnitName
        {
            get { return mvarGeoSpaceUnitName; }
            internal set { mvarGeoSpaceUnitName = value; }
        }
        /// <summary>
        /// The size of the coordinate system units
        /// </summary>
        public double GeoSpaceUnitSize
        {
            get { return mvarGeoSpaceUnitSize; }
            internal set { mvarGeoSpaceUnitSize = value; }
        }
        /// <summary>
        /// The name of the Projection
        /// </summary>
        public string ProjectionName
        {
            get { return mvarProjectionName; }
            internal set { mvarProjectionName = value; }
        }
        /// <summary>
        /// The projection false East value
        /// </summary>
        public double FalseEast
        {
            get { return mvarFalseEast; }
            internal set { mvarFalseEast = value; }
        }
        /// <summary>
        /// The projection false North value
        /// </summary>
        public double FalseNorth
        {
            get { return mvarFalseNorth; }
            internal set { mvarFalseNorth = value; }
        }
        /// <summary>
        /// Latitude of Origin value
        /// </summary>
        public double LatitudeOrigin
        {
            get { return mvarLatOrigin; }
            internal set { mvarLatOrigin = value; }
        }
        /// <summary>
        /// Longitude of Origin value
        /// </summary>
        public double LongitudeOrigin
        {
            get { return mvarLongOrigin; }
            internal set { mvarLongOrigin = value; }
        }
        /// <summary>
        /// Scale factor
        /// </summary>
        public double ScaleFactor
        {
            get { return mvarScaleFactor; }
            internal set { mvarScaleFactor = value; }
        }
        /// <summary>
        /// The name of the units used by the projection
        /// </summary>
        public string ProjectionUnitName
        {
            get { return mvarProjectionUnitName; }
            internal set { mvarProjectionUnitName = value; }
        }
        /// <summary>
        /// The size of the units used by the projection
        /// </summary>
        public double ProjectionUnitSize
        {
            get { return mvarProjectionUnitSize; }
            internal set { mvarProjectionUnitSize = value; }
        }


        #endregion

        public string WKT { get; set; } = string.Empty;

        public int EPSG { get; set; }
    }
}
