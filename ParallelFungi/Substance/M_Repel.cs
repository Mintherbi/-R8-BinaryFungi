using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace ParallelFungi.Substance
{
    public struct RepelMesh : ISubstance
    {
        public Mesh Substance { get; set; }
        public double Force { get; set; }
        public int Threshold { get; set; }
        private double max_force;

        public RepelMesh(Mesh substance, double force = 10, int threshold = 10, bool direction = true)
        {
            Substance = substance;
            Force = force;
            Threshold = threshold;
            max_force = force / (threshold * threshold);
        }
    }
}
