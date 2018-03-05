using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ArcShapeFile
{
    /// <summary>
    /// Contains the Database field information and data values.  As each Field object is a representation of the database field of the DBF file format there are some restrictions on both field size (maximum of 245 characters for text as memo fields are not supported) and field names (maximum of 10 characters).
    /// </summary>
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [Guid("A977E6A5-7C5E-4fdd-859C-B53BE4C60433")]
    public class Field : ArcShapeFile.IField
    {
        #region **********          Field Variables           **********

        private object mvarValue=null;
        private string mvarStatus=null; // Status indicators : N=Name Change, T=Type Change, S=Size Change .=DecimalChange, D=Delete
        private string mvarFieldName = "";
        private eFieldType mvarFieldType = eFieldType.shpText;
        private short mvarFieldSize = 10;
        private short mvarFieldDecimal = 0;
        private object missing = null;

        #endregion

        #region **********          Field Properties          **********

        ///<summary>A string representing the name of the database field</summary>
        /// <remarks>
        /// Field objects can't share the same name with any object in the same collection.  You can test for this by setting the <see cref="ArcShapeFile.Fields.FixFieldNames">FixFieldNames</see> property to true, otherwise on your own head be it.
        /// When creating a new <b>ShapeFile</b>, if a Fields collection has not been defined and initialized using the <see cref="ArcShapeFile.ShapeFile.WriteFieldDefs">WriteFieldDefs</see> method then a data file will be created with one Field (SHAPE_ID) when the first <see cref="ArcShapeFile.ShapeFile.WriteShape">WriteShape</see> command is given. The DBF file needs at least one field in it.
        /// </remarks>
        public string Name
        {
	        get { return mvarFieldName; }
	        set { mvarFieldName = value; }
        }

        ///<summary>A short integer representing the type of the database field</summary>
        ///<remarks>
        /// This property determines what kind of data the Field object can hold.  When creating a new Field object some predefined data types exist, in some cases removing the need to provide the Size and Decimal values.
        /// The Type values are described below: 
        ///<list type="table">
        ///	<listheader>
        ///		<enum>Data Type</enum>
        ///		<eval>Value</eval>
        ///		<description>Description</description>
        ///	</listheader>
        ///	 <item>
        ///	 	<enum>ShpNumeric</enum>
        ///		<eval>19</eval>
        ///		<description>Generic numeric input, FieldSize and FieldDecimal values must be defined.  Converts to System.Double</description>
        ///	</item>
        ///	<item>
        ///		<enum>ShpDate</enum>
        ///		<eval>8</eval>
        ///		<description>Date value stored in dBASE as an 8 character value (YYYYMMDD), FieldSize =8, FieldDecimal =  0.  Converts to System.DateTime</description>
        ///	</item>
        ///	<item>
        ///		<enum>ShpText</enum>
        ///		<eval>10</eval>
        ///		<description>Generic character input, FieldSize will default to 10 unless defined, FieldDecimal defaults to 0.  Converts to System.String</description>
        ///	</item>
        ///	<item>
        ///		<enum>ShpBoolean</enum>
        ///		<eval>1</eval>
        ///		<description>Logical True/False value FieldSize = 1, FieldDecimal = 0.  Converts to System.Boolean</description>
        ///	</item>
        ///	<item>
        ///		<enum>ShpDouble</enum>
        ///		<eval>7</eval>
        ///		<description>Double precision numeric, FieldSize = 30, FieldDecimal = 10.  Converts to System.Double</description>
        ///	</item>
        ///	<item>
        ///		<enum>ShpLong</enum>
        ///		<eval>4</eval>
        ///		<description>Big Integer, FieldSize = 10, FieldDecimal = 0.  Converts to System.Int64</description>
        ///	</item>
        ///	<item>
        ///		<enum>ShpInteger</enum>
        ///		<eval>3</eval>
        ///		<description>Small Integer, FieldSize = 5, FieldDecimal = 0.  Converts to System.Int32</description>
        ///	</item>
        ///	<item>
        ///		<enum>ShpSingle</enum>
        ///		<eval>6</eval>
        ///		<description>Single precision numeric, FieldSize = 20, FieldDecimal = 5.  Converts to System.Single</description>
        ///	</item>
        ///	<item>
        ///		<enum>ShpFloat</enum>
        ///		<eval>20</eval>
        ///		<description>Float numeric, FieldSize = 20, FieldDecimal = 5.  Comprises of 16 digit mantissa and 2 digit exponent (e.g. 1.1234567890123456E+04).  Converts to System.Double</description>
        ///	</item>
        ///</list>
        ///</remarks>
        ///<seealso cref="ArcShapeFile.eFieldType">eFieldType ENum</seealso>
        public eFieldType Type
        {
	        get { return mvarFieldType; }
	        set { mvarFieldType = value; }
        }

        ///<summary>A short integer representing the length of the database field</summary>
        ///<remarks>For Fields with the <see cref="Type"/> of shpText (System.String) the value of the Size property cannot exceed 255.  This is a limitation of the DBF format.</remarks>
        public short Size {
	        get { return mvarFieldSize; }
	        set { mvarFieldSize = value; }
        }

        ///<summary>A short integer representing the number of digits to the right of the decimal place</summary>
        /// <remarks>
        /// For fields that contain numeric data, the Decimal property indicates the number of significant digits to the right of the decimal point that the field can hold.  This can be set to default values using predefined data types referred to by the <see cref="Type"/> property.
        /// </remarks>
        public short Decimal
        {
	        get { return mvarFieldDecimal; }
	        set { mvarFieldDecimal = value; }
        }

        ///<summary>A object representing the value of the database field</summary>
        ///<remarks>
        ///The data value in this property will take on the equivalent system data type of the <see cref="Type"/> you defined 
        ///</remarks>
        public object Value
        {
	        get { return mvarValue; }
	        set {
		        if (mvarValue == value) {
                    Globals.mvarFieldChange = false;
		        } else {
                    Globals.mvarFieldChange = true;
			        mvarValue = value;
		        }
	        }
        }

        /// <summary>
        /// Reports if the field has been marked for deletion.  This will not physically occur until the Pack command is given
        /// </summary>
        /// <seealso cref="Delete"/>
        /// <seealso cref="UnDelete"/>
        /// <seealso cref="ArcShapeFile.ShapeFile.Pack">Pack</seealso>
        public bool IsDeleted
        {
            get
            {
                if (mvarStatus.Contains("D"))
                { return true; }
                else { return false; }
            }
        }


        /// <summary>Holds the status of the field - has it been modified, deleted, etc. for use with writing modifications</summary>
        internal string Status {
	        get { return mvarStatus; }
	        set { mvarStatus = value; }
        }

        #endregion

        #region **********          Field Methods             **********
        ///<summary>Change the field name and type of a particular field</summary>
        /// <param name="Name">The new Field name of the ShapeFile Database record</param>
        /// <param name="Type">The new Type of field to be created</param>
        /// <remarks>You can change the details of any field in the Fields collection using this method.  One thing to be aware of though is that 
        /// once you've changed to the details you still have to write them out to the DBF file.  This is done with the <see cref="ArcShapeFile.ShapeFile.ModifyShape">ModifyShape</see>
        /// method.  If you don't do this and start writing out ShapeFile records then you will corrupt your database - so use some common sense here.</remarks>
        public void Modify(string Name, eFieldType Type)
        { ChangeField(Name, Type, -1, -1); }
        ///<summary>Change the field name, type and size of a particular field</summary>
        /// <param name="Name">The new Field name of the ShapeFile Database record</param>
        /// <param name="Type">The new Type of field to be created</param>
        /// <param name="Size">The new length of the field</param>
        /// <remarks>You can change the details of any field in the Fields collection using this method.  One thing to be aware of though is that 
        /// once you've changed to the details you still have to write them out to the DBF file.  This is done with the <see cref="ArcShapeFile.ShapeFile.ModifyShape">ModifyShape</see>
        /// method.  If you don't do this and start writing out ShapeFile records then you will corrupt your database - so use some common sense here.</remarks>
        public void Modify(string Name, eFieldType Type, short Size)
        { ChangeField(Name, Type, Size, -1); }
        ///<summary>Change the field name, type, size and decimal placing of a particular field</summary>
        /// <param name="Name">The new Field name of the ShapeFile Database record</param>
        /// <param name="Type">The new Type of field to be created</param>
        /// <param name="Size">The new length of the field</param>
        /// <param name="Decimal">The new number of digits to be stored right of the decimal point</param>
        /// <remarks>You can change the details of any field in the Fields collection using this method.  One thing to be aware of though is that 
        /// once you've changed to the details you still have to write them out to the DBF file.  This is done with the <see cref="ArcShapeFile.ShapeFile.ModifyShape">ModifyShape</see>
        /// method.  If you don't do this and start writing out ShapeFile records then you will corrupt your database - so use some common sense here.</remarks>
        public void Modify(string Name, eFieldType Type, short Size, short Decimal)
        { ChangeField(Name, Type, Size, Decimal); }
        ///<summary>Change the field name and size of a particular field</summary>
        /// <param name="Name">The new Field name of the ShapeFile Database record</param>
        /// <param name="Size">The new length of the field</param>
        /// <remarks>You can change the details of any field in the Fields collection using this method.  One thing to be aware of though is that 
        /// once you've changed to the details you still have to write them out to the DBF file.  This is done with the <see cref="ArcShapeFile.ShapeFile.ModifyShape">ModifyShape</see>
        /// method.  If you don't do this and start writing out ShapeFile records then you will corrupt your database - so use some common sense here.</remarks>
        public void Modify(string Name, short Size)
        { ChangeField(Name, missing, Size, -1); }
        ///<summary>Change the field name of a particular field</summary>
        /// <param name="Name">The new Field name of the ShapeFile Database record</param>
        /// <remarks>You can change the details of any field in the Fields collection using this method.  One thing to be aware of though is that 
        /// once you've changed to the details you still have to write them out to the DBF file.  This is done with the <see cref="ArcShapeFile.ShapeFile.ModifyShape">ModifyShape</see>
        /// method.  If you don't do this and start writing out ShapeFile records then you will corrupt your database - so use some common sense here.</remarks>
        public void Modify(string Name)
        { ChangeField(Name, missing, -1 , -1); }
        ///<summary>Change the field name, size and decimal placing of a particular field</summary>
        /// <param name="Name">The new Field name of the ShapeFile Database record</param>
        /// <param name="Size">The new length of the field</param>
        /// <param name="Decimal">The new number of digits to be stored right of the decimal point</param>
        /// <remarks>You can change the details of any field in the Fields collection using this method.  One thing to be aware of though is that 
        /// once you've changed to the details you still have to write them out to the DBF file.  This is done with the <see cref="ArcShapeFile.ShapeFile.ModifyShape">ModifyShape</see>
        /// method.  If you don't do this and start writing out ShapeFile records then you will corrupt your database - so use some common sense here.</remarks>
        public void Modify(string Name, short Size, short Decimal)
        { ChangeField(Name, missing, Size, Decimal); }
        ///<summary>Change the field type, size and decimal placing of a particular field</summary>
        /// <param name="Type">The new Type of field to be created</param>
        /// <param name="Size">The new length of the field</param>
        /// <param name="Decimal">The new number of digits to be stored right of the decimal point</param>
        /// <remarks>You can change the details of any field in the Fields collection using this method.  One thing to be aware of though is that 
        /// once you've changed to the details you still have to write them out to the DBF file.  This is done with the <see cref="ArcShapeFile.ShapeFile.ModifyShape">ModifyShape</see>
        /// method.  If you don't do this and start writing out ShapeFile records then you will corrupt your database - so use some common sense here.</remarks>
        public void Modify(eFieldType Type, short Size, short Decimal)
        { ChangeField(null, missing, Size, Decimal); }
        ///<summary>Change the field size and decimal placing of a particular field</summary>
        /// <param name="Size">The new length of the field</param>
        /// <param name="Decimal">The new number of digits to be stored right of the decimal point</param>
        /// <remarks>You can change the details of any field in the Fields collection using this method.  One thing to be aware of though is that 
        /// once you've changed to the details you still have to write them out to the DBF file.  This is done with the <see cref="ArcShapeFile.ShapeFile.ModifyShape">ModifyShape</see>
        /// method.  If you don't do this and start writing out ShapeFile records then you will corrupt your database - so use some common sense here.</remarks>
        public void Modify(short Size, short Decimal)
        { ChangeField(null, missing, Size, Decimal); }
        ///<summary>Change the field size of a particular field</summary>
        /// <param name="Size">The new length of the field</param>
        /// <remarks>You can change the details of any field in the Fields collection using this method.  One thing to be aware of though is that 
        /// once you've changed to the details you still have to write them out to the DBF file.  This is done with the <see cref="ArcShapeFile.ShapeFile.ModifyShape">ModifyShape</see>
        /// method.  If you don't do this and start writing out ShapeFile records then you will corrupt your database - so use some common sense here.</remarks>
        public void Modify(short Size)
        { ChangeField(null, missing, Size, -1); }
        private void ChangeField(string Name, Object Type, short Size, short Decimal)
        {

	        short nFieldSize = 0;
	        short nFieldDecimal = 0;

	        // Exit if there is nothing to do
            //if ((Type == missing))
            //{
            //    if ((Size == -1))
            //    {
            //        if (Decimal == -1) 
            //        { return; }
            //    }
            //}

	        // Do nothing to deleted records
	        if (mvarStatus == "D")
		        return;

	        //Change the FieldName
	        if ((Name != null) & mvarFieldName != Name.ToUpper()) {
		        // Set change flag
		        mvarStatus = mvarStatus + "N";
		        mvarFieldName = Name.ToUpper();
	        }

            if (Type != missing)
            {
		        // Set change flag
		        mvarStatus = mvarStatus + "T";

		        if ((Size == -1)) {
			        nFieldSize = 10;
		        } else if (Size == 0) {
			        nFieldSize = 10;
		        }

		        switch ((eFieldType)Type) {
			        case eFieldType.shpBoolean:
				        nFieldDecimal = 0;
				        nFieldSize = 1;
				        break;
			        case eFieldType.shpDate:
				        nFieldDecimal = 0;
				        nFieldSize = 8;
				        break;
			        case eFieldType.shpDouble:
				        nFieldDecimal = 10;
				        nFieldSize = 30;
				        break;
			        case eFieldType.shpLong:
				        nFieldDecimal = 0;
				        nFieldSize = 10;
				        break;
			        case eFieldType.shpInteger:
				        nFieldDecimal = 0;
				        nFieldSize = 5;
				        break;
			        case eFieldType.shpSingle:
				        nFieldDecimal = 5;
				        nFieldSize = 20;
				        break;
			        case eFieldType.shpText:
				        nFieldDecimal = 0;
				        break;
		        }

		        if (nFieldSize != mvarFieldSize) {
			        mvarStatus = mvarStatus + "S";
			        mvarFieldSize = nFieldSize;
		        }
                mvarFieldType = (eFieldType)Type;


	        }

	        if ((Size != -1)) {
		        if (Size != mvarFieldSize) {
			        mvarStatus +=  "S";
			        mvarFieldSize = Size;
		        }
	        }

	        if ((Decimal != -1)) 
            {
		        if (Decimal != mvarFieldDecimal)
                {
			        mvarStatus +=  ".";
			        mvarFieldDecimal = Decimal;
		        }
	        } 
            else 
            {
		        mvarFieldDecimal = nFieldDecimal;
	        }

        }

        /// <summary> Marks the field for deletion.  This will not physically occur until the Pack command is given.</summary>
        /// <seealso cref="IsDeleted"/>
        /// <seealso cref="UnDelete"/>
        /// <seealso cref="ArcShapeFile.ShapeFile.Pack">Pack</seealso>
        public void Delete()
        {
            mvarStatus += "D";
        }

        /// <summary> Unmarks the field for deletion.  Just in case you've had second thoughts about deleting a Field.</summary>
        /// <seealso cref="IsDeleted"/>
        /// <seealso cref="Delete"/>
        /// <seealso cref="ArcShapeFile.ShapeFile.Pack">Pack</seealso>
        public void UnDelete()
        { 
            mvarStatus = mvarStatus.Replace("D", ""); 
        }


        #endregion
    }
}
