using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace ParallelFungi.Data
{
    public struct AttractData : ISubstance
    {
        public Point3d Substance { get; set; }
        public double Force { get; set; }
        public int Threshold { get; set; }
        private double max_force;

        public AttractData(Point3d substance, double force = 10, int threshold=10, bool direction=true)
        {
            this.Substance = substance;
            this.Force = force;
            this.Threshold = threshold;
            this.max_force = force / (threshold * threshold);
        }
    }
}
