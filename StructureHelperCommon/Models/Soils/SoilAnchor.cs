using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Soils
{
    public enum DurabilityType
    {
        Temporary,
        Eturnal
    }
    public class SoilAnchor : ISaveable
    {
        private double rootLength;
        private double rootDiameter;
        private double freeLength;
        private double jetTubeDiameter;
        private double waterCementRatio;
        private double boreHoleDiameter;
        private double angleToHorizont;
        private double additionalSurfPressure;
        private double headLevel;

        /// <inheritdoc/>
        public Guid Id {get; private set;}
        /// <summary>
        /// Length of root, m
        /// </summary>
        public double RootLength
        {
            get => rootLength; set
            {
                CheckObject.CheckMinMax(value, 0d, 10d);
                rootLength = value;
            }
        }
        /// <summary>
        /// Diameter of root, m
        /// </summary>
        public double RootDiameter
        {
            get => rootDiameter; set
            {
                CheckObject.CheckMinMax(value, 0d, 1d);
                rootDiameter = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public double GroundLevel { get; set; }
        /// <summary>
        /// Absolute level of head of anchor, m
        /// </summary>
        public double HeadLevel { get => headLevel; set => headLevel = value; }
        /// <summary>
        /// Free Length, m
        /// </summary>
        public double FreeLength
        {
            get => freeLength; set
            {
                CheckObject.CheckMinMax(value, 0d, 20d);
                freeLength = value;
            }
        }
        /// <summary>
        /// Diameter of boregole, m
        /// </summary>
        public double BoreHoleDiameter
        {
            get => boreHoleDiameter; set
            {
                CheckObject.CheckMinMax(value, 0d, 1d);
                boreHoleDiameter = value;
            }
        }
        /// <summary>
        /// Diameter of tube for jetting of mortar
        /// </summary>
        public double JetTubeDiameter
        {
            get => jetTubeDiameter; set
            {
                CheckObject.CheckMinMax(value, 0d, 0.1d);
                jetTubeDiameter = value;
            }
        }
        /// <summary>
        /// Water-Cement ratio of jetting mortar
        /// </summary>
        public double WaterCementRatio
        {
            get => waterCementRatio; set
            {
                CheckObject.CheckMinMax(value, 0d, 3d);
                waterCementRatio = value;
            }
        }
        /// <summary>
        /// Angle between horizontal plane and axis of anchor, degree
        /// </summary>
        public double AngleToHorizont
        {
            get => angleToHorizont; set
            {
                CheckObject.CheckMinMax(value, 0d, 60d);
                angleToHorizont = value;
            }
        }
        /// <summary>
        /// Additional pressure on the surface of ground, Pa
        /// </summary>
        public double AdditionalSurfPressure
        {
            get => additionalSurfPressure; set
            {
                CheckObject.CheckMinMax(value, 0d, 2e4d); //20kPa
                additionalSurfPressure = value;
            }
        }
        public DurabilityType DurabilityType {get;set;}


        public SoilAnchor(Guid id)
        {
            Id = id;
            RootLength = 6;
            HeadLevel = -4d;
            FreeLength = 7d;
            BoreHoleDiameter = 0.145d;
            RootDiameter = boreHoleDiameter / 0.9d;
            JetTubeDiameter = 0.025d;
            WaterCementRatio = 0.45d;
            AngleToHorizont = 15;
            DurabilityType = DurabilityType.Temporary;
        }

        public SoilAnchor() : this(Guid.NewGuid()) { }

    }
}
