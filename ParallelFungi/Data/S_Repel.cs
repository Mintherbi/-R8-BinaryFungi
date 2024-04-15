using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelFungi.Data
{
    public struct RepelData : ISubstance
    {
        public Point3d Substance { get; set; }
        public double Force { get; set; }
        public int Threshold { get; set; }
        private double min_force;

        public RepelData(Point3d substance, double force = 10, int threshold = 10, bool direction = true)
        {
            this.Substance = substance;
            this.Force = force *(-1);
            this.Threshold = threshold;
            this.min_force = force / (threshold * threshold);
        }
    }
}
