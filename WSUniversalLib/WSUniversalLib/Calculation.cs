using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSUniversalLib
{
    public class Calculation
    {
        public int CalculateRawMaterial(int count, float width, float length, float productType, float materialType)
        {
            double area = width * length;
            double rawMaterialNeeded = 0;
            double coefficient = 0;
            switch (productType)
            {
                case 1:
                    coefficient = 1.1;
                    break;
                case 2:
                    coefficient = 2.5;
                    break;
                case 3:
                    coefficient = 8.43;
                    break;
                default:
                    return -1;
            }
            double materialDefectRate = 0;
            switch (materialType)
            {
                case 1:
                    materialDefectRate = 0.003;
                    break;
                case 2:
                    materialDefectRate = 0.0012;
                    break;
                default:
                    return -1;
            }
            rawMaterialNeeded = count * area * coefficient / (1 - materialDefectRate);
            int roundedRawMaterialNeeded = Convert.ToInt32(Math.Ceiling(rawMaterialNeeded));

            return roundedRawMaterialNeeded;
        }
    }
}
