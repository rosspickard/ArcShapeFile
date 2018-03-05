using System;
using System.Collections.Generic;
using System.Text;

namespace ArcShapeFile
{
    /// <summary>
    /// A Part is a connected sequence of two or more points in a line or polygon.  Parts may or may not be connected to one another.  Parts may or may not intersect one another.  A part can be used to describe holes in polygons, or polygons and lines that share the same database attributes.
    /// </summary>
    public class Part
    {

        #region **********          Local Variables               **********

        private double mvarMBRXMin = 0;
        private double mvarMBRXMax = 0;
        private double mvarMBRYMin = 0;
        private double mvarMBRYMax = 0;
        private double? mvarZMin = null;
        private double? mvarZMax = null;
        private double? mvarMeasureMin = null;
        private double? mvarMeasureMax = null;
        private int mvarStart = 0;
        private int mvarFinish = 0;
        private bool mvarDonut = false;
        private ePartType mvarPartType = ePartType.none;
        private double mvarPartArea = 0;
        private eDirection? mvarDirection = null;
        private double? mvarPartCentroidX = null;
        private double? mvarPartCentroidY = null;
        private double? mvarPartPerimeter = null;

        #endregion

        #region **********          Public Part Properties        **********

        /// <summary>
        /// The End property of each Part denotes the ordinal position of the last vertice that describes the part shape within the Vertices collection.
        /// </summary>
        /// <remarks>
        /// Use this property, together with the <see cref="Begins"/> property to navigate your way through each part of a multipart shape record.
        /// </remarks>
        /// <example>
        /// <code>
        /// foreach(Part p in shp.Parts)
        /// {
        ///     Console.WriteLine("Part begins at vertice no. {0} and ends at vertice no. {1}", p.Begins, p.Ends);
        ///     for(int v = p.Begins; v &lt;= p.Ends; v++)
        ///     {
        ///         Console.WriteLine("Vertice Position: {0}  X Value: {1}  Y Value {2}", v, shp.Vertices[v].X_Cord, shp.Vertices[v].Y_Cord);
        ///     }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="Ends"/>
        public int Ends
        {
            get { return mvarFinish; }
            set { mvarFinish = value; }
        }


        /// <summary>
        /// The Begins property of each Part denotes the ordinal position of the first vertice that describes the part shape within the Vertices collection.
        /// </summary>
        /// <remarks>
        /// Use this property, together with the <see cref="Ends"/> property to navigate your way through each part of a multipart shape record.
        /// </remarks>
        /// <example>
        /// <code>
        /// foreach(Part p in shp.Parts)
        /// {
        ///     Console.WriteLine("Part begins at vertice no. {0} and ends at vertice no. {1}", p.Begins, p.Ends);
        ///     for(int v = p.Begins; v &lt;= p.Ends; v++)
        ///     {
        ///         Console.WriteLine("Vertice Position: {0}  X Value: {1}  Y Value {2}", v, shp.Vertices[v].X_Cord, shp.Vertices[v].Y_Cord);
        ///     }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="Begins"/>
        public int Begins
        {
            get { return mvarStart; }
            set { mvarStart = value; }
        }


        /// <summary>
        /// Returns the area of the part for polygon shapes.
        /// </summary>
        /// <remarks>
        /// Referencing this property through the Parts collection you will notice that some values are negative and some are positive. If the value is positive then the polygon vertices are ordered clockwise about the normal, otherwise it's counter clockwise with a negative area indicating a hole
        /// </remarks>
        public double Area
        {
            get { return mvarPartArea; }
            set
            {mvarPartArea = value; }
        }


        /// <summary>
        /// This property denotes the direction in which each vertice part has been captured.
        /// </summary>
        /// <remarks>
        /// Polygon rings are normally captured in a clockwise direction ... unless they are holes, in which case they should be captured as anticlockwise. If you are using this library to create your own polygons and are unsure as to the direction of each part you can (and should) force the issue using the <see cref="ArcShapeFile.ShapeFile.SetPartDirection">SetPartDirection</see> method.
        /// </remarks>
        public eDirection? Direction
        {
            get { return mvarDirection; }
            set { mvarDirection = value; }
        }


        /// <summary>
        /// Returns an object that represents the X Centroid of the polygon shape described by this Part
        /// </summary>
        public double? CentroidX
        {
            get { return mvarPartCentroidX; }
            set { mvarPartCentroidX = value; }
        }


        /// <summary>
        /// Returns an object that represents the Y Centroid of the polygon shape described by this Part
        /// </summary>
        public double? CentroidY
        {
            get { return mvarPartCentroidY; }
            set { mvarPartCentroidY = value; }
        }


        /// <summary>
        /// Provides the length of the perimeter around the part polygon shape or the length a line shape.
        /// </summary>
        /// <remarks>
        /// Forget about using this property on MultiPatch shapes - the calculated value will be way out.  This property is really designed to cater for polygons and lines.
        /// If you want the perimeter length of the entire shape use the ShapeFile <see cref="ArcShapeFile.ShapeFile.Perimeter">Perimeter</see> property
        /// </remarks>
        public double? Perimeter
        {
            get { return mvarPartPerimeter; }
            set { mvarPartPerimeter = value; }
        }


