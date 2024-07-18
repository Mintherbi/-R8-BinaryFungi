using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rhino;

namespace ParallelFungi.Substance
{
    public interface ISubstance
    {
        IGeometry Substance { get; set; }
        double Force { get; set; }
        int Threshold { get; set; }
    }
}
