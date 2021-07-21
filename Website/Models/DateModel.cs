using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Website.Properties;

namespace Website.Models
{
    public class DateModel : IValidatableObject
    {
        private const int PeriodoMaximoDias = 365;
        private const string DataInicialDisplay = "Data Inicial";
        private const string DataFinalDisplay = "Data Final";

        public static readonly DateModel Empty = new DateModel();

        private DateTime? dataInicial = null;
        private DateTime? dataFinal = null;

        [Display(Name = DataInicialDisplay)]
        public string DataInicial { get => this.GetDataInicial(); set => this.SetDataInicial(value); }

        [Display(Name = DataFinalDisplay)]
        public string DataFinal { get => this.GetDataFinal(); set => this.SetDataFinal(value); }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //return ((IValidatableObject)Empty).Validate(validationContext);

            if (this.dataInicial.HasValue is false)
                yield return new ValidationResult(String.Format(Messages.RequiredField, DataInicialDisplay), new[] { nameof(DataInicial) });

            if (this.dataFinal.HasValue is false)
                yield return new ValidationResult(String.Format(Messages.RequiredField, DataFinalDisplay), new[] { nameof(DataFinal) });

            if (this.dataInicial.HasValue && this.dataFinal.HasValue)
            {
                if (this.dataInicial.Value > this.dataFinal.Value)
                    yield return new ValidationResult(String.Format(Messages.CannotBeGreaterThan, DataInicialDisplay, DataFinalDisplay), new[] { nameof(DataInicial) });
                else if (dataFinal.Value.Subtract(dataInicial.Value).Days > PeriodoMaximoDias)
                    yield return new ValidationResult(String.Format(Messages.CannotBeMoreThanDays, PeriodoMaximoDias), new[] { nameof(DataFinal) });
            }
        }

        private string GetDataInicial()
        {
            return this.GetDataInicial(Formats.DateStringFormat);
        }

        public string GetDataInicial(string format)
        {
            return String.Format(format, this.dataInicial);
        }

        private void SetDataInicial(string value)
        {
            this.dataInicial = null;
            if (DateTime.TryParseExact(value, Formats.DateString, CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime dateValue))
                this.dataInicial = dateValue;
        }

        private string GetDataFinal()
        {
            return this.GetDataFinal(Formats.DateStringFormat);
        }

        public string GetDataFinal(string format)
        {
            return String.Format(format, this.dataFinal);
        }

        private void SetDataFinal(string value)
        {
            this.dataFinal = null;
            if (DateTime.TryParseExact(value, Formats.DateString, CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime dateValue))
                this.dataFinal = dateValue;
        }
    }
}
