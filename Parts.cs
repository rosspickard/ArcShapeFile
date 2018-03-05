using System;
using System.Collections.Generic;
using System.Text;

namespace ArcShapeFile
{
    /// <summary>
    /// The <b>Parts</b> Collection consists of one or more <b>Part</b> objects, each representing the beginning and end point of each ring or part in the current record.
    /// </summary>
    /// <remarks>Some ShapeFile records consist of polygons with donuts inside them or multiple shapes that share the same database attribution.  With the <b>Parts</b> collection
    /// <para>I've tried to give you a bit more information about these features, including the start and end ordinal of the vertices that make up each ring, the centroid of the ring and a few other bits and pieces.</para>
    /// <para>ou can refer to each <B>Part</B> object within the collection by:
    /// <ul>
	/// <li>Iteration by using the 0 based ordinal - i.e. for(int i=0;i &lt; shp.Parts.Count; i++)</li>
	/// <li>Iteration by reference - i.e. foreach(Part pt in shp.Parts)</li>
    /// </ul></para></remarks>
    public class Parts : System.Collections.CollectionBase
    {

        #region **********          Internal Methods              **********

        internal void Add(int Begins)
        {
            //create a new object
            Part objNewMember = default(Part);
            objNewMember = new Part();

            //set the properties passed into the method
            objNewMember.Begins = Begins;

            List.Add(objNewMember);

        }

        #endregion

        #region **********          Public Methods                **********

        /// <summary>
        /// Grabs the Part information based on the Index
        /// </summary>
        /// <param name="Index">The index of the Part within the collection</param>
        public Part this[int Index]
        {
            get
            {
                return (Part)List[Index];
            }
        }

        #endregion

    }
}
