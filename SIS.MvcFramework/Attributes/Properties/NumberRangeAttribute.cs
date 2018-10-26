namespace SIS.MvcFramework.Attributes.Properties
{
    using System.ComponentModel.DataAnnotations;

    public class NumberRangeAttribute : ValidationAttribute
    {
        private readonly double minValue;
        private readonly double maxValue;

        public NumberRangeAttribute(double minValue, double maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        public override bool IsValid(object value)
        {
            double valueAsNumber = (double)value;

            return this.minValue <= valueAsNumber && this.maxValue >= valueAsNumber;
        }
    }
}
