using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listings.Domain
{
    public class DefaultListingPdfReportSetting : BindableObject
    {
        private bool _isOwnerNameVisible;
        public bool IsOwnerNameVisible
        {
            get { return _isOwnerNameVisible; }
            set
            {
                _isOwnerNameVisible = value;
                RaisePropertyChanged();
                ProcessOnPropertyChanged();
            }
        }

        private bool _isEmployerVisible;
        public bool IsEmployerVisible
        {
            get { return _isEmployerVisible; }
            set
            {
                _isEmployerVisible = value;
                RaisePropertyChanged();
                ProcessOnPropertyChanged();
            }
        }

        private bool _isVacationVisible;
        public bool IsVacationVisible
        {
            get { return _isVacationVisible; }
            set
            {
                _isVacationVisible = value;
                RaisePropertyChanged();
                ProcessOnPropertyChanged();
            }
        }

        private bool _areOtherHoursVisible;
        public bool AreOtherHoursVisible
        {
            get { return _areOtherHoursVisible; }
            set
            {
                _areOtherHoursVisible = value;
                RaisePropertyChanged();
                ProcessOnPropertyChanged();
            }
        }

        private bool _areShortHalfHoursEnabled;
        public bool AreShortHalfHoursEnabled
        {
            get { return _areShortHalfHoursEnabled; }
            set
            {
                _areShortHalfHoursEnabled = value;
                RaisePropertyChanged();
                ProcessOnPropertyChanged();
            }
        }

        private bool _areWorkedHoursVisible;
        public bool AreWorkedHoursVisible
        {
            get { return _areWorkedHoursVisible; }
            set
            {
                _areWorkedHoursVisible = value;
                RaisePropertyChanged();
                ProcessOnPropertyChanged();
            }
        }

        private bool _areSiknessHours;
        public bool AreSiknessHoursVisible
        {
            get { return _areSiknessHours; }
            set
            {
                _areSiknessHours = value;
                RaisePropertyChanged();
                ProcessOnPropertyChanged();
            }
        }

        private bool _areHolidaysHoursVisible;
        public bool AreHolidaysHoursVisible
        {
            get { return _areHolidaysHoursVisible; }
            set
            {
                _areHolidaysHoursVisible = value;
                RaisePropertyChanged();
                ProcessOnPropertyChanged();
            }
        }

        private bool _areLunchHoursVisible;
        public bool AreLunchHoursVisible
        {
            get { return _areLunchHoursVisible; }
            set
            {
                _areLunchHoursVisible = value;
                RaisePropertyChanged();
                ProcessOnPropertyChanged();
            }
        }

        private bool _areTotalWorkedHoursVisible;
        public bool AreTotalWorkedHoursVisible
        {
            get { return _areTotalWorkedHoursVisible; }
            set
            {
                _areTotalWorkedHoursVisible = value;
                RaisePropertyChanged();
                ProcessOnPropertyChanged();
            }
        }

        private bool _isHourlyWageVisible;
        public bool IsHourlyWageVisible
        {
            get { return _isHourlyWageVisible; }
            set
            {
                _isHourlyWageVisible = value;
                RaisePropertyChanged();
                ProcessOnPropertyChanged();
            }
        }

        private bool _areVacationDaysVisible;
        public bool AreVacationDaysVisible
        {
            get { return _areVacationDaysVisible; }
            set
            {
                _areVacationDaysVisible = value;
                RaisePropertyChanged();
                ProcessOnPropertyChanged();
            }
        }

        private bool _areDietsVisible;
        public bool AreDietsVisible
        {
            get { return _areDietsVisible; }
            set
            {
                _areDietsVisible = value;
                RaisePropertyChanged();
                ProcessOnPropertyChanged();
            }
        }

        private bool _arePaidHolidaysVisible;
        public bool ArePaidHolidaysVisible
        {
            get { return _arePaidHolidaysVisible; }
            set
            {
                _arePaidHolidaysVisible = value;
                RaisePropertyChanged();
                ProcessOnPropertyChanged();
            }
        }

        private bool _areBonusesVisible;
        public bool AreBonusesVisible
        {
            get { return _areBonusesVisible; }
            set
            {
                _areBonusesVisible = value;
                RaisePropertyChanged();
                ProcessOnPropertyChanged();
            }
        }

        private bool _aAreDollarsVisible;
        public bool AreDollarsVisible
        {
            get { return _aAreDollarsVisible; }
            set
            {
                _aAreDollarsVisible = value;
                RaisePropertyChanged();
                ProcessOnPropertyChanged();
            }
        }

        private bool _isPrepaymentVisible;
        public bool IsPrepaymentVisible
        {
            get { return _isPrepaymentVisible; }
            set
            {
                _isPrepaymentVisible = value;
                RaisePropertyChanged();
                ProcessOnPropertyChanged();
            }
        }

        private bool _isSicknessVisible;
        public bool IsSicknessVisible
        {
            get { return _isSicknessVisible; }
            set
            {
                _isSicknessVisible = value;
                RaisePropertyChanged();
                ProcessOnPropertyChanged();
            }
        }


        public DefaultListingPdfReportSetting()
        {
            ResetSettings();
        }


        public DefaultListingPdfReportSetting(DefaultListingPdfReportSetting setting)
        {
            IsEmployerVisible = setting.IsEmployerVisible;
            IsOwnerNameVisible = setting.IsOwnerNameVisible;

            AreShortHalfHoursEnabled = setting.AreShortHalfHoursEnabled;
            AreWorkedHoursVisible = setting.AreWorkedHoursVisible;
            AreLunchHoursVisible = setting.AreLunchHoursVisible;
            AreOtherHoursVisible = setting.AreOtherHoursVisible;
            AreTotalWorkedHoursVisible = setting.AreTotalWorkedHoursVisible;
            IsVacationVisible = setting.IsVacationVisible;
            AreSiknessHoursVisible = setting.AreSiknessHoursVisible;
            AreHolidaysHoursVisible = setting.AreHolidaysHoursVisible;

            IsHourlyWageVisible = setting.IsHourlyWageVisible;
            AreVacationDaysVisible = setting.AreVacationDaysVisible;
            AreDietsVisible = setting.AreDietsVisible;
            ArePaidHolidaysVisible = setting.ArePaidHolidaysVisible;
            AreBonusesVisible = setting.AreBonusesVisible;
            AreDollarsVisible = setting.AreDollarsVisible;
            IsPrepaymentVisible = setting.IsPrepaymentVisible;
            IsSicknessVisible = setting.IsSicknessVisible;
        }


        public delegate void PropertyChangedHandler(object sender, EventArgs args);
        public event PropertyChangedHandler OnPropertyChanged;

        private void ProcessOnPropertyChanged()
        {
            if (OnPropertyChanged != null) {
                OnPropertyChanged(this, EventArgs.Empty);
            }
        }


        public bool IsEqual(object obj)
        {
            if (obj == null) {
                return false;
            }

            if (object.ReferenceEquals(this, obj)) {
                return true;
            }

            if (obj.GetType() != this.GetType()) {
                return false;
            }

            DefaultListingPdfReportSetting setting = (DefaultListingPdfReportSetting)obj;

            if (IsEmployerVisible != setting.IsEmployerVisible) { return false; }
            if (IsOwnerNameVisible != setting.IsOwnerNameVisible) { return false; }

            if (AreShortHalfHoursEnabled != setting.AreShortHalfHoursEnabled) { return false; }
            if (AreWorkedHoursVisible != setting.AreWorkedHoursVisible) { return false; }
            if (AreLunchHoursVisible != setting.AreLunchHoursVisible) { return false; }
            if (AreOtherHoursVisible != setting.AreOtherHoursVisible) { return false; }
            if (AreTotalWorkedHoursVisible != setting.AreTotalWorkedHoursVisible) { return false; }
            if (IsVacationVisible != setting.IsVacationVisible) { return false; }
            if (AreSiknessHoursVisible != setting.AreSiknessHoursVisible) { return false; }
            if (AreHolidaysHoursVisible != setting.AreHolidaysHoursVisible) { return false; }

            if (IsHourlyWageVisible != setting.IsHourlyWageVisible) { return false; }
            if (AreVacationDaysVisible != setting.AreVacationDaysVisible) { return false; }
            if (AreDietsVisible != setting.AreDietsVisible) { return false; }
            if (ArePaidHolidaysVisible != setting.ArePaidHolidaysVisible) { return false; }
            if (AreBonusesVisible != setting.AreBonusesVisible) { return false; }
            if (AreDollarsVisible != setting.AreDollarsVisible) { return false; }
            if (IsPrepaymentVisible != setting.IsPrepaymentVisible) { return false; }
            if (IsSicknessVisible != setting.IsSicknessVisible) { return false; }

            return true;
        }


        public void ResetSettings()
        {
            IsEmployerVisible = true;
            IsOwnerNameVisible = true;

            AreShortHalfHoursEnabled = false;
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
