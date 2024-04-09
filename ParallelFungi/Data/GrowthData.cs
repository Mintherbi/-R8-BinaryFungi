using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelFungi.Data
{
    public struct GrowthData
    {
        public int r_growth { get; set; }
        public double r_branch { get; set; }
        public double coef_neighbor_sensing { get; set; }

        public GrowthData(int r_growth = 1, double r_branch = 0.1, double coef_neighbor_sensing = 3)
        {
            this.r_growth = r_growth;
            this.r_branch = r_branch;
            this.coef_neighbor_sensing = coef_neighbor_sensing;
        }
    }
}
