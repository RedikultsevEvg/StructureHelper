using StructureHelper.Infrastructure.UI.Converters.Units;
using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.Units;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.LimitCurve;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

//Copyright (c) 2023 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    public class SurroundDataViewModel : OkCancelViewModelBase, IDataErrorInfo
    {
        const string ForceUnitString = "kN";
        const string MomentUnitString = "kNm";
        static IConvertUnitLogic operationLogic = new ConvertUnitLogic();
        static IGetUnitLogic unitLogic = new GetUnitLogic();
        public SurroundData SurroundData
        {
            get => surroundData; set
            {
                surroundData = value;
                RefreshAll();
            }
        }

        public void RefreshAll()
        {
            OnPropertyChanged(nameof(Logic));
            OnPropertyChanged(nameof(XMax));
            OnPropertyChanged(nameof(XMin));
            OnPropertyChanged(nameof(YMax));
            OnPropertyChanged(nameof(YMin));
            OnPropertyChanged(nameof(ConstZ));
            OnPropertyChanged(nameof(XLabel));
            OnPropertyChanged(nameof(YLabel));
            OnPropertyChanged(nameof(ZLabel));
            OnPropertyChanged(nameof(XUnitLabel));
            OnPropertyChanged(nameof(YUnitLabel));
            OnPropertyChanged(nameof(ZUnitLabel));
        }

        private static IUnit unitForce = unitLogic.GetUnit(UnitTypes.Force, ForceUnitString);
        private static IUnit unitMoment = unitLogic.GetUnit(UnitTypes.Moment, MomentUnitString);
        private SurroundData surroundData;

        public IValueConverter ForceConverter { get => new Force(); }
        public IValueConverter MomentConverter { get => new Moment();}
        public List<ConstOneDirectionConverter> Logics { get; }
        public ConstOneDirectionConverter Logic
        {
            get
            {
                var logic = SurroundData.ConvertLogicEntity;
                return logic;
            }
            set
            {
                SurroundData.ConvertLogicEntity = value;
                OnPropertyChanged(nameof(Logic));
                OnPropertyChanged(nameof(XLabel));
                OnPropertyChanged(nameof(YLabel));
                OnPropertyChanged(nameof(ZLabel));
                OnPropertyChanged(nameof(XUnitLabel));
                OnPropertyChanged(nameof(YUnitLabel));
                OnPropertyChanged(nameof(ZUnitLabel));
            }
        }

        public string XLabel { get => SurroundData.ConvertLogicEntity.XAxisName; }
        public string YLabel { get => SurroundData.ConvertLogicEntity.YAxisName; }
        public string ZLabel { get => SurroundData.ConvertLogicEntity.ConstAxisName; }
        public string XUnitLabel => GetLabel(SurroundData.ConvertLogicEntity.XForceType);
        public string YUnitLabel => GetLabel(SurroundData.ConvertLogicEntity.YForceType);
        public string ZUnitLabel => GetLabel(SurroundData.ConvertLogicEntity.ZForceType);

        public double XMax
        {
            get
            {
                var factor = GetFactor(SurroundData.ConvertLogicEntity.XForceType);
                return SurroundData.XMax * factor;
            }

            set
            {
                var factor = GetFactor(SurroundData.ConvertLogicEntity.XForceType);
                SurroundData.XMax = value / factor;
                OnPropertyChanged(nameof(XMax));
            }
        }

        public double XMin
        {
            get
            {
                var factor = GetFactor(SurroundData.ConvertLogicEntity.XForceType);
                return SurroundData.XMin * factor;
            }

            set
            {
                var factor = GetFactor(SurroundData.ConvertLogicEntity.XForceType);
                SurroundData.XMin = value / factor;
                OnPropertyChanged(nameof(XMin));
            }
        }

        public double YMax
        {
            get
            {
                var factor = GetFactor(SurroundData.ConvertLogicEntity.YForceType);
                return SurroundData.YMax * factor;
            }

            set
            {
                var factor = GetFactor(SurroundData.ConvertLogicEntity.YForceType);
                SurroundData.YMax = value / factor;
                OnPropertyChanged(nameof(YMax));
            }
        }

        public double YMin
        {
            get
            {
                var factor = GetFactor(SurroundData.ConvertLogicEntity.YForceType);
                return SurroundData.YMin * factor;
            }

            set
            {
                var factor = GetFactor(SurroundData.ConvertLogicEntity.YForceType);
                SurroundData.YMin = value / factor;
                OnPropertyChanged(nameof(YMin));
            }
        }

        public double ConstZ
        {
            get
            {
                var factor = GetFactor(SurroundData.ConvertLogicEntity.ZForceType);
                return SurroundData.ConstZ * factor;
            }

            set
            {
                var factor = GetFactor(SurroundData.ConvertLogicEntity.ZForceType);
                SurroundData.ConstZ = value / factor;
                SurroundData.ConvertLogicEntity.ConstDirectionValue = SurroundData.ConstZ;
                OnPropertyChanged(nameof(ConstZ));
            }
        }

        public string Error => throw new NotImplementedException();

        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    //case nameof(PointCount):
                    //    if (PointCount < 24)
                    //    {
                    //        error = "Point count must be greater than 24";
                    //    }
                    //    break;
                }
                return error;
            }
        }

        public SurroundDataViewModel(SurroundData surroundData)
        {
            this.SurroundData = surroundData;
            Logics = new();
            Logics.AddRange(ConvertLogics.ConverterLogics);
            Logic = Logics
                .Where(x => x.Id == surroundData.ConvertLogicEntity.Id)
                .Single();
            OnPropertyChanged(nameof(Logic));
        }
        private static double GetFactor(ForceTypes forceType)
        {
            if (forceType == ForceTypes.Force)
            {
                return unitForce.Multiplyer;
            }
            else if (forceType == ForceTypes.MomentMx || forceType == ForceTypes.MomentMy)
            {
                return unitMoment.Multiplyer;
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(forceType));
            }
        }
        private static string GetLabel(ForceTypes forceType)
        {
            if (forceType == ForceTypes.Force)
            {
                return unitForce.Name;
            }
            else if (forceType == ForceTypes.MomentMx || forceType == ForceTypes.MomentMy)
            {
                return unitMoment.Name;
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(forceType));
            }
        }
    }
}
