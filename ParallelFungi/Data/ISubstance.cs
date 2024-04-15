using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelFungi.Data
{
    internal interface ISubstance
    {
        Point3d Substance { get; set; }
        double Force { get; set; }
        int Threshold { get; set; }
    }
}
