using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelFungi.Data
{
    public struct GrowthData
    {
        public int growth_rate { get; set; }
        public double branch_probability { get; set; }
        public double neighbor_sensing_sensitivity { get; set; }
        public double quad_decay_threshold { get; set; }
        public GrowthData(int growth_rate = 1, double branch_probability = 0.1, double neighbor_sensing_sensitivity = 3, double quad_decay_threshold = 5)
        {
            this.growth_rate = growth_rate;
            this.branch_probability = branch_probability;
            this.neighbor_sensing_sensitivity = neighbor_sensing_sensitivity;
            this.quad_decay_threshold = quad_decay_threshold;
        }
    }
}
