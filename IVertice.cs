using System;
using System.Collections.Generic;
using System.Text;

namespace ArcShapeFile
{
    interface IVertice
    {
        double X_Cord { get; set;}
        double Y_Cord { get; set;}
        double? Z_Cord { get; set;}
        double? Measure { get; set;}

    }
}