        /// <summary>
        /// Returns Double that represents the bounding rectangle minimum X of the polygon shape described by this Part
        /// </summary>
        public double MBRXMin
        {
            get { return mvarMBRXMin; }
            set { mvarMBRXMin = value; }
        }



        /// <summary>
        /// Returns Double that represents the bounding rectangle maximum X of the polygon shape described by this Part
        /// </summary>
        public double MBRXMax
        {
            get { return mvarMBRXMax; }
            set { mvarMBRXMax = value; }
        }



        /// <summary>
        /// Returns Double that represents the bounding rectangle minimum Y of the polygon shape described by this Part
        /// </summary>
        public double MBRYMin
        {
            get { return mvarMBRYMin; }
            set { mvarMBRYMin = value; }
        }



        /// <summary>
        /// Returns a Double that represents the bounding rectangle maximum Y of the polygon shape described by this Part
        /// </summary>
        public double MBRYMax
        {
            get { return mvarMBRYMax; }
            set { mvarMBRYMax = value; }
        }


        /// <summary>
        /// Returns a Double that represents the bounding rectangle minimum Z of the polygonZ shape described by this Part
        /// </summary>
        public double? zMin
        {
            get { return mvarZMin; }
            set { mvarZMin = value; }
        }



        /// <summary>
        /// Returns a Double that represents the bounding rectangle minimum Z of the polygonZ shape described by this Part
        /// </summary>
        public double? zMax
        {
            get { return mvarZMax; }
            set { mvarZMax = value; }
        }


        /// <summary>
        /// Returns a Double that represents the bounding rectangle minimum measure of the polygonZ and polygonM shape described by this Part
        /// </summary>
        public double? MeasureMin
        {
            get { return mvarMeasureMin; }
            set { mvarMeasureMin = value; }
        }



        /// <summary>
        /// Returns aDouble that represents the bounding rectangle maximum measure of the polygonZ and polygonM shapes described by this Part
        /// </summary>
        public double? MeasureMax
        {
            get { return mvarMeasureMax; }
            set { mvarMeasureMax = value; }
        }



        /// <summary>
        /// A polygon consists of one or more rings, or parts.   A polygon may contain multiple outer rings.  The order of vertices or orientation for a ring indicates which side of the ring is the interior of the polygon.  The neighborhood to the right of an observer walking along the ring in vertex order is the neighborhood inside the polygon.  Vertices of rings defining holes in polygons are always in a counterclockwise direction.  Vertices for a single, ringed polygon are, therefore, always in a clockwise order.
        /// </summary>
        /// <remarks>
        /// The IsHole property attempts to indicate that the polygon part is a hole by comparing the minimum bounding rectangles of all parts of the current ShapeFile record.  A True value is returned if the part is contained with the bounds of any other part of the current ShapeFile record.  As a further check a line is drawn from the interior polygon part through the largest polygon part and the number of intersections counted (an odd number indicates that the part is within the larger part).  Why this method?  I have found that ShapeFiles created by some translators use parts to group distinct polygons that share the same attribute data, that is they do not conform to the idea that polygon consists of a series of outer rings.<br/>
        /// <verse xml:space="preserve"><img src="../parteg1.gif"/>         <img src="../parteg2.gif"/></verse><br/>
        /// The figure on the left shows how parts can be used to denote distinct polygons with the same attributes, while the figure on the right shows how parts can be used to create holes in the polygon.  One thing that you should note programatically is that polygon holes <b>must be</b> Anti Clockwise.  You can force this before creating your shape with the <see cref="ArcShapeFile.ShapeFile.SetPartDirection">SetPartDirection</see> method for any parts in your shape.
        /// </remarks>
        /// <seealso cref="ArcShapeFile.ShapeFile.SetPartDirection">SetPartDirection Method</seealso>
        public bool IsHole
        {
            get { return mvarDonut; }
            set { mvarDonut = value; }
        }


        /// <summary>
        /// A MultiPatch shape consists of a number of surfaces, each described by a vertice part.  What type of MultiPatch shape each part is is described by the PatchType.  The parts of a MultiPatch can be of the following types:
        /// </summary>
        /// <remarks>
        /// The following is straight out of the ESRI technical description.  A sequence of parts (or rings) can describe a polygonal surface patch with holes.  The sequence typically consists of all Outer Ring, representing the outer boundary of the patch followed by a number of Inner Rings representing holes. When the individual types of rings in a collection of rings representing a polygonal patch with holes are unknown, the sequence must start with First Ring, followed by a number of Rings. A sequence of Rings not preceded by a First Ring is treated as a sequence of Outer Rings without holes.
        /// The following figure shows examples of all types of MultiPatch parts.<br/>
        /// <img src="../multipatch.gif"/>
        /// </remarks>
        public ePartType PartType
        {
            get { return mvarPartType; }
            set { mvarPartType = value; }
        }


        #endregion
    }
}
