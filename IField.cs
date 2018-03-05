using System;
namespace ArcShapeFile
{
    interface IField
    {
        short Decimal { get; set; }
        bool IsDeleted { get;}
        void Modify(string Name, short Size, short Decimal);
        void Modify(string Name, short Size);
        void Modify(eFieldType Type, short Size, short Decimal);
        void Modify(short Size, short Decimal);
        void Modify(short Size);
        void Modify(string Name, eFieldType Type, short Size, short Decimal);
        void Modify(string Name, eFieldType Type);
        void Modify(string Name, eFieldType Type, short Size);
        void Delete();
        void UnDelete();
        string Name { get; set; }
        short Size { get; set; }
        eFieldType Type { get; set; }
        object Value { get; set; }
    }
}
