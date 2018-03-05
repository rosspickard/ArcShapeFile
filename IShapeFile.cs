using System;
namespace ArcShapeFile
{
    //[Guid("EA3FAAC9-C49F-49dd-80AA-785D877012F9")]
    interface IShapeFile
    {
        void AddNullShape();
        double? Area { get; }
        bool BOF { get; }
        double CentroidX { get; }
        double CentroidY { get; }
        void Close();
        void CopyFrom(string ShapeFileName, int RecordNumber);
        void CopyTo(string ShapeFileName, int RecordNumber);
        void CopyTo(string ShapeFileName);
        bool CreateEmpty { get; set; }
        void WriteShape();
        int CurrentRecord { get; }
        void DeletePart(int PartNo);
        void DeleteShape();
        void EmptyShape();
        bool EOF { get; }
        Fields Fields { get; }
        void FindFirst(string QueryString);
        void FindFirst(double InX, double InY);
        void FindFirst(double InX, double InY, double Tolerance);
        void FindNext();
        bool IsVisible(double InX, double InY, double Tolerance);
        bool IsVisible(double InX, double InY);
        eLanguage Language { get; set; }
        void LoadDBFData();
        void LoadFieldDefs(string DBFFileName);
        void LoadShapeData();
        double? mMax { get; }
        double? mMin { get; }
        void ModifyShape();
        void MoveFirst();
        void MoveLast();
        void MoveNext();
        void MovePrevious();
        void MoveTo(int Index);
        bool NoMatch { get; }
        void Open(string filename);
        void Open(string filename, eShapeType shapetype, bool lockfile);
        void Open(string filename, bool lockfile);
        void Open(string filename, eShapeType shapetype);
        void Pack();
        Parts Parts { get; }
        double Perimeter { get; }
        Projection Projection { get; set; }
        eReadMode ReadMode { get; set; }
        int RecordCount { get; }
        bool RetainData { get; set; }
        void SetPartDirection(int PartNo, eDirection Orientation);
        string ShapeFileName { get; }
        eShapeType ShapeType { get; }
        bool TestForHoles { get; set; }
        void UnDeleteShape();
        void UpdateMBR();
        Vertices Vertices { get; }
        void WriteFieldDefs();
        void WriteProjection(eGeocentricDatums Datum);
        void WriteProjection(eGeographicDatums Projection);
        double xMax { get; }
        double xMin { get; }
        double yMax { get; }
        double yMin { get; }
        double? zMax { get; }
        double? zMin { get; }
    }
}
