using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Copyright (c) 2023 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperCommon.Models.Sections
{
    public class AccidentalEccentricityLogic : IAccidentalEccentricityLogic
    {
        private double lengthFactor;
        private double sizeFactor;
        private double minEccentricity;

        public double Length { get; set; }
        public double SizeX { get; set; }
        public double SizeY { get; set; }
        public IForceTuple InitialForceTuple { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }
        public AccidentalEccentricityLogic()
        {
            lengthFactor = 600d;
            sizeFactor = 30d;
            minEccentricity = 0.01d;
        }
        public ForceTuple GetForceTuple()
        {
            var lengthEccetricity = Length / lengthFactor;
            TraceLogger?.AddMessage(string.Format("Accidental eccentricity by length ea = {0} / {1} = {2}", Length, lengthFactor, lengthEccetricity));
            var sizeXEccetricity = SizeX / sizeFactor;
            TraceLogger?.AddMessage(string.Format("Accidental eccentricity by SizeX ea ={0} / {1} = {2}", SizeX, sizeFactor, sizeXEccetricity));
            var sizeYEccetricity = SizeY / sizeFactor;
            TraceLogger?.AddMessage(string.Format("Accidental eccentricity by SizeY ea ={0} / {1} = {2}", SizeY, sizeFactor, sizeYEccetricity));
            TraceLogger?.AddMessage(string.Format("Minimum accidental eccentricity ea = {0}", minEccentricity));
            var xEccentricity = Math.Abs(InitialForceTuple.My / InitialForceTuple.Nz);
            TraceLogger?.AddMessage(string.Format("Actual eccentricity e0,x = {0}", xEccentricity));
            var yEccentricity = Math.Abs(InitialForceTuple.Mx / InitialForceTuple.Nz);
            TraceLogger?.AddMessage(string.Format("Actual eccentricity e0,y = {0}", yEccentricity));

            var xFullEccentricity = new List<double>()
            {
                lengthEccetricity,
                sizeXEccetricity,
                minEccentricity,
                xEccentricity
            }
            .Max();
            string mesEx = string.Format("Eccentricity e,x = max({0}; {1}; {2}; {3}) = {4}",
                lengthEccetricity, sizeXEccetricity,
                minEccentricity, xEccentricity,
                xFullEccentricity);
            TraceLogger?.AddMessage(mesEx);
            var yFullEccentricity = new List<double>()
            {
                lengthEccetricity,
                sizeYEccetricity,
                minEccentricity,
                yEccentricity
            }
            .Max();
            string mesEy = string.Format("Eccentricity e,y = max({0}; {1}; {2}; {3}) = {4}",
                lengthEccetricity, sizeYEccetricity,
                minEccentricity, yEccentricity,
                yFullEccentricity);
            TraceLogger?.AddMessage(mesEy);
            var xSign = InitialForceTuple.Mx == 0d ? -1d : Math.Sign(InitialForceTuple.Mx);
            var ySign = InitialForceTuple.My == 0d ? -1d : Math.Sign(InitialForceTuple.My);
            var mx = (-1d) * InitialForceTuple.Nz * yFullEccentricity * xSign;
            var my = (-1d) * InitialForceTuple.Nz * xFullEccentricity * ySign;
            TraceLogger?.AddMessage(string.Format("Bending moment arbitrary X-axis Mx = {0} * {1} = {2}", InitialForceTuple.Nz, yFullEccentricity, mx), TraceLogStatuses.Debug);
            TraceLogger?.AddMessage(string.Format("Bending moment arbitrary Y-axis My = {0} * {1} = {2}", InitialForceTuple.Nz, xFullEccentricity, my), TraceLogStatuses.Debug);

            var newTuple = new ForceTuple()
            {
                Mx = mx,
                My = my,
                Nz = InitialForceTuple.Nz,
                Qx = InitialForceTuple.Qx,
                Qy = InitialForceTuple.Qy,
                Mz = InitialForceTuple.Mz,
            };
            TraceLogger?.AddEntry(new TraceTablesFactory().GetByForceTuple(newTuple));
            return newTuple;
        }
    }
}
