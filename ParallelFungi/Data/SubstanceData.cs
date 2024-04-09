using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace ParallelFungi.Data
{
    public struct SubstanceData
    {
        public Point3d substance { get; set; }
        public double force { get; set; }
        public int threshold { get; set; }
        public bool direction { get; set; }         //false : repel, true : atrract
        private double max_force;

        public SubstanceData(Point3d substance, double force = 10, int threshold=10, bool direction=true)
        {
            this.substance = substance;
            this.force = force;
            this.threshold = threshold;
            this.direction = direction;
            this.max_force = force / (threshold * threshold);
        }
    }
}
