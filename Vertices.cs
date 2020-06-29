using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace ArcShapeFile
{
    /// <summary>
    /// <para>A collection that holds all the X,Y,Z and Measure data of each shape record.</para>
    /// </summary>
    /// <remarks>
    /// <para>The Vertices collection is simply a representates all the coordinates of the current ShapeFile record.
    /// It always points to the <see cref="ArcShapeFile.ShapeFile.CurrentRecord">current record</see> of the ShapeFiles object.</para>
    /// <para>You can refer to each <B>Field</B> object within the collection by:
    ///<ul>
	///<li>Iteration by using the 0 based ordinal - i.e. for(int i=0;i &lt; shp.Vertices.Count; i++)</li>
	///<li>Iteration by reference - i.e. foreach(Vertice vt in shp.Vertices)</li>
    ///</ul>
    /// </para></remarks>
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [Guid("1BBEA337-0B68-418c-B380-BB54B38866B1")]
    public class Vertices : System.Collections.CollectionBase
    {

        #region **********          Local Variables               **********

        private double mvarMBRXMin;
        private double mvarMBRXMax;
        private double mvarMBRYMin;
        private double mvarMBRYMax;
        private double? mvarMBRZMin;
        private double? mvarMBRZMax;
        private double? mvarMeasureMin;
        private double? mvarMeasureMax;
        private int mvarNoOfPoints = 0;
        private eReadMode mvarReadmode;
        private double? nullValue =null;
        internal byte[] vertData;

        #endregion

        #region **********          Vertice Event Handlers        **********

        /// <summary>
        /// A handler to trap vertices being added
        /// </summary>
        /// <param name="e">The parsed vertice event arguments</param>
        public delegate void AddVertHandler(AddVertArgs e);
        /// <summary>
        /// The event fired each time a vertice is added to the collection 
        /// </summary>
        /// <remarks>
        /// This event allows you to track when a Vertice has been added.  The arguments passed by the event are:
        ///<list type="table">
        ///    <listheader>
        ///        <term>Argument</term>
        ///        <description>Description</description>
        ///    </listheader>
        ///    <item>
        ///        <term>VerticeNo</term>
        ///        <description>the ordinal position of the inserted Vertice</description>
        ///    </item>
        ///    <item>
        ///        <term>=Insert;</term>
        ///        <description>>A boolean value indicating whether the Vertice has been inserted rather than appended to the bottom</description>
        ///    </item>
        ///</list>
        /// </remarks>
        public event AddVertHandler VerticeAdded;
        internal virtual void onAdd(AddVertArgs e) { if (VerticeAdded != null) VerticeAdded(e); }

        /// <summary>
        /// A handler to trap vertices being deleted
        /// </summary>
        /// <param name="e">The parsed vertice event arguments</param>
        public delegate void DelVertHandler(DelVertArgs e);
        /// <summary>
        /// The event fired each time a vertice is deleted from the collection 
        /// </summary>
        /// <remarks>
        /// This event allows you to track when a Vertice has been removed.  The arguments passed by the event are:
        ///<list type="table">
        ///    <listheader>
        ///        <term>Argument</term>
        ///        <description>Description</description>
        ///    </listheader>
        ///    <item>
        ///        <term>VerticeNo</term>
        ///        <description>the ordinal position of the removed Vertice (prior to removal of course)</description>
        ///    </item>
        ///</list>
        /// </remarks>
        public event DelVertHandler VerticeDeleted;
        internal virtual void onDelete(DelVertArgs e) { if (VerticeDeleted != null) VerticeDeleted(e); }

        /// <summary>
        /// A handler to trap parts being added
        /// </summary>
        /// <param name="e">The parsed part event arguments</param>
        internal delegate void AddPartHandler(AddPartArgs e);
        /// <summary>
        /// The event fired each time a part is added to the collection 
        /// </summary>
        internal event AddPartHandler PartAdded;
        internal virtual void onPartAdd(AddPartArgs e) { if (PartAdded != null) PartAdded(e); }

        /// <summary>
        /// Event to signify that all vertices have been cleared
        /// </summary>
        internal event EventHandler VerticesCleared;
        internal virtual void onVerticesCleared(EventArgs e) { if (VerticesCleared != null) VerticesCleared(this, e); }

        #endregion

        #region **********          Public Vertice Properties     **********
        /// <summary>
        /// Grabs the coordinate vertice from the collection
        /// </summary>
        /// <param name="Index">The index number of the vertice</param>
        public Vertice this[int Index]
        {
            get
            {
                Vertice functionReturnValue = default(Vertice);
                int mvarCurrentVertice = 0;
                if (mvarReadmode != eReadMode.FastRead)
                {
                    functionReturnValue = (Vertice)List[Index];
                }
                else
                {
                    // Read the Data directly from the held array
                    mvarCurrentVertice = Index;
                    functionReturnValue = PopulateVertice(Index);

                }
                return functionReturnValue;
            }
            set
            {
                if (mvarReadmode != eReadMode.FastRead)
                    List[Index] = value;
            }
        }

        /// <summary>
        /// Returns the number of vertices in the current record
        /// </summary>
        /// <remarks>
        /// You can use this property to mark the maximum value of any loop iterating through the collection.  Items in this collection can be indexed from 0 to 1 - the returned value.
        /// Remember that Vertices can have multiple parts to them so iterating from the Part.<see cref="ArcShapeFile.Part.Begins">Begins</see> to Part.<see cref="ArcShapeFile.Part.Ends">Ends</see> might be a better idea.
        /// </remarks>
        public new int Count
        {
            get { return mvarNoOfPoints; }
        }
       
        /// <summary>
        /// Gets the number of points the the current record
        /// </summary>
        /// <value></value>
        internal int NoOfPoints
        {
            get { return mvarNoOfPoints; }
            set { mvarNoOfPoints = value; }
        }

        internal eReadMode ReadMode
        {
            set { mvarReadmode = value; }
        }

        // The MBRs of the shape

        /// <summary>
        /// The minimimum X value for the current ShapeFile record
        /// </summary>
        /// <remarks>
        /// Part of the ShapeFile record minimum bounding values for the current ShapeFile record, this value and is popoulated for all shape types.
        /// </remarks>
        public double xMin
        {
            get { return mvarMBRXMin; }
            set { mvarMBRXMin = value; }
        }

        /// <summary>
        /// The maximimum X value for the current ShapeFile record
        /// </summary>
        /// <remarks>
        /// Part of the ShapeFile record minimum bounding values for the current ShapeFile record, this value and is popoulated for all shape types.
        /// </remarks>
        public double xMax
        {
            get { return mvarMBRXMax; }
            set { mvarMBRXMax = value; }
        }

        /// <summary>
        /// The minimimum Y value for the current ShapeFile record
        /// </summary>
        /// <remarks>
        /// Part of the ShapeFile record minimum bounding values for the current ShapeFile record, this value and is popoulated for all shape types.
        /// </remarks>
        public double yMin
        {
            get { return mvarMBRYMin; }
            set { mvarMBRYMin = value; }
        }

        /// <summary>
        /// The maximimum Y value for the current ShapeFile record
        /// </summary>
        /// <remarks>
        /// Part of the ShapeFile record minimum bounding values for the current ShapeFile record, this value and is popoulated for all shape types.
        /// </remarks>
        public double yMax
        {
            get { return mvarMBRYMax; }
            set { mvarMBRYMax = value; }
        }

        /// <summary>
        /// The maximimum X value for the current ShapeFile record
        /// </summary>
        /// <remarks>
        /// Part of the ShapeFile record minimum bounding values for the current ShapeFile record, this value and is popoulated for all shape types.
        /// </remarks>
        public double? mMin
        {
            get { return mvarMeasureMin; }
            set
            {
                if (!value.Equals(nullValue))
                {
                    if (!mvarMeasureMin.Equals(nullValue))
                    {
                        if (Convert.ToDouble(mvarMeasureMin) < -1E+38)
                        {
                            mvarMeasureMin = nullValue;
                        }
                        else
                        {
                            mvarMeasureMin = value;
                        }
                    }
                    else
                    {
                        mvarMeasureMin = value;
                    }
                }
                else
                {
                    mvarMeasureMin = nullValue;
                }

            }
        }

        /// <summary>
        /// The minimimum non-null measure value for the current ShapeFile record
        /// </summary>
        /// <remarks>
        /// Part of the ShapeFile record minimum bounding values for the current ShapeFile record, this value may be null and populated only for shapes in X,Y,Z space or Measured X,Y space.
        /// </remarks>
        public double? mMax
        {
            get { return mvarMeasureMax; }
            set
            {
                if (!value.Equals(nullValue))
                {
                    if (!mvarMeasureMax.Equals(nullValue))
                    {
                        if (Convert.ToDouble(mvarMeasureMax) < -1E+38)
                        {
                            mvarMeasureMax = nullValue;
                        }
                        else
                        {
                            mvarMeasureMax = value;
                        }
                    }
                    else
                    {
                        mvarMeasureMax = value;
                    }
                }
                else
                {
                    mvarMeasureMax = nullValue;
                }

            }
        }

        /// <summary>
        /// The minimimum non-null Z value for the current ShapeFile record
        /// </summary>
        /// <remarks>
        /// Part of the ShapeFile record minimum bounding values for the current ShapeFile record, this value is populated only for shapes in X,Y,Z space and will be null otherwise
        /// </remarks>
        public double? zMin
        {
            get { return mvarMBRZMin; }
            set { mvarMBRZMin = value; }
        }

        /// <summary>
        /// The maximimum Z value for the current ShapeFile record
        /// </summary>
        /// <remarks>
        /// Part of the ShapeFile record minimum bounding values for the current ShapeFile record, this value is populated only for shapes in X,Y,Z space and will be null otherwise
        /// </remarks>
        public double? zMax
        {
            get { return mvarMBRZMax; }
            set { mvarMBRZMax = value; }
        }

        #endregion

        #region **********          Public Vertice Methods        **********

        /// <summary>
        /// Deletes the vertice from the Vertices Collection
        /// </summary>
        /// <param name="vntIndexKey">The ordinal position of the vertice to be removed.</param>
        /// <remarks>
        /// The Index number is an integer (int32) indicating the ordinal position of the vertice coordinates in the collection.
        /// Now this is where I should give you a warning.  Deleting vertices is up to you ... but ... (there is always a but) the rules are:
        /// <ul>
        /// <li>Move to the next record before saving the changes and the vertice point won't be removed</li>
        /// <li>When you save the altered shape with one less vertice point in it the ShapeFile will need to be re-written.  I've tried to make this procedure as fast as possible but ...</li>
        /// <li>Removing vertices will automatically adjust the <see cref="ArcShapeFile.Part.Begins">Begins</see> and <see cref="ArcShapeFile.Part.Ends">Ends</see> properties of the <see cref="ArcShapeFile.Parts">Parts</see> collection</li>
        /// </ul>
        /// Get the picture?
        /// The deleted <see cref="ArcShapeFile.Vertice">Vertice</see> becomes permanently removed from the shape when you use the <see cref="ArcShapeFile.ShapeFile.ModifyShape">ModifyShape</see> method.
        /// </remarks>
        /// <seealso cref="ArcShapeFile.ShapeFile.ModifyShape"/>
        public new void RemoveAt(int vntIndexKey)
        {
            if (vntIndexKey >= 0 & vntIndexKey < List.Count)
            {
                Vertice mVert = (Vertice)List[vntIndexKey];
                //System.Diagnostics.Debug.WriteLine("X: " + mVert.X_Cord.ToString() + "   Y:" + mVert.Y_Cord.ToString());
                List.RemoveAt(vntIndexKey);
                mvarNoOfPoints--;
                Globals.mvarVerticeChange = true;
                onDelete(new DelVertArgs(vntIndexKey));
            }
        }

        /// <summary>
        /// Clears all data from the Vertices Collection
        /// </summary>
        /// <remarks>
        /// When would you use this?  When you've realised that you don't want the Vertice record after all.  You don't have to add this method after a Move, <see cref="ArcShapeFile.ShapeFile.WriteShape">WriteShape</see> or <see cref="ArcShapeFile.ShapeFile.ModifyShape">ModifyShape</see> as it's done for you, ready for the next lot of Vertices.
        /// </remarks>
        /// <example>
        /// <code lang="C#">
        ///using (ShapeFile myShape = new ShapeFile())
        ///{
        ///    myShape.Open(@"C:\Shapes\Polygon.shp", true);
        ///
        ///    // Add a new record
        ///    myShape.Vertices.Add(10, 10);
        ///    myShape.Vertices.Add(10, 20);
        ///    myShape.Vertices.Add(20, 20);
        ///    myShape.Vertices.Add(20, 10);
        ///    myShape.Vertices.Add(10, 10);
        ///    myShape.Fields[0].Value = "New Record";
        ///    myShape.Fields[1].Value = 3.45;
        ///    myShape.Fields[2].Value = DateTime.Now;
        ///    myShape.Fields[3].Value = 3.45E10;
        ///
        ///    // Ooops ... let's clear the vertices and try again
        ///    myShape.Vertices.Clear();
        ///    
        ///    // try again
        ///    myShape.Vertices.Add(15, 10);
        ///    myShape.Vertices.Add(15, 20);
        ///    myShape.Vertices.Add(20, 20);
        ///    myShape.Vertices.Add(20, 10);
        ///    myShape.Vertices.Add(15, 10);
        ///    myShape.WriteShape();
        ///}
        /// </code>
        /// <code lang="VB">
        ///Using myShape As New ShapeFile()
        ///    myShape.Open(@"C:\Shapes\Polygon.shp", True)
        ///
        ///    ' Add a new record
        ///    myShape.Vertices.Add(10, 10)
        ///    myShape.Vertices.Add(10, 20)
        ///    myShape.Vertices.Add(20, 20)
        ///    myShape.Vertices.Add(20, 10)
        ///    myShape.Vertices.Add(10, 10)
        ///    myShape.Fields(0).Value = "New Record"
        ///    myShape.Fields(1).Value = 3.45
        ///    myShape.Fields(2).Value = DateTime.Now
        ///    myShape.Fields(3).Value = 34500000000.0
        ///
        ///    ' Ooops ... let's clear the vertices and try again
        ///    myShape.Vertices.Clear();
        ///    
        ///    // try again
        ///    myShape.Vertices.Add(15, 10)
        ///    myShape.Vertices.Add(15, 20)
        ///    myShape.Vertices.Add(20, 20)
        ///    myShape.Vertices.Add(20, 10)
        ///    myShape.Vertices.Add(15, 10)
        ///    myShape.WriteShape()
        ///End Using
        /// </code>
        /// </example>
        /// <seealso cref="ArcShapeFile.ShapeFile.ModifyShape"/>
        /// <seealso cref="ArcShapeFile.ShapeFile.WriteShape"/>
        public new void Clear()
        {
                // Use when removing all elements from the collection
                List.Clear();
                mvarMBRXMin = 0;
                mvarMBRXMax = 0;
                mvarMBRYMin = 0;
                mvarMBRYMax = 0;
                mvarMBRZMin = null;
                mvarMBRZMax = null;
                mvarMeasureMin = null;
                mvarMeasureMax = null;
                mvarNoOfPoints = 0;
                onVerticesCleared(EventArgs.Empty);
        }

        /// <summary>
        /// Any vertices added after this command will be considered to be a member of a new part or ring.
        /// </summary>
        /// <remarks>
        /// Parts or rings are used to describe polygon holes or shapes that share the same attribution (amoungst other things). This method increments the internal part number counter and appends a new <see cref="ArcShapeFile.Part">Part</see> to the collection.  Any vertices added thereafter are deemed to belong to this new Part.
        /// </remarks>
        public void NewPart()
        { //mvarNoOfParts++; 
            onPartAdd(new AddPartArgs(ePartType.none));
        }
        /// <summary>
        /// Any vertices added after this command will be considered to be a member of a new part or ring.
        /// For MultiPatch shapes a ring PartType should be provided
        /// </summary>
        /// <param name="PartType">The MultiPatch Part Type</param>
        /// <remarks>
        /// Parts or rings are used to describe polygon holes or shapes that share the same attribution (amoungst other things). This method increments the internal part number counter and appends a new <see cref="ArcShapeFile.Part">Part</see> to the collection.  Any vertices added thereafter are deemed to belong to this new Part.
        /// For MultiPatch shapes you have the option of parsing the type of part.  I've never seen this shape type being used in anger so there may still be work to be done here.
        /// </remarks>
        public void NewPart(ePartType PartType)
        { //mvarNoOfParts++; 
            onPartAdd(new AddPartArgs(PartType));
        }

        #region Add Method Overloads

        /// <summary>
        /// Adds a new X and Y vertice into the ShapeFile Vertices collection
        /// </summary>
        /// <param name="X_Cord">The X Coordinate of the vertice</param>
        /// <param name="Y_Cord">The Y Coordinate of the vertice</param>
        public void Add(double X_Cord,double Y_Cord)
        { AddVertice(X_Cord, Y_Cord, nullValue, nullValue, -1); }
        /// <summary>
        /// Inserts a new x and Y vertice into the ShapeFile Vertices collection at a given position
        /// </summary>
        /// <param name="X_Cord">The X Coordinate of the vertice</param>
        /// <param name="Y_Cord">The Y Coordinate of the vertice</param>
        /// <param name="InsertAfter">Where should this vertice be inserted - defaults to end of list</param>
        public void Add(double X_Cord, double Y_Cord, int InsertAfter)
        { AddVertice(X_Cord, Y_Cord, nullValue, nullValue, InsertAfter); }
        /// <summary>
        /// Adds a new X, Y and Measure vertice into the ShapeFile Vertices collection
        /// </summary>
        /// <param name="X_Cord">The X Coordinate of the vertice</param>
        /// <param name="Y_Cord">The Y Coordinate of the vertice</param>
        /// <param name="Measure">The Measure of a 3D ShapeFile's  vertice (may be NULL)</param>
        public void Add(double X_Cord, double Y_Cord, object Measure)
        { AddVertice(X_Cord, Y_Cord, Convert.ToDouble(Measure), nullValue, -1); }
        /// <summary>
        /// Inserts a new X, Y and Measure vertice into the ShapeFile Vertices collection at a given position
        /// </summary>
        /// <param name="X_Cord">The X Coordinate of the vertice</param>
        /// <param name="Y_Cord">The Y Coordinate of the vertice</param>
        /// <param name="Measure">The Measure of a 3D ShapeFile's  vertice (may be NULL)</param>
        /// <param name="InsertAfter">Where should this vertice be inserted - defaults to end of list</param>
        public void Add(double X_Cord, double Y_Cord, object Measure, int InsertAfter)
        { AddVertice(X_Cord, Y_Cord, Convert.ToDouble(Measure), nullValue, InsertAfter); }
        /// <summary>
        /// Adds a new X, Y and Z vertice into the ShapeFile Vertices collection
        /// </summary>
        /// <param name="X_Cord">The X Coordinate of the vertice</param>
        /// <param name="Y_Cord">The Y Coordinate of the vertice</param>
        /// <param name="Z_Cord">The Z Coordinate of a 3D ShapeFile's vertice</param>
        public void Add(double X_Cord, double Y_Cord, double Z_Cord)
        { AddVertice(X_Cord, Y_Cord, nullValue, Z_Cord, -1); }
        /// <summary>
        /// Inserts a new X, Y and Z vertice into the ShapeFile Vertices collection at a given position
        /// </summary>
        /// <param name="X_Cord">The X Coordinate of the vertice</param>
        /// <param name="Y_Cord">The Y Coordinate of the vertice</param>
        /// <param name="Z_Cord">The Z Coordinate of a 3D ShapeFile's vertice</param>
        /// <param name="InsertAfter">Where should this vertice be inserted - defaults to end of list</param>
        public void Add(double X_Cord, double Y_Cord, double Z_Cord, int InsertAfter)
        { AddVertice(X_Cord, Y_Cord, nullValue, Z_Cord, InsertAfter); }
        /// <summary>
        /// Adds a new X, Y, Z and Measure vertice into the ShapeFile Vertices collection
        /// </summary>
        /// <param name="X_Cord">The X Coordinate of the vertice</param>
        /// <param name="Y_Cord">The Y Coordinate of the vertice</param>
        /// <param name="Measure">The Measure of a 3D ShapeFile's  vertice (may be NULL)</param>
        /// <param name="Z_Cord">The Z Coordinate of a 3D ShapeFile's vertice</param>
        public void Add(double X_Cord, double Y_Cord, object Measure, double Z_Cord)
        { AddVertice(X_Cord, Y_Cord, Convert.ToDouble(Measure), Z_Cord, -1); }
        /// <summary>
        /// Inserts a new X, Y, Z and Measure vertice into the ShapeFile Vertices collection at a given position
        /// </summary>
        /// <param name="X_Cord">The X Coordinate of the vertice</param>
        /// <param name="Y_Cord">The Y Coordinate of the vertice</param>
        /// <param name="Measure">The Measure of a 3D ShapeFile's  vertice (may be NULL)</param>
        /// <param name="Z_Cord">The Z Coordinate of a 3D ShapeFile's vertice</param>
        /// <param name="InsertAfter">Where should this vertice be inserted - defaults to end of list</param>
        public void Add(double X_Cord, double Y_Cord, object Measure, double Z_Cord, int InsertAfter)
        { AddVertice(X_Cord, Y_Cord, Convert.ToDouble(Measure), Z_Cord, InsertAfter); }
        /// <summary>
        /// Adds a new vertice into the ShapeFile Vertices collection
        /// </summary>
        /// <param name="vert">The ShapeFile Vertice structure to be added</param>
        public void Add(Vertice vert)
        { AddVertice(vert.X_Cord, vert.Y_Cord, vert.Measure, vert.Z_Cord, -1); }
        /// <summary>
        /// Inserts a new vertice into the ShapeFile Vertices collection at a given position
        /// </summary>
        /// <param name="vert">The ShapeFile Vertice structure to be added</param>
        /// <param name="InsertAfter">Where should this vertice be inserted - defaults to end of list</param>
        public void Add(Vertice vert, int InsertAfter)
        { AddVertice(vert.X_Cord, vert.Y_Cord, vert.Measure, vert.Z_Cord, InsertAfter); }
        /// <summary>
        /// Adds the specified Well Known Geometry Text into the ShapeFile Vertices collection. Refer to the wikipedia page <see aref="https://en.wikipedia.org/wiki/Well-known_text"/> for details
        /// </summary>
        /// <param name="WKT">The Well Known Text.</param>
        public void Add(string WKT)
        {
            WKT = WKT.Trim();
            if (!WKT.Contains("EMPTY") || WKT.Contains("()"))
            {
                bool hasZ = false, hasM = false;
                if (WKT.Contains(" M") || WKT.Contains(" ZM"))
                    hasM = true;
                if (WKT.Contains(" Z") || WKT.Contains(" ZM"))
                    hasZ = true;

                if (WKT.StartsWith("POINT"))
                {
                    WKT = WKT.Substring(WKT.IndexOf("("));
                    WKT = WKT.Replace("(", "").Trim();
                    WKT = WKT.Replace(")", "").Trim();
                    string[] bits = WKT.Split(' ');
                    double x = Convert.ToDouble(bits[0].Trim(), System.Globalization.CultureInfo.InvariantCulture), y = Convert.ToDouble(bits[1].Trim(), System.Globalization.CultureInfo.InvariantCulture);
                    double? z = nullValue, m = nullValue;
                    if (hasZ && hasM)
                    {
                        z = Convert.ToDouble(bits[2].Trim(), System.Globalization.CultureInfo.InvariantCulture);
                        m = Convert.ToDouble(bits[3].Trim(), System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        if (hasZ)
                            z = Convert.ToDouble(bits[3].Trim(), System.Globalization.CultureInfo.InvariantCulture);
                        if (hasM)
                            m = Convert.ToDouble(bits[3].Trim(), System.Globalization.CultureInfo.InvariantCulture);
                    }
                    AddVertice(x, y, m, z, -1);
                }
                else if (WKT.StartsWith("MULTIPOINT") || WKT.StartsWith("LINESTRING"))
                {
                    WKT = WKT.Substring(WKT.IndexOf("("));
                    // Remove all brackets
                    WKT = WKT.Replace("(", "").Trim();
                    WKT = WKT.Replace(")", "").Trim();
                    string[] coords = WKT.Split(',');
                    foreach (string coord in coords)
                    {
                        string[] bits = coord.Trim().Split(' ');
                        double x = Convert.ToDouble(bits[0].Trim(), System.Globalization.CultureInfo.InvariantCulture), y = Convert.ToDouble(bits[1].Trim(), System.Globalization.CultureInfo.InvariantCulture);
                        double? z = nullValue, m = nullValue;
                        if (hasZ && hasM)
                        {
                            z = Convert.ToDouble(bits[2].Trim(), System.Globalization.CultureInfo.InvariantCulture);
                            m = Convert.ToDouble(bits[3].Trim(), System.Globalization.CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            if (hasZ)
                                z = Convert.ToDouble(bits[3].Trim(), System.Globalization.CultureInfo.InvariantCulture);
                            if (hasM)
                                m = Convert.ToDouble(bits[3].Trim(), System.Globalization.CultureInfo.InvariantCulture);
                        }
                        AddVertice(x, y, m, z, -1);
                    }
                }
                else if (WKT.StartsWith("POLYGON") || WKT.StartsWith("MULTILINE") || WKT.StartsWith("MULTIPOLYGON"))
                {
                    WKT = WKT.Substring(WKT.IndexOf("("));
                    WKT = WKT.Replace("), (", "),(").Trim();

                    if (WKT.Contains("),("))
                    {
                        //string[] parts = WKT.Split("),(".ToCharArray());
                        string[] separatingStrings = { "), (", "),(" };
                        string[] parts = WKT.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < parts.Length; i++)
                        {
                            if (i > 0)
                                NewPart();
                            // Remove all brackets
                            parts[i] = parts[i].Replace("(", "").Trim();
                            parts[i] = parts[i].Replace(")", "").Trim();
                            string[] coords = parts[i].Split(',');
                            foreach (string coord in coords)
                            {
                                string[] bits = coord.Trim().Split(' ');
                                double x = Convert.ToDouble(bits[0].Trim(), System.Globalization.CultureInfo.InvariantCulture), y = Convert.ToDouble(bits[1].Trim(), System.Globalization.CultureInfo.InvariantCulture);
                                double? z = nullValue, m = nullValue;
                                if (hasZ && hasM)
                                {
                                    z = Convert.ToDouble(bits[2].Trim(), System.Globalization.CultureInfo.InvariantCulture);
                                    m = Convert.ToDouble(bits[3].Trim(), System.Globalization.CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    if (hasZ)
                                        z = Convert.ToDouble(bits[3].Trim(), System.Globalization.CultureInfo.InvariantCulture);
                                    if (hasM)
                                        m = Convert.ToDouble(bits[3].Trim(), System.Globalization.CultureInfo.InvariantCulture);
                                }
                                AddVertice(x, y, m, z, -1);
                            }
                        }
                    }

                }
            }
        }
        /// <summary>
        /// Adds the specified Well Known Binary byte array into the ShapeFile Vertices collection. Refer to the wikipedia page  <see aref="https://en.wikipedia.org/wiki/Well-known_text"/> for details
        /// </summary>
        /// <param name="WKB">The Well Known Binary array.</param>
        public void Add(byte[] WKB)
        {
            int pos = 0;
            bool isBigEndian = (WKB[0] == 1);
            int GeomType = 0;
            byte[] iBytes = new byte[4];
            Buffer.BlockCopy(WKB, 1, iBytes, 0, 4);
            if (!isBigEndian)
                Array.Reverse(iBytes);
            GeomType = BitConverter.ToInt32(iBytes, 0);
            int gmType = GeomType % 1000;

            // only deal with types the ShapeFile can handle
            if (gmType > 0 && gmType < 7)
            {
                pos = 5;
                UInt32 NoPolys = 1;
                if (gmType == 6 || gmType == 5)
                {
                    Buffer.BlockCopy(WKB, pos, iBytes, 0, 4);
                    if (!isBigEndian)
                        Array.Reverse(iBytes);
                    NoPolys = BitConverter.ToUInt32(iBytes, 0);
                    pos += 4;
                }

                for (int plyNo = 0; plyNo < NoPolys; plyNo++)
                {
                    if (gmType > 4)
                    {
                        isBigEndian = (WKB[pos] == 1);
                        pos++;
                    }

                    Buffer.BlockCopy(WKB, pos, iBytes, 0, 4);
                    pos += 4;
                    if (!isBigEndian)
                        Array.Reverse(iBytes);
                    int pGeomType = BitConverter.ToInt32(iBytes, 0);
                    int pgmType = pGeomType % 1000;

                    // only deal with types the ShapeFile can handle
                    if (pgmType > 0 && pgmType < 7)
                    {
                        if (plyNo > 0)
                            NewPart();

                        bool hasZ = false, hasM = false;
                        if (pGeomType - pgmType == 3000)
                        {
                            hasZ = true;
                            hasM = true;
                        }
                        else if (pGeomType - pgmType == 2000)
                            hasM = true;
                        else if (pGeomType - pgmType == 1000)
                            hasZ = true;

                        UInt32 NoRings = 1;
                        if (pgmType == 3)
                        {
                            Buffer.BlockCopy(WKB, pos, iBytes, 0, 4);
                            if (!isBigEndian)
                                Array.Reverse(iBytes);
                            NoRings = BitConverter.ToUInt32(iBytes, 0);
                            pos += 4;
                        }
                        for (int ring = 0; ring < NoRings; ring++)
                        {
                            if (ring > 0)
                                NewPart();

                            UInt32 NoPoints = 1;
                            if (pgmType > 1)
                            {
                                Buffer.BlockCopy(WKB, pos, iBytes, 0, 4);
                                if (!isBigEndian)
                                    Array.Reverse(iBytes);
                                NoPoints = BitConverter.ToUInt32(iBytes, 0);
                                pos += 4;
                            }

                            for (int ptNo = 0; ptNo < NoPoints; ptNo++)
                            {
                                byte[] dBytes = new byte[8];
                                Buffer.BlockCopy(WKB, pos, dBytes, 0, 8);
                                if (!isBigEndian)
                                    Array.Reverse(dBytes);
                                double x = BitConverter.ToDouble(dBytes, 0);
                                pos += 8;
                                Buffer.BlockCopy(WKB, pos, dBytes, 0, 8);
                                if (!isBigEndian)
                                    Array.Reverse(dBytes);
                                double y = BitConverter.ToDouble(dBytes, 0);
                                double? z = null, m = null;
                                pos += 8;

                                if (hasM && hasZ)
                                {
                                    Buffer.BlockCopy(WKB, pos, dBytes, 0, 8);
                                    if (!isBigEndian)
                                        Array.Reverse(dBytes);
                                    m = BitConverter.ToDouble(dBytes, 0);
                                    pos += 8;
                                    Buffer.BlockCopy(WKB, pos, dBytes, 0, 8);
                                    if (!isBigEndian)
                                        Array.Reverse(dBytes);
                                    z = BitConverter.ToDouble(dBytes, 0);
                                }
                                else if (hasZ || hasM)
                                {
                                    Buffer.BlockCopy(WKB, pos, dBytes, 0, 8);
                                    if (!isBigEndian)
                                        Array.Reverse(dBytes);
                                    if (hasZ)
                                        z = BitConverter.ToDouble(dBytes, 0);
                                    else
                                        m = BitConverter.ToDouble(dBytes, 0);
                                }

                                AddVertice(x, y, m, z, -1);

                            }

                        }
                    }
                }

            }
        }

        private void AddVertice(double X_Cord, double Y_Cord, double? Measure, double? Z_Cord, int InsertAfter)
        {
            //create a new object
            Vertice objNewMember = default(Vertice);
            objNewMember = new Vertice();

            // Set defaults
            if (InsertAfter < 0)
            { InsertAfter = -1; }
            else if (InsertAfter >= mvarNoOfPoints)
            { InsertAfter = mvarNoOfPoints - 1; }
            else
            { InsertAfter++; }


            // Set Vertice Values
            objNewMember.X_Cord = X_Cord;
            objNewMember.Y_Cord = Y_Cord;
            objNewMember.Z_Cord = Z_Cord;
            objNewMember.Measure = Measure;
            //objNewMember.PartType = PartType;

            // Update MBR Values
            if (mvarNoOfPoints == 0)
            {
                mvarMBRXMin = X_Cord;
                mvarMBRXMax = X_Cord;
                mvarMBRYMin = Y_Cord;
                mvarMBRYMax = Y_Cord;
                mvarMBRZMin = Z_Cord;
                mvarMBRZMax = Z_Cord;
                mvarMeasureMin = Measure;
                mvarMeasureMax = Measure;
            }
            else
            {
                mvarMBRXMin = Math.Min(X_Cord, mvarMBRXMin);
                mvarMBRXMax = Math.Max(X_Cord, mvarMBRXMax);
                mvarMBRYMin = Math.Min(Y_Cord, mvarMBRYMin);
                mvarMBRYMax = Math.Max(Y_Cord, mvarMBRYMax);
                if (Z_Cord != null)
                {
                    mvarMBRZMin = Math.Min(Convert.ToDouble(Z_Cord), Convert.ToDouble(mvarMBRZMin));
                    mvarMBRZMax = Math.Max(Convert.ToDouble(Z_Cord), Convert.ToDouble(mvarMBRZMax));
                }
                if (Measure != null & Measure > -1E+38)
                {
                    mvarMeasureMin = Math.Min(Convert.ToDouble(Measure), Convert.ToDouble(mvarMeasureMin));
                    mvarMeasureMax = Math.Max(Convert.ToDouble(Measure), Convert.ToDouble(mvarMeasureMax));
                }
            }
            mvarNoOfPoints++;


            if (InsertAfter == -1)
            { 
                List.Add(objNewMember);
                onAdd(new AddVertArgs( Convert.ToInt32(List.Count - 1),false));
            }
            else
            {
                List.Insert(InsertAfter,objNewMember); 
                onAdd (new AddVertArgs( InsertAfter, true));
            }
        }

        #endregion

        #endregion

        #region **********          Internal Methods              **********

        internal void Populate(FileStream fsShapeFile, FileStream fsIndexFile, int recordnumber)
        {
            // Populate the raw byte data into the Vertice VertData array
            byte[] LongBytes = new byte[4];
            long FilePos = 0;

            if (recordnumber > 1)
            {
                //Read the location out out the index file
                int Offset = 100 + ((recordnumber - 1) * 8);
                fsIndexFile.Seek(Offset, SeekOrigin.Begin);
                fsIndexFile.Read(LongBytes, 0, 4);
                Array.Reverse(LongBytes);
                FilePos = (BitConverter.ToInt32(LongBytes, 0) * 2) + 4;
            }
            else
            {
                FilePos = 104;
            }

            if (FilePos > fsShapeFile.Length)
            { return; }

            // Move to start of the Shape Record
            FilePos = fsShapeFile.Seek(FilePos, SeekOrigin.Begin);
            // Ignore the record number
            if (FilePos == 0)
            {
                throw new Exception("The ShapeFile is corrupted at record number " + recordnumber.ToString());
            }

            fsShapeFile.Read(LongBytes, 0, 4);
            Array.Reverse(LongBytes);
            int ContentLength = BitConverter.ToInt32(LongBytes, 0);

            // Check to avoid silly errors
            if (ContentLength == 0)
            {
                throw new Exception("The ShapeFile is corrupted at record number " + recordnumber.ToString());
            }

            vertData = new byte[ContentLength * 2];
            fsShapeFile.Read(vertData, 0, ContentLength * 2);


        }

        internal Vertice PopulateVertice(int Index)
        {
            // *************************************************************************************
            // * Read the coordinates from the stored byte array                                   *
            // * This Sub is used in the FastRead ReadMode and populates a single Vertice instance *
            // *************************************************************************************


            {
                int ArrayPos = 0;
                double lvarXCord = 0;
                double lvarYCord = 0;
                double? lvarZCord = null;
                double? lvarMeasure = null;
                double lvarM = 0;
                int lvarNoOfParts = 0;
                eShapeType lvarShapeType = (eShapeType)BitConverter.ToInt32(vertData, 0);


                //lvarPartNo = 1;

                // X & Y Values
                switch ((eShapeType)lvarShapeType)
                {

                    // *******************************
                    // * Point Shapes                *
                    // *******************************
                    case eShapeType.shpPoint:
                    case eShapeType.shpPointM:
                    case eShapeType.shpPointZ:
                        ArrayPos = 4;
                        lvarXCord = BitConverter.ToDouble(vertData, ArrayPos);
                        lvarYCord = BitConverter.ToDouble(vertData, ArrayPos + 8);
                        if (lvarShapeType == eShapeType.shpPointZ)
                        {
                            lvarZCord = BitConverter.ToDouble(vertData, 20);
                            if (vertData.Length > 20)
			    {
				    lvarMeasure = BitConverter.ToDouble(vertData, 28);
				    if (lvarMeasure <= -1E+38)
				    {
					lvarMeasure = null;
				    }
			    }
                        }
                        if (lvarShapeType == eShapeType.shpPointM)
                        {
                            lvarM = BitConverter.ToDouble(vertData, 20);
                            if (vertData.Length > 20)
                            {
				    if (lvarM > -1E+38)
				    {
					lvarMeasure = lvarM;
				    }
			    }
                        }

                        break;
                    // *******************************
                    // * MultiPoint Shapes           *
                    // *******************************
                    case eShapeType.shpMultiPoint:
                    case eShapeType.shpMultiPointZ:
                    case eShapeType.shpMultiPointM:
                        ArrayPos = 40 + (Index * 16);
                        lvarXCord = BitConverter.ToDouble(vertData, ArrayPos);
                        lvarYCord = BitConverter.ToDouble(vertData, ArrayPos + 8);
                        if (lvarShapeType == eShapeType.shpMultiPointZ)
                        {
                            ArrayPos = 56 + (mvarNoOfPoints * 16) + (Index * 16);
                            lvarZCord = BitConverter.ToDouble(vertData, ArrayPos);
                            ArrayPos = 72 + (mvarNoOfPoints * 24) + (Index * 8);
                            lvarM = BitConverter.ToDouble(vertData, ArrayPos);
                            if (ArrayPos < vertData.Length)
                            {
				    if (lvarM > -1E+38)
				    {
					lvarMeasure = lvarM;
				    }
			    }
                        }
                        if (lvarShapeType == eShapeType.shpMultiPointM)
                        {
                            ArrayPos = 56 + (mvarNoOfPoints * 16) + (Index * 8);
                            if (ArrayPos < vertData.Length)
                            {
                                lvarM = BitConverter.ToDouble(vertData, ArrayPos);
                                if (lvarM > -1E+38)
                                {
                                    lvarMeasure = lvarM;
                                }
                            }
                        }

                        break;
                    // *******************************
                    // * PolyLine and Polygon Shapes *
                    // *******************************
                    case eShapeType.shpPolyLine:
                    case eShapeType.shpPolygon:
                    case eShapeType.shpPolyLineZ:
                    case eShapeType.shpPolygonZ:
                    case eShapeType.shpPolyLineM:
                    case eShapeType.shpPolygonM:
                        //Arc, Polygon
                        ArrayPos = 36;
                        lvarNoOfParts = BitConverter.ToInt32(vertData, ArrayPos);
                        ArrayPos = 44 + (lvarNoOfParts * 4) + (Index  * 16);
                        lvarXCord = BitConverter.ToDouble(vertData, ArrayPos);
                        lvarYCord = BitConverter.ToDouble(vertData, ArrayPos + 8);
                        if (lvarShapeType == eShapeType.shpPolyLineZ | lvarShapeType == eShapeType.shpPolygonZ)
                        {
                            ArrayPos = 60 + (lvarNoOfParts * 4) + (mvarNoOfPoints * 16) + (Index * 8);
                            lvarZCord = BitConverter.ToDouble(vertData, ArrayPos);
                            ArrayPos = 76 + (lvarNoOfParts * 4) + (mvarNoOfPoints * 24) + (Index * 8);
                            if (ArrayPos < vertData.Length)
			    {
				    lvarM = BitConverter.ToDouble(vertData, ArrayPos);
				    if (lvarM > -1E+38)
				    {
					lvarMeasure = lvarM;
				    }
			    }
                        }
                        if (lvarShapeType == eShapeType.shpPolyLineM | lvarShapeType == eShapeType.shpPolygonM)
                        {
                            ArrayPos = 60 + (lvarNoOfParts * 4) + (mvarNoOfPoints * 16) + (Index * 8);
                            if (ArrayPos < vertData.Length)
                            {
                                lvarM = BitConverter.ToDouble(vertData, ArrayPos);
                                if (lvarM > -1E+38)
                                {
                                    lvarMeasure = lvarM;
                                }
                            }
                        }
                        break;
                    // *******************************
                    // * MultiPatch Shapes           *
                    // *******************************
                    case eShapeType.shpMultiPatch:
                        ArrayPos = 36;
                        lvarNoOfParts = BitConverter.ToInt32(vertData, ArrayPos);
                        ArrayPos = 44 + (lvarNoOfParts * 8) + (Index * 16);
                        lvarXCord = BitConverter.ToDouble(vertData, ArrayPos);
                        lvarYCord = BitConverter.ToDouble(vertData, ArrayPos + 8);
                        ArrayPos = 60 + (lvarNoOfParts * 8) + (mvarNoOfPoints * 16) + (Index * 8);
                        lvarZCord = BitConverter.ToDouble(vertData, ArrayPos);
                        ArrayPos = 76 + (lvarNoOfParts * 8) + (mvarNoOfPoints * 24) + (Index * 8);
                        if (ArrayPos < vertData.Length)
                        {
                            lvarM = BitConverter.ToDouble(vertData, ArrayPos);
                            if (lvarM > -1E+38)
                            {
                                lvarMeasure = lvarM;
                            }
                        }
                        break;
                }

                //mvarVertices.ReadMode = FullRead
                Vertice OutVertice = new Vertice();
                OutVertice.X_Cord = lvarXCord;
                OutVertice.Y_Cord = lvarYCord;
                OutVertice.Measure = lvarMeasure;
                OutVertice.Z_Cord = lvarZCord;
                Globals.mvarVerticeChange = false;
                return OutVertice;
            }

        }

        internal void Strip()
        {
            List.Clear();
            mvarNoOfPoints = 0;
        }

        #endregion
    }

    #region **********          Event Argument Classes        **********

    #region Add Vertice Event Arguments
    /// <summary>
    /// Parses the Part Number and the Vertice Index of a newly inserted Vertices collection record
    /// </summary>
    public class AddVertArgs : EventArgs
    {
        private int nVerticeNo;
        private bool nInsert;
        /// <summary>
        /// An event fired when a vertice is added
        /// </summary>
        /// <param name="nVerticeNo">The number of the vertice in the collection</param>
        /// <param name="nInsert">Was the vertice inserted</param>
        public AddVertArgs( int nVerticeNo, bool nInsert)
        {
            this.nVerticeNo = nVerticeNo;
            this.nInsert = nInsert;
        }
        /// <summary>
        /// The number of the vertice in the collection
        /// </summary>
        public int VerticeNo { get { return nVerticeNo; } }
        /// <summary>
        /// Was the vertice inserted
        /// </summary>
        public bool Insert { get { return nInsert; } }
    }
    #endregion

    #region Delete Vertice Event Arguments
    /// <summary>
    /// Parses the Vertice Index of a newly deleted Vertices collection record
    /// </summary>
    public class DelVertArgs : EventArgs
    {
        private int nVerticeNo;
        /// <summary>
        ///  An event fired when a vertice is added
        /// </summary>
        /// <param name="nVerticeNo">The number of the vertice in the collection</param>
        public DelVertArgs( int nVerticeNo)
        {
            this.nVerticeNo = nVerticeNo;
        }
        /// <summary>
        /// The number of the vertice in the collection
        /// </summary>
        public int VerticeNo { get { return nVerticeNo; } }
    }
    #endregion

    #region Add Part Event Arguments
    internal class AddPartArgs : EventArgs
    {
        private ePartType nPartType;

        internal AddPartArgs(ePartType PartType)
        { nPartType = PartType; }

        /// <summary>
        /// The MultiPatch Part Type
        /// </summary>
        public ePartType PartType { get { return nPartType; } }

    }
    #endregion

    #endregion
}
