using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelFungi.Data
{
    public struct GrowthData
    {
        public double growth_rate { get; set; }
        public double branch_probability { get; set; }
        public double neighbor_sensing_sensitivity { get; set; }
        public double sensing_radius { get; set; }
        public double fusion_radius { get; set; }
        public GrowthData(double growth_rate = 1, double branch_probability = 0.1, double neighbor_sensing_sensitivity = 3, double sensing_radius = 5, double fusion_radius = 1)
        {
            this.growth_rate = growth_rate;
            this.branch_probability = branch_probability;
            this.neighbor_sensing_sensitivity = neighbor_sensing_sensitivity;
            this.sensing_radius = sensing_radius;
            this.fusion_radius = fusion_radius;
        }
    }
}
