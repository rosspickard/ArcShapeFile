using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ArcShapeFile
{
    /// <summary>
    /// Contains the X, Y, Z coordinates and measure information of a vertice
    /// </summary>
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [Guid("1BBEA337-0B68-418c-B380-BB54B38866A1")]
    public class Vertice : ArcShapeFile.IVertice    
    {

        #region **********          Local Variables               **********

        private double mvarX_Cord;
        private double mvarY_Cord;
        private double? mvarZ_Cord;
        private double? mvarMeasure;
        //private int mvarPartNo;
        //private ePartType mvarPartType;

        #endregion

        #region **********          Public Vertice Properties     **********


        /// <summary>
        /// The X component of the cartesian coordinate
        /// </summary>
        /// <remarks>
        /// This is one part of the standard 2 dimesional cartesian plain coordinate pair.  Adding new Vertices should be done through the <see cref="Vertices.Add(Vertice)"/> method.
        /// The <see cref="ArcShapeFile.ShapeFile.ModifyShape">ModifyShape</see> method will write any changes to existing vertices back to the ShapeFile.
        /// </remarks>
        public double X_Cord
        {
            get { return mvarX_Cord; }
            set
            {
                if (mvarX_Cord != value)
                    Globals.mvarVerticeChange = true;
                mvarX_Cord = value;
            }
        }

        /// <summary>
        /// The Y component of the cartesian coordinate
        /// </summary>
        /// <remarks>
        /// This is one part of the standard 2 dimesional cartesian plain coordinate pair.  Adding new Vertices should be done through the <see cref="Vertices.Add(Vertice)"/> method.
        /// The <see cref="ArcShapeFile.ShapeFile.ModifyShape">ModifyShape</see> method will write any changes to existing vertices back to the ShapeFile.
        /// </remarks>
        public double Y_Cord
        {
            get { return mvarY_Cord; }
            set
            {
                if (mvarY_Cord != value)
                    Globals.mvarVerticeChange = true;
                mvarY_Cord = value;
            }
        }

        /// <summary>
        /// The Z or height component of the current 3 dimensional Vertice record
        /// </summary>
        /// <remarks>
        /// Z coordinates are only applicable to shapes of type X,Y,Z Measure.  The Z_Cord value may be null. Null values are written to ShapeFiles as any value less than 10<sup>-38</sup>
        /// </remarks>
        public double? Z_Cord
        {
            get { return mvarZ_Cord; }
            set
            {
                if (mvarZ_Cord != value)
                    Globals.mvarVerticeChange = true;
                mvarZ_Cord = value;
            }
        }

        /// <summary>
        /// The measure component of the current Vertice record
        /// </summary>
        /// <remarks>
        /// Measures are only applicable to shapes of type X,Y Measure or X,Y,Z Measure.  The Measure value may be null. Null values are written to ShapeFiles as any value less than 10<sup>-38</sup>
        /// </remarks>
        public double? Measure
        {
            get { return mvarMeasure; }
            set
            {
                if (!mvarMeasure.Equals(value))
                {
                    Globals.mvarVerticeChange = true;
                }
                mvarMeasure = value;
            }
        }

        #endregion


    }
}
