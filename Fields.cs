using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ArcShapeFile
{
    /// <summary>
    /// The Collection of all Database field information and data values
    /// </summary>
    /// <remarks>
    /// <para>The Fields collection represents a single row from the Shape DBF.   It always points to the <see cref="ArcShapeFile.ShapeFile.CurrentRecord">current record</see> of the ShapeFiles object.</para>
    /// <para>You can refer to each Field object within the collection by:
    /// <ul>
	/// <li>Iteration by using the 0 based ordinal - i.e. for(int i=0;i &lt; shp.Fields.Count; i++) .</li>
	/// <li>Iteration by reference - i.e. foreach(Field fd in shp.Fields) .</li>
	/// <li>By referencing the item by FieldName - i.e. shp.Fields["myname"] .</li>
    /// </ul>
    /// </para></remarks>
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [Guid("1BBEA337-0B68-418c-B380-BB54B38866B0")]
    public class Fields : System.Collections.CollectionBase
    {
        #region **********          Local Variables               **********
        
        bool mvarIsDeleted = false;
        static bool mvarFixFieldDupls = false;
        // Database Variables;
        private Int16 dbfHeaderLength;
        private Int16 dbfRecordlength;
        private eLanguage dbfLanguage;
        private string dbfDelimiter = ".";
        private int dbfRecordCount;

        internal Int16 HeaderLength
        {
            get { return dbfHeaderLength; }
            set { dbfHeaderLength = value; }
        }
        internal Int16 Recordlength
        {
            get { return dbfRecordlength; }
            set { dbfRecordlength = value; }
        }
        internal eLanguage Language
        {
            get { return (eLanguage)dbfLanguage; }
            set { dbfLanguage = value; }
        }
        internal string Delimiter
        {
            get{return dbfDelimiter;}
            set {dbfDelimiter=value;}
        }
        internal int RecordCount
        {
            get { return dbfRecordCount; }
            set { dbfRecordCount = value; }
        }

        #endregion

        #region **********          Fields  Methods                **********

        /// <summary>
        /// Creates a new ShapeFile Database field by defining the field in detail
        /// </summary>
        /// <param name="Name">Field name of the ShapeFile Database record</param>
        /// <param name="Type">Type of field to be created</param>
        /// <param name="Size">The length of the field</param>
        /// <param name="Decimal">The number of digits to be stored right of the decimal point</param>
        /// <remarks>
        /// Use the Add method to append a field definition into the collection.  To physically write the definition to the DBF file you must use the
        /// <see cref="ArcShapeFile.ShapeFile.WriteFieldDefs">WriteFieldDefs</see> method.  You can aren't limited to adding field definitions to new shapefiles - you can append them to existing ones too.
        /// </remarks>
        public void Add(string Name, eFieldType Type, short Size, short Decimal)
        { CreateField(Name, Type, Size, Decimal); }
        /// <summary>
        /// Creates a new ShapeFile Database field using name, type and size
        /// </summary>
        /// <param name="Name">Field name of the ShapeFile Database record</param>
        /// <param name="Type">Type of field to be created</param>
        /// <param name="Size">The length of the field</param>
        /// <remarks>
        /// Use the Add method to append a field definition into the collection.  To physically write the definition to the DBF file you must use the
        /// <see cref="ArcShapeFile.ShapeFile.WriteFieldDefs">WriteFieldDefs</see> method.  You can aren't limited to adding field definitions to new shapefiles - you can append them to existing ones too.
        /// </remarks>
        public void Add(string Name, eFieldType Type, short Size)
        { CreateField(Name, Type, Size, -1); }
        /// <summary>
        /// Creates a new ShapeFile Database field using name and predetermined type
        /// </summary>
        /// <param name="Name">Field name of the ShapeFile Database record</param>
        /// <param name="Type">Type of field to be created</param>
        /// <remarks>
        /// Use the Add method to append a field definition into the collection.  To physically write the definition to the DBF file you must use the
        /// <see cref="ArcShapeFile.ShapeFile.WriteFieldDefs">WriteFieldDefs</see> method.  You can aren't limited to adding field definitions to new shapefiles - you can append them to existing ones too.
        /// </remarks>
        public void Add(string Name, eFieldType Type)
        { CreateField(Name, Type, -1, -1); }
        /// <summary>
        /// Creates a new ShapeFile Database field from a field definition
        /// </summary>
        /// <param name="thisField">The ShapeFile Field definition to be added</param>
        /// <remarks>
        /// Use the Add method to append a field definition into the collection.  To physically write the definition to the DBF file you must use the
        /// <see cref="ArcShapeFile.ShapeFile.WriteFieldDefs">WriteFieldDefs</see> method.  You can aren't limited to adding field definitions to new shapefiles - you can append them to existing ones too.
        /// </remarks>
        public void Add(Field thisField)
        { CreateField(thisField.Name, thisField.Type, thisField.Size, thisField.Decimal); }

        private void CreateField(string Name, eFieldType Type, short Size, short Decimal)
        {
            //Check existance of Field Name
            if (Name.Length>10){Name=Name.Substring(0,10);}

            foreach(Field testField in List)
            {
                if(testField.Name==Name.ToUpper() & testField.Name!="SHAPE_ID")
                {
                    if (mvarFixFieldDupls == false) { throw new System.ArgumentException("A Field already exists with this name", Name); }
                    else 
                    {
                        bool lvarFoundName = true;
                        int i = 0;
                        string newName = null;
                        while (lvarFoundName == true)
                        {
                            lvarFoundName = false;
                            i++;
                            newName = Name.Substring(0, Name.Length - i.ToString().Length) + i.ToString();
                            foreach (Field nameField in List)
                            {
                                if (nameField.Name == newName) { break; }
                            }
                        }
                        Name = newName;
                    }
                }
            }
            //create a new object
            Field objNewMember = default(Field);
            objNewMember = new Field();

            //set the properties passed into the method
            objNewMember.Name = Name.ToUpper();
            objNewMember.Status = "A";
            objNewMember.Type = Type;
            switch (Type)
            {case eFieldType.shpBoolean:
                objNewMember.Decimal = 0;
                objNewMember.Size = 1;
                break;
            case eFieldType.shpDate:
                objNewMember.Decimal = 0;
                objNewMember.Size = 8;
                break;
            case eFieldType.shpDouble:
                if (Decimal == -1) { objNewMember.Decimal = 10; }
                else { objNewMember.Decimal = Decimal; }
                if (Size == -1) { objNewMember.Size = 30; }
                else { objNewMember.Size = Size; }
                break;
            case eFieldType.shpLong:
                objNewMember.Decimal = 0;
                if (Size == -1) { objNewMember.Size = 10; }
                else { objNewMember.Size = Size; }
                break;
            case eFieldType.shpInteger:
                objNewMember.Decimal = 0;
                if (Size == -1) { objNewMember.Size = 5; }
                else { objNewMember.Size = Size; }
                break;
            case eFieldType.shpFloat:
                if (Decimal == -1) { objNewMember.Decimal = 11; }
                else { objNewMember.Decimal = Decimal; }
                if (Size == -1) { objNewMember.Size = 19; }
                else { objNewMember.Size = Size; }
                break;
            case eFieldType.shpSingle:
                if (Decimal == -1) { objNewMember.Decimal = 5; }
                else { objNewMember.Decimal = Decimal; }
                if (Size == -1) { objNewMember.Size = 20; }
                else { objNewMember.Size = Size; }
                break;
            case eFieldType.shpText:
                objNewMember.Decimal = 0;
                if (Size == -1) { objNewMember.Size = 10; }
                else { objNewMember.Size = Size; }
                break;
           default:
               if (Decimal == -1) { objNewMember.Decimal = 0; }
               else { objNewMember.Decimal = Decimal; }
               if (Size == -1) { objNewMember.Size = 10; }
               else { objNewMember.Size = Size; }
               break;
            }
            List.Add(objNewMember);
            objNewMember = null;
        }

        /// <summary>
        /// Defines if duplicate field names should be corrected by adding a number to the end of the field name
        /// </summary>
        /// <remarks>As with all databases - your field names need to be unique but as the .DBF data format limits the field name to 10 characters this can sometimes be a bit of a problem.  
        /// When you set this property to true every field name is checked when added.  If it isn't unique then the field name has a counter added to it (e.g. MYNAME1, MYNAME2).  If the field name is larger than
        /// 10 charcters in length then it will be truncated before the counter is added.</remarks>
        public bool FixFieldNames
        {
            get { return mvarFixFieldDupls; }
            set { mvarFixFieldDupls = value; }
        }

        /// <summary>
        /// Marks a field for deletion
        /// </summary>
        /// <param name="vntIndexKey">The Index if the field to be removed</param>
        /// <remarks>
        /// This method marks the indicated Field for removal from the collection.  This uses the same process as the Field <see cref="ArcShapeFile.Field.Delete">Delete</see> method in the Field object.  When the <see cref="ArcShapeFile.ShapeFile.WriteFieldDefs">WriteFieldDefs</see> method is used the removal is made permanent.
        /// </remarks>
        /// <seealso cref="ArcShapeFile.Field.Delete">Field Delete</seealso>
        /// <seealso cref="ArcShapeFile.Field.UnDelete">Field UnDelete</seealso>
        /// <seealso cref="ArcShapeFile.ShapeFile.Pack">Pack</seealso>
        public new void RemoveAt(int vntIndexKey)
        {
            Field mField = (Field)List[vntIndexKey];
            mField.Delete();
        }

        #endregion

        #region **********          Fields Properties              **********

        /// <summary>
        /// Reports if the database record is Deleted
        /// </summary>
        /// <remarks>Deleted records are often associated with Null shape records.  Creating a Null record using <see cref="ArcShapeFile.ShapeFile.AddNullShape">AddNullShape</see> will automatically add a new record to the database, but set the 
        /// delete flag.  This property will tell you the status of the current record.</remarks>
        public bool isDeleted
        {
            get { return mvarIsDeleted; }
            set { mvarIsDeleted = value; }
        }
        /// <summary>
        /// Grabs the Field record from the collection by using its ordinal position within the collection
        /// </summary>
        /// <param name="Index">The index within the collection</param>
        public Field this[int Index]
        {
            get
            {
                return (Field)List[Index];
            }
        }
        /// <summary>
        /// Grabs the Field record from the collection by using the FieldName
        /// </summary>
        /// <param name="FieldName">The field name listed in the collection</param>
        public Field this[string FieldName]
        {
            get
            {
                int retIndex = -1;
                for (int Index = 0; Index < List.Count; Index++)
                {
                    Field testField = (Field)List[Index];
                    if (testField.Name == FieldName.ToUpper())
                    {
                        retIndex = Index;
                        break;
                    }
                }
                if (retIndex > -1)
                { return (Field)List[retIndex]; }
                else
                { return null; }

            }
        }

        #endregion

        #region **********          Internal Methods              **********

        /// <summary>
        /// Removes the data values from the Fields Collection but leaves the structure behind
        /// </summary>
        internal void Strip()
        {
            foreach (Field mField in List)
            { mField.Value = null; }
        }

        #endregion

    }
}
