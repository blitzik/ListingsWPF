using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Domain
{
    public class DefaultListingPdfReportSetting : BindableObject
    {
        private bool _isOwnerNameVisible = true;
        public bool IsOwnerNameVisible { get { return _isOwnerNameVisible; } set { _isOwnerNameVisible = value; RaisePropertyChanged(); } }

        private bool _isEmployerVisible = true;
        public bool IsEmployerVisible { get { return _isEmployerVisible; } set { _isEmployerVisible = value; RaisePropertyChanged(); } }

        private bool _isVacationVisible = true;
        public bool IsVacationVisible { get { return _isVacationVisible; } set { _isVacationVisible = value; RaisePropertyChanged(); } }

        private bool _areOtherHoursVisible = true;
        public bool AreOtherHoursVisible { get { return _areOtherHoursVisible; } set { _areOtherHoursVisible = value; RaisePropertyChanged(); } }

        private bool _areWorkedHoursVisible = true;
        public bool AreWorkedHoursVisible { get { return _areWorkedHoursVisible; } set { _areWorkedHoursVisible = value; RaisePropertyChanged(); } }

        private bool _areSiknessHours = true;
        public bool AreSiknessHoursVisible { get { return _areSiknessHours; } set { _areSiknessHours = value; RaisePropertyChanged(); } }

        private bool _areHolidaysHoursVisible = true;
        public bool AreHolidaysHoursVisible { get { return _areHolidaysHoursVisible; } set { _areHolidaysHoursVisible = value; RaisePropertyChanged(); } }

        private bool _areLunchHoursVisible = true;
        public bool AreLunchHoursVisible { get { return _areLunchHoursVisible; } set { _areLunchHoursVisible = value; RaisePropertyChanged(); } }

        private bool _areTotalWorkedHoursVisible = true;
        public bool AreTotalWorkedHoursVisible { get { return _areTotalWorkedHoursVisible; } set { _areTotalWorkedHoursVisible = value; RaisePropertyChanged(); } }

        private bool _isHourlyWageVisible = true;
        public bool IsHourlyWageVisible { get { return _isHourlyWageVisible; } set { _isHourlyWageVisible = value; RaisePropertyChanged(); } }

        private bool _areVacationDaysVisible = true;
        public bool AreVacationDaysVisible { get { return _areVacationDaysVisible; } set { _areVacationDaysVisible = value; RaisePropertyChanged(); } }

        private bool _areDietsVisible = true;
        public bool AreDietsVisible { get { return _areDietsVisible; } set { _areDietsVisible = value; RaisePropertyChanged(); } }

        private bool _arePaidHolidaysVisible = true;
        public bool ArePaidHolidaysVisible { get { return _arePaidHolidaysVisible; } set { _arePaidHolidaysVisible = value; RaisePropertyChanged(); } }

        private bool _areBonusesVisible = true;
        public bool AreBonusesVisible { get { return _areBonusesVisible; } set { _areBonusesVisible = value; RaisePropertyChanged(); } }

        private bool _aAreDollarsVisible = true;
        public bool AreDollarsVisible { get { return _aAreDollarsVisible; } set { _aAreDollarsVisible = value; RaisePropertyChanged(); } } // dunno how to name this field LOL

        private bool _isPrepaymentVisible = true;
        public bool IsPrepaymentVisible { get { return _isPrepaymentVisible; } set { _isPrepaymentVisible = value; RaisePropertyChanged(); } }

        private bool _isSicknessVisible = true;
        public bool IsSicknessVisible { get { return _isSicknessVisible; } set { _isSicknessVisible = value; RaisePropertyChanged(); } }


        public void ResetSettings()
        {
            IsEmployerVisible = true;
            IsOwnerNameVisible = true;
            
            AreWorkedHoursVisible = true;
            AreLunchHoursVisible = true;
            AreOtherHoursVisible = true;
            AreTotalWorkedHoursVisible = true;
            IsVacationVisible = true;
            AreSiknessHoursVisible = true;
            AreHolidaysHoursVisible = true;
            
            IsHourlyWageVisible = true;
            AreVacationDaysVisible = true;
            AreDietsVisible = true;
            ArePaidHolidaysVisible = true;
            AreBonusesVisible = true;
            AreDollarsVisible = true;
            IsPrepaymentVisible = true;
            IsSicknessVisible = true;
        }
    }
}
